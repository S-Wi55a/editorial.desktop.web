using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using HeroSection = Csn.Retail.Editorial.Web.Features.Details.Models.HeroSection;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    public interface IHeroSectionMapper
    {
        HeroSection Map(ArticleDetailsDto article);
    }

    [AutoBind]
    public class HeroSectionMapper : IHeroSectionMapper
    {
        public HeroSection Map(ArticleDetailsDto article)
        {
            if (article.HeroSection == null) return null;

            return new HeroSection()
            {
                Type = article.HeroSection.Type,
                Images = article.HeroSection.Images,
                BrightcoveVideoId = GetBrightcoveVideoId(article.HeroSection.BrightcoveVideo),
                BrightcoveVideoIFrameUrl = GetBrightcoveVideoIFrameUrl(article.HeroSection.BrightcoveVideo),
                BrightcoveVideoEncodingUrl = GetBrightcoveVideoEncodingUrl(article.HeroSection.BrightcoveVideo),
                Headline = article.Headline,
                SubHeading = article.Subheading
            };
        }

        private string GetBrightcoveVideoId(BrightcoveVideo video)
        {
            return video == null || video.VideoId.IsNullOrEmpty() ? null : video.VideoId;
        }

        private string GetBrightcoveVideoEncodingUrl(BrightcoveVideo video)
        {
            return video == null || video.EncodingUrl.IsNullOrEmpty() ? null : video.EncodingUrl;
        }

        private string GetBrightcoveVideoIFrameUrl(BrightcoveVideo video)
        {
            return video == null ? null : $"https://players.brightcove.net/674523943001/{video.PlayerId}_default/index.html?videoId={video.VideoId}";
        }
    }
}