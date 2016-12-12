using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

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
                BrightcoveVideoIFrameUrl = GetBrightcoveVideoIFrameUrl(article.HeroSection.BrightcoveVideo)
            };
        }

        private string GetBrightcoveVideoIFrameUrl(BrightcoveVideo video)
        {
            return video == null ? null : $"https://players.brightcove.net/674523943001/{video.PlayerId}_default/index.html?videoId={video.VideoId}";
        }
    }
}