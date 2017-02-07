using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;

namespace Csn.Retail.Editorial.Web.Features.Details.Models
{
    public class ArticleViewModel
    {
        public string NetworkId { get; set; }
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
        public EditorialExpertRating ExpertRatings { get; set; }
        public ProCon ProsCons { get; set; }
        public string DateAvailable { get; set; }
        public MediaMotiveData MediaMotiveData { get; set; }
        public DisqusData DisqusData { get; set; }
        public RelatedArticleData RelatedArticleData { get; set; }
    }

    public class MediaMotiveData
    {
        public string KruxId { get; set; }
        public string MediaMotiveDomain { get; set; }
        public List<MMItem> MediaMotiveItem { get; set; }

        public class MMItem
        {
            public string TileId { get; set; }
            public string TileDescription { get; set; }
            public string DataKruxRequired { get; set; }
            public string TileUri { get; set; }
            public TileUrl TileUrls { get; set; }

            public class TileUrl
            {
                public string JServerUrl { get; set; }
                public string AdClickUrl { get; set; }
                public string IServerUrl { get; set; }
            }
        }
    }

    public class HeroSection
    {
        public HeroType Type { get; set; }
        public List<Image> Images { get; set; }
        public string BrightcoveVideoIFrameUrl { get; set; }
        public string NetworkId { get; set; }
        public int ImageIndex { get; set; }
    }

    public class SocialMetaData
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public string SiteName { get; set; }
        public List<string> SocialSharingNetworks { get; set; }

    }

    public class ProCon
    {
        public Pro Pros { get; set; }
        public Con Cons { get; set; }

        public class Pro
        {
            public string Heading { get; set; }
            public List<string> Items { get; set; }
        }

        public class Con
        {
            public string Heading { get; set; }
            public List<string> Items { get; set; }
        }
    }

    public class EditorialExpertRating
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public int OverallRating { get; set; }

        public List<ExpertItem> Items { get; set; }

        public class ExpertItem
        {
            public string Category { get; set; }
            public int Rating { get; set; }
        }
    }

    public class DisqusData
    {
        public string Identifier { get; set; }
        public string ArticleUrl { get; set; }
        public string Title { get; set; }
    }
}