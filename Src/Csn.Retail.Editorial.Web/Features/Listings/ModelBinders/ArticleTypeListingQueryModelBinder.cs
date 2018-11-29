using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Constants;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;

namespace Csn.Retail.Editorial.Web.Features.Listings.ModelBinders
{
    [ModelBinderType(typeof(ArticleTypeListingQuery))]
    public class ArticleTypeListingQueryModelBinder : DefaultModelBinder
    {
        private readonly IArticleTypeListingLookup _articleTypeLookup;

        public ArticleTypeListingQueryModelBinder(IArticleTypeListingLookup articleTypeLookup)
        {
            _articleTypeLookup = articleTypeLookup;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var articleTypeRouteValue = bindingContext.ValueProvider.GetValue("articleType");

            if (articleTypeRouteValue == null) return null;

            var articleType = _articleTypeLookup.GetArticleTypeFromSlug(articleTypeRouteValue.AttemptedValue.Trim('/'));

            if (!articleType.HasValue) return null;

            return new ArticleTypeListingQuery
            {
                ArticleType = articleType.Value,
                Pg = bindingContext.ValueProvider.TryGetValueOrDefault(EditorialQueryStringParams.Offset, 0),
                Sb = bindingContext.ValueProvider.TryGetValueOrDefault(EditorialQueryStringParams.Sort, string.Empty),
            };
        }
    }
}