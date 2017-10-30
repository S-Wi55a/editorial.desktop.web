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
        private readonly IBrightcoveIFrameBuilder _brightcoveIFrameBuilder;

        public HeroSectionMapper(IBrightcoveIFrameBuilder brightcoveIFrameBuilder)
        {
            _brightcoveIFrameBuilder = brightcoveIFrameBuilder;
        }

        public HeroSection Map(ArticleDetailsDto article)
        {
            if (article.HeroSection == null) return null;

            return new HeroSection()
            {
                Type = article.HeroSection.Type,
                Images = article.HeroSection.Images,
                BrightcovePlayerId = GetBrightcovePlayerId(article.HeroSection.BrightcoveVideo),
                BrightcoveVideoId = GetBrightcoveVideoId(article.HeroSection.BrightcoveVideo),
                BrightcoveVideoIFrameUrl = _brightcoveIFrameBuilder.Build(article.HeroSection.BrightcoveVideo, article.NetworkId),
                BrightcoveVideoEncodingUrl = GetBrightcoveVideoEncodingUrl(article.HeroSection.BrightcoveVideo),
                Headline = article.Headline,
                SubHeading = article.Subheading,
                NetworkId = article.NetworkId
            };
        }

        private string GetBrightcovePlayerId(BrightcoveVideo video)
        {
            return video == null || video.PlayerId.IsNullOrEmpty() ? null : video.PlayerId;
        }

        private string GetBrightcoveVideoId(BrightcoveVideo video)
        {
            return video == null || video.VideoId.IsNullOrEmpty() ? null : video.VideoId;
        }

        private string GetBrightcoveVideoEncodingUrl(BrightcoveVideo video)
        {
            return video == null || video.EncodingUrl.IsNullOrEmpty() ? null : video.EncodingUrl;
        }
    }
}