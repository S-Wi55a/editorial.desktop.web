using System.Web.Mvc;
using Autofac.Integration.Mvc;

namespace Csn.Retail.Editorial.Web.Features.Listings.ModelBinders
{
    [ModelBinderType(typeof(ArticleTypeListingQuery))]
    public class ArticleTypeListingQueryModelBinder : DefaultModelBinder
    {
        private readonly IArticleTypeLookup _articleTypeLookup;

        public ArticleTypeListingQueryModelBinder(IArticleTypeLookup articleTypeLookup)
        {
            _articleTypeLookup = articleTypeLookup;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var articleSlugRouteValue = bindingContext.ValueProvider.GetValue("articleSlug");

            if (articleSlugRouteValue == null) return null;

            var articleType = _articleTypeLookup.GetArticleTypeFromSlug(articleSlugRouteValue.AttemptedValue);

            if (!articleType.HasValue) return null;

            return new ArticleTypeListingQuery
            {
                ArticleType = articleType.Value
            };
        }
    }
}