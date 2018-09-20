using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations;
using Csn.Retail.Editorial.Web.Features.Shared.Hero.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;

namespace Csn.Retail.Editorial.Web.Features.Landing.Models
{
    public class LandingViewModel
    {
        public List<CarouselViewModel> Carousels { get; set; }
        public Nav Nav { get; set; }
        public string Title { get; set; }
        public CampaignAdResult CampaignAd { get; set; }
        public PolarNativeAdsData PolarNativeAdsData { get; set; }
        public Dictionary<string, string> InsightsData { get; set; }
        public string HeroTitle { get; set; }
        public string HeroImage { get; set; }
        public SeoData SeoData { get; set; }
    }

    public class CarouselViewModel
    {
        public string Title { get; set; }
        public List<SearchResult> CarouselItems { get; set; }
        public string ViewAllLink { get; set; }
        public bool HasMrec { get; set; }
        public string NextQuery { get; set; }
        public CarouselTypes CarouselType { get; set; }
        public PolarAds PolarAds { get; set; }
        public bool HasNativeAd { get; set; }
    }

    public class Nav
    { 
        public NavResult NavResults { get; set; }
        public string DisqusSource { get; set; }

    }
}