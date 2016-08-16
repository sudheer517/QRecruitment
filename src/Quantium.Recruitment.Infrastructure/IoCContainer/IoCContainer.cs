using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Quantium.Recruitment.Infrastructure.Repositories;

namespace Quantium.Recruitment.Infrastructure.IoCContainer
{
    public class IoCContainer
    {
        private static IConfigurationRoot _configuration { get; set; }

        public static IServiceCollection ConfigureServices(IConfigurationRoot configuration)
        {
            var services = new ServiceCollection();

            _configuration = configuration;
            services.AddSingleton(_configuration);
            services.AddSingleton<IRecruitmentContext, RecruitmentContext>();
            services.AddTransient<IQuestionRepository, QuestionRepository>();

            return services;
        }
    }
}
