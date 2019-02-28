using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    public interface ISeoDataMapper
    {
        SeoData Map(Shared.Proxies.EditorialApi.SeoData seoData);
    }

    [AutoBind]
    public class SeoDataMapper: ISeoDataMapper
    {
        public SeoData Map(Shared.Proxies.EditorialApi.SeoData seoData)
        {
            if (seoData == null) return null;
            return new SeoData
            {
                Title = seoData.Title,
                AlternateUrl = seoData.AlternateUrl,
                CanonicalUrl = seoData.CanonicalUrl,
                CanonicalAmpUrl = seoData.CanonicalAmpUrl,
                Description = seoData.Description,
                Keywords = seoData.Keywords
            };
        }
    }
}