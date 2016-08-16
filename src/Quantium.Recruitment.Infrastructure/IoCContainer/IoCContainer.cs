using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Quantium.Recruitment.Infrastructure.Repositories;
using Quantium.Recruitment.Infrastructure.RDbContext;

namespace Quantium.Recruitment.Infrastructure.IoCContainer
{
    public class IoCContainer
    {
        public static IServiceCollection ConfigureServices(IConfigurationRoot configuration)
        {
            var services = new ServiceCollection();

            services.AddSingleton(configuration);
            services.AddSingleton<IRecruitmentContext, RecruitmentContext>();
            services.AddTransient<IQuestionRepository, QuestionRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITestRepository, TestRepository>();

            return services;
        }
    }
}
