using Reinforced.Typings.Attributes;

namespace Quantium.Recruitment.ApiServices.Models
{
    [TsClass]
    public class LabelDto : Identifiable
    {
        public string Name { get; set; }

        public long JobId { get; set; }

        public long TestId { get; set; }

        //public JobDto Job { get; set; }

        //public TestDto Test { get; set; }
    }
}
