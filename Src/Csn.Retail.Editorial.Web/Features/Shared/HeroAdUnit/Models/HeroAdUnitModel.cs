using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Csn.Retail.Editorial.Web.Features.Shared.HeroAdUnit.Models
{
    public class CampaignAdResult
    {
        public CampaignAdResult()
        {
            LeftSection = new CampaignSection();
            RightSection = new CampaignSection();
            Images = new CampaignImages();
        }

        public string Name { get; set; }
        public bool IsDefaultCampaign { get; set; }
        public CampaignSection LeftSection { get; set; }
        public CampaignSection RightSection { get; set; }

        public CampaignImages Images { get; set; }
    }

    public class CampaignImages
    {
        public string Desktop { get; set; }
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
}