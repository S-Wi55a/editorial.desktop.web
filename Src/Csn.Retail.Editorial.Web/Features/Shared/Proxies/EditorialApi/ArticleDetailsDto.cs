using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi
{
    public class ArticleDetailsDto
    {
        public string NetworkId { get; set; }
        public ArticleTemplateType ArticleTemplateType { get; set; }
        public string ArticleType { get; set; }
        public bool IsSponsoredArticle { get; set; }
        public HeroSection HeroSection { get; set; }
        public string Headline { get; set; }
        public string Subheading { get; set; }
        public string Summary { get; set; }
        public string SubType { get; set; }
        public string ReviewLocation { get; set; }
        public string MakeModelHeading { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Lifestyles { get; set; }
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
        public MoreArticleData MoreArticleData { get; set; }
        public SeoData SeoData { get; set; }
        public Dictionary<string, string> InsightsData { get; set; }
        public StockListingData StockListingData { get; set; }
        public AlsoConsiderData AlsoConsiderData { get; set; }
        public string SpecDataGetVariantsUrl { get; set; }
    }

    public class DisqusData
    {
        public string DisqusSource { get; set; }
        public string Identifier { get; set; }
        public string ArticleUrl { get; set; }
        public string Title { get; set; }
    }

    public class MediaMotiveData
    {
        public string KruxId { get; set; }
        public string BaseUrl { get; set; }
        public string CommonTags { get; set; }
        public string ScriptPath { get; set; } // JServer
        public string NoScriptPath { get; set; } // AdClick
        public string NoScriptImagePath { get; set; } // IServer
        public Dictionary<string, MediaMotiveAdUnit> AdUnits { get; set; }
        public bool ShowSponsoredTextLinks { get; set; }
    }

    public class MediaMotiveAdUnit
    {
        public string TileId { get; set; }
        public string Description { get; set; }
        public bool DataKruxRequired { get; set; }
        public string Tags { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
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

    public class PriceModel
    {
        public string PriceText { get; set; }
        public string PriceLabel { get; set; }
        public string PriceDisclaimer { get; set; }
    }

    public class EditorialItem
    {
        public string Badge { get; set; }
        public string BodyStyleCategory { get; set; }
        public string BodyStyleSubCategory { get; set; }
        public string Category { get; set; }
        public string EngineCapacity { get; set; }
        public string Make { get; set; }
        public string MarketingGroup { get; set; }
        public string Model { get; set; }
        public string Series { get; set; }
        public string SubCategory { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
    }

    public class HeroSection
    {
        public HeroType Type { get; set; }
        public List<Image> Images { get; set; }
        public BrightcoveVideo BrightcoveVideo { get; set; }

    }

    public class Contributor
    {
        public string ButtonHeading { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Bio { get; set; }
        public string LinkUrl { get; set; }
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

    public class SeoData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CanonicalUrl { get; set; }
        public string AlternateUrl { get; set; }
        public string Keywords { get; set; }
    }

    public enum ArticleTemplateType
    {
        Standard,
        Wide
    }

    public enum HeroType
    {
        SingleImage,
        DoubleImage,
        MultipleImages,
        Video,
        ImagesAndVideo,
        WideImage,
        WideVideo
    }

    public class Image
    {
        public string Url { get; set; }
        public string AlternateText { get; set; }
        public string Caption { get; set; }
        public string Type { get; set; }
    }

    public class BrightcoveVideo
    {
        public string VideoId { get; set; }
        public string PlayerId { get; set; }
        public string EncodingUrl { get; set; }
    }

    // this captures all the possible properties across all the content section
    // types. Makes mapping from the response easier than mapping to specific
    // types for each section type. Need to make sure that all properties added
    // here are nullable
    public class ContentSection
    {
        public ContentSectionType SectionType { get; set; }

        public string Content { get; set; }

        public string IFrameUrl { get; set; }

        public Image Image { get; set; }
    }

    public enum ContentSectionType
    {
        Html,
        SingleImage,
        BrightcoveVideo,
        Quote
    }

    public class StockListingData
    {
        public string Heading { get; set; }
        public List<StockFilters> Filters { get; set; }
        public string ViewAllStockButton { get; set;  }
    }

    public class AlsoConsiderData
    {
        public string Uri { get; set; }
    }
}