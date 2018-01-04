using System;
using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Newtonsoft.Json;

namespace Csn.Retail.Editorial.Web.Features.Landing.Models
{
    public class LandingViewModel
    {
        public List<CarouselViewModel> Carousels { get; set; }
        public Nav Nav { get; set; }
        public CampaignAdResult CampaignAd { get; set; }
        public string Title { get; set; }
    }

    public class CarouselViewModel
    {
        public string Title { get; set; }
        public List<SearchResult> CarouselItems { get; set; }
        public string ViewAllLink { get; set; }
        public bool HasMrec { get; set; }
        public string NextQuery { get; set; }
    }

    public class Nav
    { 
        public NavResult NavResults { get; set; }
    }

    public class CampaignAdResult
    {
        public CampaignAdResult()
        {
            LeftSection = new CampaignSection();
            RightSection = new CampaignSection();
        }

        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsDefaultCampaign { get; set; }

        public CampaignSection LeftSection { get; set; }
        public CampaignSection RightSection { get; set; }

        public string ImageUrl { get; set; }

        public string ImpressionTrackerUrl { get; set; }
        public string ClickTrackerUrl { get; set; }

        [JsonIgnore]
        public int TrackerRandomNumber { get; set; }
    }

    public class CampaignSection
    {
        public string Title { get; set; }

        public string LinkText { get; set; }

        public string LinkUrl { get; set; }

        //TODO: Remove this hack code when campaign completed. This property should come from CMS
        public string LinkTarget
        {
            get
            {
                if (LinkUrl == null) return "_self";
                return LinkUrl.IndexOf("FCID=382572", StringComparison.OrdinalIgnoreCase) == -1 ? "_self" : "_blank";
            }
        }

        [JsonIgnore]
        public bool IsShown => !string.IsNullOrWhiteSpace(Title) || !string.IsNullOrWhiteSpace(LinkText);

        [JsonIgnore]
        public bool IsLinked => !string.IsNullOrWhiteSpace(LinkUrl);
    }

    public class HomePageModel
    {
        public CampaignAdResult CampaignAd { get; set; }
    }

}