using System.Collections.Generic;
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
        private static readonly Dictionary<string, ArticleType> _articleTypeLookupFromSlug = new Dictionary<string, ArticleType>
        {
            { "news", ArticleType.News },
            { "reviews", ArticleType.Reviews },
            { "videos", ArticleType.Video },
            { "advice", ArticleType.Advice},
            { "features", ArticleType.Features },
            { "sponsored", ArticleType.Sponsored },
            { "actualidad", ArticleType.Actualidad },
            { "las-mejores-compras", ArticleType.LasMejoresCompras },
            { "compra-de-auto", ArticleType.CompraDeAuto },
            { "comparativas", ArticleType.Comparativas },
            { "consejos", ArticleType.Consejos },
            { "pruebas", ArticleType.Pruebas }
        };

        private static readonly Dictionary<ArticleType, string> _articleFacetNameLookup = new Dictionary<ArticleType, string>
        {
            { ArticleType.News, "News" },
            { ArticleType.Reviews, "Review" },
            { ArticleType.Video, "Video" },
            { ArticleType.Advice, "Advice"},
            { ArticleType.Features, "Feature" },
            { ArticleType.Sponsored, "Sponsored" },
            { ArticleType.Actualidad, "Actualidad" },
            { ArticleType.LasMejoresCompras, "Las Mejores Compras" },
            { ArticleType.CompraDeAuto, "Compra de Auto" },
            { ArticleType.Comparativas, "Comparativas" },
            { ArticleType.Consejos, "Consejos" },
            { ArticleType.Pruebas, "Pruebas" },
        };

        public ArticleType? GetArticleTypeFromSlug(string articleTypeSlug)
        {
            if (!_articleTypeLookupFromSlug.TryGetValue(articleTypeSlug, out var articleType))
            {
                return null;
            }

            return articleType;
        }

        public string GetFacetNameFromArticleType(ArticleType articleType)
        {
            if (!_articleFacetNameLookup.TryGetValue(articleType, out var facetName))
            {
                return null;
            }

            return facetName;
        }
    }
}