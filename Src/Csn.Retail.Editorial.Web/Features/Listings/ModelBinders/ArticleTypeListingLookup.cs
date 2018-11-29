using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Listings.ModelBinders
{
    public interface IArticleTypeListingLookup
    {
        ArticleType? GetArticleTypeFromSlug(string articleTypeSlug);
        string GetFacetNameFromArticleType(ArticleType articleType);
    }

    /// <summary>
    /// Currently only Sponsored is required here. All other article types are now handled by ryvuss seo fragments
    /// </summary>
    [AutoBindAsSingleton]
    public class ArticleTypeListingLookup : IArticleTypeListingLookup
    {
        private static readonly Dictionary<string, ArticleType> _articleTypeLookupFromSlug = new Dictionary<string, ArticleType>
        {
            { "sponsored", ArticleType.Sponsored }
        };

        private static readonly Dictionary<ArticleType, string> _articleFacetNameLookup = new Dictionary<ArticleType, string>
        {
            { ArticleType.Sponsored, "Sponsored" }
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