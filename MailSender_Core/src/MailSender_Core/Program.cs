using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Quantium.Recruitment.Portal.Helpers;

namespace MailSender_Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Program p = new Program();
            Task.Run(async () =>
            {
                await p.DOWork();
                // Do any async anything you need here without worry
            }).GetAwaiter().GetResult();

        }

        public async Task<bool> DOWork()
        {

                MessageSender _emailSender = new MessageSender();
                var roleManager = this.serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = this.serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                DBHelper dbhelper = new DBHelper(Configuration.GetConnectionString("RecruitmentDB"));

            // To send Email with Credentials
                List<ApplicationUser> clients=dbhelper.FetchClients();
                foreach(ApplicationUser client in clients)
                {
                    string password = this.GeneratePassword();              
                var user =await userManager.FindByNameAsync(client.Email);              
                if (userManager.HasPasswordAsync(user).Result)
                {
                   await userManager.RemovePasswordAsync(user);                   
                }
                IdentityResult passwordAdded = await userManager.AddPasswordAsync(user, password);

                if (passwordAdded.Succeeded)
                    {                       
                             await _emailSender.SendEmailAsync(client.Email, "Credentials for Login", string.Format("Please use below credentials for Login \\n {0} \\n {1}",
                        client.Email, password));
                        dbhelper.UpdateUser(client.Email);                       
                    }

                }

            //To send email to the candidates to whom test was created
            List<Candidate> testClients = dbhelper.FetchTestMailClients();
            foreach (Candidate client in testClients)
            {
                await _emailSender.SendEmailAsync(client.Email, "Test was Generated", string.Format("Hi {0} ,\\n A test was generated for you.Please login and complete the test ",
                       client.Name));
                dbhelper.UpdateTestMailClient(client.Email);
            }
                return true;

        } 
        private readonly IServiceProvider serviceProvider;

        public IConfigurationRoot Configuration { get; private set; }

        public string GeneratePassword()
        {
            Random randomizer = new Random();
            List<char> pwd = new List<char>();
            for (int i = 0; i < 3; i++)
            {
                pwd.Add((char)randomizer.Next(97, 122));
            }
            pwd.Add((char)randomizer.Next(65, 90));
            pwd.Add((char)randomizer.Next(35, 46));
            for (int i = 0; i < 3; i++)
            {
                pwd.Add((char)randomizer.Next(48, 57));
            }
            return new string(pwd.ToArray());
        }
        public Program()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();           

            var services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            // Register EntityFramework 7
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));

            // Register UserManager & RoleManager
            services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            // UserManager & RoleManager require logging and HttpContext dependencies
            services.AddLogging();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
