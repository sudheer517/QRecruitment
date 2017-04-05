using System.IO;
using System.Security.Cryptography.X509Certificates;
using AspNetCoreSpa.Server.Entities;
using AspNetCoreSpa.Server.Filters;
using AspNetCoreSpa.Server.Services;
using AspNetCoreSpa.Server.Services.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using System.Threading.Tasks;
using Quantium.Recruitment.Infrastructure.Repositories;
using AspNetCoreSpa.Server.Repositories.Abstract;
using Quantium.Recruitment.Entities;
using Newtonsoft.Json.Serialization;
using Quantium.Recruitment.Portal.Helpers;
using Quantium.Recruitment.Portal.Server.Helpers;
using Quantium.Recruitment.Portal.Server.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreSpa.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSslCertificate(this IServiceCollection services, IHostingEnvironment hostingEnv)
        {
            //var cert = new X509Certificate2(Path.Combine(hostingEnv.ContentRootPath, "extra", "cert.pfx"), "game123");

            //services.Configure<KestrelServerOptions>(options =>
            //{
            //    options.UseHttps(cert);

            //});

            return services;
        }
        public static IServiceCollection AddCustomizedMvc(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ModelValidationFilter));
                options.Filters.Add(new RequireHttpsAttribute());
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            return services;
        }
        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            // For api unauthorised calls return 401 with no body
            services.AddIdentity<QRecruitmentUser, QRecruitmentRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Cookies.ApplicationCookie.LoginPath = "/";
                options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/Home") &&
                            ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                        {
                            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                        else if (ctx.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                        {
                            ctx.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        return Task.FromResult(0);
                    }
                };
            })
            .AddEntityFrameworkStores<ApplicationDbContext, int>()
            .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Startup.Configuration["Data:SqlServerConnectionString"]);

            });
            return services;
        }
        public static IServiceCollection RegisterCustomServices(this IServiceCollection services)
        {
            // New instance every time, only configuration class needs so its ok
            services.Configure<SmsSettings>(options => Startup.Configuration.GetSection("SmsSettingsTwillio").Bind(options));
            services.AddTransient<UserResolverService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SmsSender>();
            services.AddScoped<ApiExceptionFilter>();

            services.AddTransient<IEntityBaseRepository<Admin>, AdminRepository>();
            services.AddTransient<IEntityBaseRepository<Candidate>, CandidateRepository>();
            services.AddTransient<IEntityBaseRepository<Department>, DepartmentRepository>();
            services.AddTransient<IEntityBaseRepository<Question>, QuestionRepository>();
            services.AddTransient<IEntityBaseRepository<Label>, LabelRepository>();
            services.AddTransient<IEntityBaseRepository<Difficulty>, DifficultyRepository>();
            services.AddTransient<IEntityBaseRepository<QuestionGroup>, QuestionGroupRepository>();
            services.AddTransient<IEntityBaseRepository<Job>, JobRepository>();
            services.AddTransient<IEntityBaseRepository<Test>, TestRepository>();
            services.AddTransient<IEntityBaseRepository<Candidate_Job>, CandidateJobRepository>();
            services.AddTransient<IEntityBaseRepository<Challenge>, ChallengeRepository>();
            services.AddTransient<IEntityBaseRepository<Job_Difficulty_Label>, JobDifficultyLabelRepository>();
            services.AddTransient<IEntityBaseRepository<Option>, OptionRepository>();
            services.AddTransient<IEntityBaseRepository<Feedback>, FeedbackRepository>();
            services.AddTransient<IEntityBaseRepository<FeedbackType>, FeedbackTypeRepository>();

            services.AddTransient<IHttpHelper, HttpHelper>();
            services.AddTransient<IAccountHelper, AccountHelper>();

            return services;
        }
    }
}
