using System.Collections.Generic;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Listings.ModelBinders
{
    public interface IArticleTypeLookup
    {
        ArticleType? GetArticleTypeFromSlug(string articleTypeSlug);
        string GetFacetNameFromArticleType(ArticleType articleType);
    }

    [AutoBindAsSingleton]
    public class ArticleTypeLookup : IArticleTypeLookup
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        private static readonly Dictionary<string, ArticleType> _articleTypeLookupFromSlug = new Dictionary<string, ArticleType>
        {
            { "news", ArticleType.News },
            { "reviews", ArticleType.Reviews },
            { "videos", ArticleType.Video },
            { "advice", ArticleType.Advice},
            { "features", ArticleType.Features },
            { "riding-advice", ArticleType.RidingAdvice},
            { "engine-reviews", ArticleType.Engine },
            { "products", ArticleType.Products },
            { "tips", ArticleType.Tips },
            { "tow-tests", ArticleType.TowTests },
            { "motoracing", ArticleType.Motoracing },
            { "sponsored", ArticleType.Sponsored }
        };

        private static readonly Dictionary<ArticleType, string> _articleFacetNameLookup = new Dictionary<ArticleType, string>
        {
            { ArticleType.News, "News" },
            { ArticleType.Reviews, "Review" },
            { ArticleType.Video, "Video" },
            { ArticleType.Advice, "Advice"},
            { ArticleType.Features, "Feature" },
            { ArticleType.RidingAdvice, "Riding Advice"},
            { ArticleType.Engine, "Engine Review" },
            { ArticleType.Product, "Product" },
            { ArticleType.Products, "Products" },
            { ArticleType.Tips, "Tips" },
            { ArticleType.TowTests, "Tow Test" },
            { ArticleType.Motoracing, "MotoRacing" },
            { ArticleType.Sponsored, "Sponsored" }
        };

        public ArticleTypeLookup(ITenantProvider<TenantInfo> tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        public ArticleType? GetArticleTypeFromSlug(string articleTypeSlug)
        {
            if (!_articleTypeLookupFromSlug.TryGetValue(articleTypeSlug, out var articleType))
            {
                return null;
            }

            if(articleType == ArticleType.Advice && _tenantProvider.Current().Name == "carsales")
            {
                return ArticleType.CarAdvice;
            }

            return articleType;
        }

        public string GetFacetNameFromArticleType(ArticleType articleType)
        {
            if (articleType == ArticleType.CarAdvice && _tenantProvider.Current().Name == "carsales")
            {
                return "Car Advice";
            }

            if (!_articleFacetNameLookup.TryGetValue(articleType, out var facetName))
            {
                return null;
            }

            return facetName;
        }
    }
}