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
        public int Order { get; set; }
        public string Url { get; set; }
        public string AlternateText { get; set; }
        public string Caption { get; set; }
    }

    public class BrightcoveVideo
    {
        public string VideoId { get; set; }
        public string PlayerId { get; set; }
    }
}