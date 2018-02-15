using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
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
                Headline = article.Headline,
                SubHeading = article.Subheading,
                NetworkId = article.NetworkId,
                BrightcoveVideo = article.HeroSection.BrightcoveVideo
            };
        }
    }
}