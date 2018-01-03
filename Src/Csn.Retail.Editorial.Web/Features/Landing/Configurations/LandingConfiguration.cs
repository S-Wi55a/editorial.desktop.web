using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.Landing.Configurations
{
    public class LandingConfig
    {
        public List<LandingConfigurationSet> Configs { get; set; }
    }

    public class LandingConfigurationSet
    {
        public string Type { get; set; }
        public List<LandingCarousel> Carousels { get; set; }
    }

    public class LandingCarousel
    {
        public string Title { get; set; }
        public bool DisplayMrec { get; set; }
        public string Query { get; set; }
        public string Sort { get; set; }
        public string ViewAll { get; set; }
    }
}