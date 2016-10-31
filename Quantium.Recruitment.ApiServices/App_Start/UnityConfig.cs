using Microsoft.Practices.Unity;
using Quantium.Recruitment.Infrastructure;
using System.Web.Http;
using Quantium.Recruitment.Infrastructure.Repositories;
using Unity.WebApi;
using System.Web.Http.Dependencies;
using System.Web;

namespace Quantium.Recruitment.ApiServices
{
    public static class UnityConfig
    {
        public static IDependencyResolver RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IRecruitmentContext, RecruitmentContext>(new PerRequestLifetimeManager());
            container.RegisterType<IConnectionString, ConnectionString>();
            container.RegisterType<ICandidateRepository, CandidateRepository>();
            container.RegisterType<IAdminRepository, AdminRepository>();
            container.RegisterType<IDepartmentRepository, DepartmentRepository>();

            container.RegisterType<IQuestionRepository, QuestionRepository>();
            container.RegisterType<IOptionRepository, OptionRepository>();
            container.RegisterType<ILabelRepository, LabelRepository>();
            container.RegisterType<IDifficultyRepository, DifficultyRepository>();
            container.RegisterType<IJobLabelDifficultyRepository, JobLabelDifficultyRepository>();
            container.RegisterType<IQuestionGroupRepository, QuestionGroupRepository>();

            container.RegisterType<ITestRepository, TestRepository>();
            container.RegisterType<IJobRepository, JobRepository>();
            container.RegisterType<ICandidateJobRepository, CandidateJobRepository>();
            container.RegisterType<IChallengeRepository, ChallengeRepository>(new TransientLifetimeManager());
            container.RegisterType<ICandidateSelectedOptionRepository, CandidateSelectedOptionRepository>();

            return new UnityDependencyResolver(container);
        }
    }

    public class PerRequestLifetimeManager : LifetimeManager
    {
        private readonly object key = new object();

        public override object GetValue()
        {
            if (HttpContext.Current != null &&
                HttpContext.Current.Items.Contains(key))
                return HttpContext.Current.Items[key];
            else
                return null;
        }

        public override void RemoveValue()
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Items.Remove(key);
        }

        public override void SetValue(object newValue)
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Items[key] = newValue;
        }
    }
}