using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    public interface IArticleTypeLabelMapper
    {
        ArticleTypeLabel Map(ArticleDetailsDto source);
    }

    [AutoBind]
    public class ArticleTypeLabelMapper : IArticleTypeLabelMapper
    {
        private const string SponsoredInfoHtml = "<h2>Sponsored & Advertorial Content</h2><p>In some cases, Carsales will work with advertisers to bring you relevant content that has been made possible by advertisers and their partners, these Ads will be marked 'Sponsored'. Carsales has reviewed the content to ensure it is relevant and of appropriate quality.</p><p><a href='https://help.carsales.com.au/hc/en-gb/articles/208468026-About-Ad-Content-personalisation' target='_blank'>Learn More</a></p>";

        public ArticleTypeLabel Map(ArticleDetailsDto source)
        {
            return new ArticleTypeLabel
            {
                ArticleTypeText = source.ArticleTypeLabel,
                InfoHtml = source.ArticleTypeLabel == "Sponsored" ? SponsoredInfoHtml : string.Empty
            };
        }
    }
}