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
        public List<LandingCarouselConfiguration> CarouselConfigurations { get; set; }
        public HeroAdSettings HeroAdSettings { get; set; }
    }

    public class LandingCarouselConfiguration
    {
        public string Title { get; set; }
        public bool DisplayMrec { get; set; }
        public bool DisplayNativeAd { get; set; } // Different to PolarAds, this is COTW, BOTM
        public string Query { get; set; }
        public string Sort { get; set; }
        public string ViewAll { get; set; }
        public PolarAds PolarAds { get; set; }
        public CarouselTypes CarouselType { get; set; }
        public List<CarouselItem> CarouselItems { get; set; }
    }

    public class CarouselItem
    {
        public string ImageUrl { get; set; }
        public string ItemUrl { get; set; }
    }

    public enum CarouselTypes
    {
        Article,
        Driver,
        Featured
    }

    public class PolarAds
    {
        public bool Display { get; set; }
        public int PlacementId { get; set; }
    }

    public class HeroAdSettings
    {
        public bool HasHeroAd { get; set; }
        public string HeroTitle { get; set; }
        public string HeroMake { get; set; }
    }
}