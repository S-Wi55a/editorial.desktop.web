using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;

namespace Csn.Retail.Editorial.Web.Features.Details.Models
{
    public class ArticleViewModel
    {
        public string Headline { get; set; }
        public string SubHeading { get; set; }
        public string Summary { get; set; }
        public string ArticleType { get; set; }
        public HeroSection HeroSection { get; set; }
        public List<Contributor> Contributors { get; set; }
        public List<ContentSection> ContentSections { get; set; }
        public SocialMetaData SocialMetaData { get; set; }

        public List<EditorialItem> Items { get; set; }
        public PriceModel Price { get; set; }
        public List<EditorialExpertRating> ExpertRatings { get; set; }
        public List<string> Pros { get; set; }
        public List<string> Cons { get; set; }
        public string DateAvailable { get; set; }

    }

    public class HeroSection
    {
        public HeroType Type { get; set; }
        public List<Image> Images { get; set; }
        public string BrightcoveVideoIFrameUrl { get; set; }
        public string NetworkId { get; set; }
        public int ImageIndex { get; set; }
    }

}