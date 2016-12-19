using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi
{
    public class ArticleDetailsDto
    {
        public ArticleTemplateType ArticleTemplateType { get; set; }
        public string ArticleType { get; set; }
        public HeroSection HeroSection { get; set; }
        public string Headline { get; set; }
        public string Subheading { get; set; }
        public string Summary { get; set; }
        public List<ContentSection> ContentSections { get; set; }
    }

    public class HeroSection
    {
        public HeroType Type { get; set; }
        public List<Image> Images { get; set; }
        public BrightcoveVideo BrightcoveVideo { get; set; }
    }

    public enum ArticleTemplateType
    {
        NarrowHero,
        WideHero
    }

    public enum HeroType
    {
        SingleImage,
        MultipleImages,
        Video,
        ImagesAndVideo
    }

    public class Image
    {
        public string Url { get; set; }
        public string AlternateText { get; set; }
        public string Caption { get; set; }
    }

    public class BrightcoveVideo
    {
        public string VideoId { get; set; }
        public string PlayerId { get; set; }
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
}