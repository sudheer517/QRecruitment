using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreSpa.Server.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Quantium.Recruitment.Entities;
using Quantium.Recruitment.Portal.Server.Entities;

namespace AspNetCoreSpa.Server
{
    public class SeedDbData
    {
        readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly UserManager<QRecruitmentUser> _userManager;
        private readonly RoleManager<QRecruitmentRole> _roleManager;

        public SeedDbData(IWebHost host, ApplicationDbContext context)
        {
            var services = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));
            var serviceScope = services.CreateScope();
            _hostingEnv = serviceScope.ServiceProvider.GetService<IHostingEnvironment>();
            _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<QRecruitmentRole>>();
            _userManager = serviceScope.ServiceProvider.GetService<UserManager<QRecruitmentUser>>();
            _context = context;
            CreateRoles(); // Add roles
            CreateUsers(); // Add users
            //AddLanguagesAndContent();
            AddRecruitmentSeedData();
        }

        private void CreateRoles()
        {
            var rolesToAdd = new List<QRecruitmentRole>(){
                new QRecruitmentRole { Name= "Admin" },
                new QRecruitmentRole { Name= "Candidate"}
            };
            foreach (var role in rolesToAdd)
            {
                if (!_roleManager.RoleExistsAsync(role.Name).Result)
                {
                    _roleManager.CreateAsync(role).Result.ToString();
                }
            }
        }

        private void CreateUsers()
        {
            if (!_context.ApplicationUsers.Any())
            {
                var qAdminUser = new QRecruitmentUser { UserName = Startup.Configuration["RecruitmentAdminEmail"], Email = Startup.Configuration["RecruitmentAdminEmail"], CreatedDate = DateTime.Now, IsEnabled = true };
                var adminUserResult = _userManager.CreateAsync(qAdminUser, Startup.Configuration["RecruitmentAdminPassword"]).Result;
                qAdminUser = _userManager.FindByEmailAsync(Startup.Configuration["RecruitmentAdminEmail"]).Result;
                var adminRoleResult = _userManager.AddToRoleAsync(qAdminUser, "Admin").Result;

                //var qUser = new QRecruitmentUser { UserName = "user@user.com", Email = "user@user.com", CreatedDate = DateTime.Now, IsEnabled = true };
                //var userResult =  _userManager.CreateAsync(qUser, "batman@123").Result;
                //var userRoleResult = _userManager.AddToRoleAsync(qUser, "Candidate").Result;
            }
        }

        private void AddLanguagesAndContent()
        {
            //if (!_context.Languageses.Any())
            //{
            //    _context.Languageses.Add(new Language { Id = 1, Locale = "en", Description = "English" });
            //    _context.Languageses.Add(new Language { Id = 2, Locale = "fr", Description = "Frensh" });
            //    _context.SaveChanges();
            //}

            //if (!_context.Content.Any())
            //{
            //    _context.Content.Add(new Content { Id = 1, Key = "TITLE" });
            //    _context.Content.Add(new Content { Id = 2, Key = "APP_NAV_HOME" });
            //    _context.Content.Add(new Content { Id = 3, Key = "APP_NAV_EXAMPLES" });
            //    _context.Content.Add(new Content { Id = 4, Key = "APP_NAV_LOGIN" });
            //    _context.Content.Add(new Content { Id = 5, Key = "APP_NAV_LOGOUT" });
            //    _context.Content.Add(new Content { Id = 6, Key = "APP_NAV_REGISTER" });
            //    _context.Content.Add(new Content { Id = 7, Key = "APP_NAV_ADMIN" });
            //    _context.SaveChanges();
            //}

            //if (!_context.ContentText.Any())
            //{
            //    _context.ContentText.Add(new ContentText { Text = "Site title", LanguageId = 1, ContentId = 1 });
            //    _context.ContentText.Add(new ContentText { Text = "Titre du site", LanguageId = 2, ContentId = 1 });

            //    _context.ContentText.Add(new ContentText { Text = "Home", LanguageId = 1, ContentId = 2 });
            //    _context.ContentText.Add(new ContentText { Text = "Accueil", LanguageId = 2, ContentId = 2 });

            //    _context.ContentText.Add(new ContentText { Text = "Examples", LanguageId = 1, ContentId = 3 });
            //    _context.ContentText.Add(new ContentText { Text = "Exemples", LanguageId = 2, ContentId = 3 });

            //    _context.ContentText.Add(new ContentText { Text = "Login", LanguageId = 1, ContentId = 4 });
            //    _context.ContentText.Add(new ContentText { Text = "S'identifier", LanguageId = 2, ContentId = 4 });

            //    _context.ContentText.Add(new ContentText { Text = "Logout", LanguageId = 1, ContentId = 5 });
            //    _context.ContentText.Add(new ContentText { Text = "Connectez - Out", LanguageId = 2, ContentId = 5 });

            //    _context.ContentText.Add(new ContentText { Text = "Register", LanguageId = 1, ContentId = 6 });
            //    _context.ContentText.Add(new ContentText { Text = "registre", LanguageId = 2, ContentId = 6 });

            //    _context.ContentText.Add(new ContentText { Text = "Admin", LanguageId = 1, ContentId = 7 });
            //    _context.ContentText.Add(new ContentText { Text = "Admin", LanguageId = 2, ContentId = 7 });

            //    _context.SaveChanges();
            //}
        }

        private void AddRecruitmentSeedData()
        {
            #region Department
            Department softwareDepartment = new Department() { Name = "Software" };
            Department analyticsDepartment = new Department() { Name = "Analytics" };

            _context.Departments.Add(softwareDepartment);
            _context.Departments.Add(analyticsDepartment);

            #endregion Department

            #region Admin

            Admin admin1 = new Admin()
            {
                FirstName = "Bruce",
                LastName = "Wayne",
                Email = "admin@admin.com",
                IsActive = true,
                Mobile = 9642013699,
                Department = softwareDepartment,
                PasswordSent = true,
            };

            //Admin admin2 = new Admin()
            //{
            //    FirstName = "Rakesh",
            //    LastName = "Rohan",
            //    Email = "Rakesh.Aitipamula@quantium.co.in",
            //    IsActive = true,
            //    Mobile = 9052791243,
            //    Department = softwareDepartment,
            //    PasswordSent = true
            //};

            //Admin admin3 = new Admin()
            //{
            //    FirstName = "Ravi",
            //    LastName = "Bhaskar",
            //    Email = "Ravi.Bhaskar@quantium.co.in",
            //    IsActive = true,
            //    Mobile = 7799814877,
            //    Department = softwareDepartment,
            //    PasswordSent = true
            //};

            //Admin admin4 = new Admin()
            //{
            //    FirstName = "Mit",
            //    LastName = "Suthar",
            //    Email = "Mit.Suthar@quantium.co.in",
            //    IsActive = true,
            //    Mobile = 8886008855,
            //    Department = softwareDepartment,
            //    PasswordSent = true
            //};

            //Admin admin5 = new Admin()
            //{
            //    FirstName = "Babul",
            //    LastName = "Reddy",
            //    Email = "Babul.Yasa@quantium.co.in",
            //    IsActive = true,
            //    Mobile = 9618415060,
            //    Department = softwareDepartment,
            //    PasswordSent = true
            //};

            Admin admin6 = new Admin()
            {
                FirstName = "Jayaram",
                LastName = "Putineedi",
                Email = "Jayaram.Putineedi@quantium.co.in",
                IsActive = true,
                Mobile = 9542749797,
                Department = softwareDepartment,
                PasswordSent = true
            };

            Admin admin7 = new Admin()
            {
                FirstName = "Samyuktha",
                LastName = "Kodali",
                Email = "Samyuktha.Kodali@quantium.co.in",
                IsActive = true,
                Mobile = 9651525567,
                Department = softwareDepartment,
                PasswordSent = true
            };

            //Admin admin8 = new Admin()
            //{
            //    FirstName = "Rakesh",
            //    LastName = "Rohan",
            //    Email = "rkshrohan@gmail.com",
            //    IsActive = true,
            //    Mobile = 9052791243,
            //    Department = softwareDepartment,
            //    PasswordSent = true
            //};

            Admin admin9 = new Admin()
            {
                FirstName = "Ashwini",
                LastName = "Maddala",
                Email = "ashwini.maddala@gmail.com",
                IsActive = true,
                Mobile = 9966654926,
                Department = softwareDepartment,
                PasswordSent = true
            };

            Admin admin10 = new Admin()
            {
                FirstName = "Banu",
                LastName = "Saladi",
                Email = "bhanu499@gmail.com",
                IsActive = true,
                Mobile = 9642013699,
                Department = softwareDepartment,
                PasswordSent = true
            };


            //Admin admin11 = new Admin()
            //{
            //    FirstName = "Kannan",
            //    LastName = "Perumal",
            //    Email = "kannan.perumal@quantium.co.in",
            //    IsActive = true,
            //    Mobile = 8886008855,
            //    Department = softwareDepartment,
            //    PasswordSent = true
            //};

            _context.Admins.Add(admin1);
            //_context.Admins.Add(admin2);
            //_context.Admins.Add(admin3);
            //_context.Admins.Add(admin4);
            //_context.Admins.Add(admin5);
            _context.Admins.Add(admin6);
            _context.Admins.Add(admin7);
            //_context.Admins.Add(admin8);
            _context.Admins.Add(admin9);
            _context.Admins.Add(admin10);
            //_context.Admins.Add(admin11);

            #endregion Admin

            #region Candidate

            //Candidate candidate1 = new Candidate
            //{
            //    FirstName = "Aman",
            //    LastName = "Agarwal",
            //    Email = "aman.agarwal@gmail.com",
            //    Mobile = 9595959595,
            //    IsActive = true,
            //    PasswordSent = true,
            //    TestMailSent = 1,
            //    CreatedUtc = DateTime.UtcNow,
            //    Admin = admin1,
            //};

            //Candidate candidate2 = new Candidate
            //{
            //    FirstName = "Raj",
            //    LastName = "Kundal",
            //    Email = "Raj.Kundal@gmail.com",
            //    Mobile = 9595958885,
            //    IsActive = true,
            //    PasswordSent = true,
            //    TestMailSent = 1,
            //    CreatedUtc = DateTime.UtcNow,
            //    Admin = admin1,
            //};

            //Candidate candidate3 = new Candidate
            //{
            //    FirstName = "Pooja",
            //    LastName = "Sharma",
            //    Email = "Pooja.Sharma41@gmail.com",
            //    Mobile = 96759987453,
            //    IsActive = true,
            //    PasswordSent = true,
            //    TestMailSent = 1,
            //    CreatedUtc = DateTime.UtcNow,
            //    Admin = admin1,
            //};

            //Candidate candidate4 = new Candidate
            //{
            //    FirstName = "Rakesh1",
            //    LastName = "Rohan1",
            //    Email = "rakeshrohan@outlook.com",
            //    Mobile = 9242526667,
            //    IsActive = true,
            //    PasswordSent = true,
            //    TestMailSent = 1,
            //    CreatedUtc = DateTime.UtcNow,
            //};

            //Candidate candidate5 = new Candidate
            //{
            //    FirstName = "Rakesh2",
            //    LastName = "Rohan2",
            //    Email = "rakesh.rohan@outlook.com",
            //    Mobile = 9010456746,
            //    IsActive = true,
            //    PasswordSent = true,
            //    TestMailSent = 1,
            //    CreatedUtc = DateTime.UtcNow,
            //};

            //Candidate candidate6 = new Candidate
            //{
            //    FirstName = "Firefist",
            //    LastName = "Ace",
            //    Email = "0firefist0@gmail.com",
            //    Mobile = 9052791243,
            //    IsActive = true,
            //    PasswordSent = true,
            //    TestMailSent = 1,
            //    CreatedUtc = DateTime.UtcNow,
            //};

            //Candidate candidate7 = new Candidate
            //{
            //    FirstName = "Po",
            //    LastName = "Chungwa",
            //    Email = "user@user.com",
            //    Mobile = 9052791243,
            //    IsActive = true,
            //    PasswordSent = true,
            //    TestMailSent = 1,
            //    CreatedUtc = DateTime.UtcNow,
            //    Admin = admin1,
            //};

            //_context.Candidates.Add(candidate1);
            //_context.Candidates.Add(candidate2);
            //_context.Candidates.Add(candidate3);
            //_context.Candidates.Add(candidate4);
            //_context.Candidates.Add(candidate5);
            //_context.Candidates.Add(candidate6);
            //_context.Candidates.Add(candidate7);

            #endregion Candidate

            #region Label

            Label label1 = new Label
            {
                Name = "C#",
            };

            Label label2 = new Label
            {
                Name = "SQL",
            };

            Label label3 = new Label
            {
                Name = "R",
            };

            Label label4 = new Label
            {
                Name = "Modelling",
            };

            Label label5 = new Label
            {
                Name = "Others",
            };

            _context.Labels.Add(label1);
            _context.Labels.Add(label2);
            _context.Labels.Add(label3);
            _context.Labels.Add(label4);
            _context.Labels.Add(label5);

            #endregion Label

            #region Difficulty

            Difficulty diff1 = new Difficulty
            {
                Name = "Easy",
            };

            Difficulty diff2 = new Difficulty
            {
                Name = "Medium",
            };

            Difficulty diff3 = new Difficulty
            {
                Name = "Hard",
            };

            _context.Difficulties.Add(diff1);
            _context.Difficulties.Add(diff2);
            _context.Difficulties.Add(diff3);

            #endregion Difficulty

            _context.SaveChanges();
        }

    }
}
