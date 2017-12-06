using System.Web;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Csn.Retail.Editorial.Web.Features.Listings.Constants;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.WebMetrics.Core.Model;
using Expresso.Syntax;

namespace Csn.Retail.Editorial.Web.Features.Listings.ModelBinders
{
    [ModelBinderType(typeof(GetListingsQuery))]
    public class GetListingsQueryModelBinder : DefaultModelBinder
    {
        private readonly IExpressionParser _parser;

        public GetListingsQueryModelBinder(IExpressionParser parser)
        {
            _parser = parser;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var query = bindingContext.ValueProvider.TryGetValueOrDefault(ListingsQueryStringParams.Query, string.Empty);
            var seoFragment = bindingContext.ValueProvider.TryGetValueOrDefault("seoFragment", "");

            return new GetListingsQuery()
            {
                Query = query,
                SeoFragment = !string.IsNullOrEmpty(seoFragment) ? $"/{seoFragment}" : string.Empty,
                Offset = bindingContext.ValueProvider.TryGetValueOrDefault(ListingsQueryStringParams.Offset, 0),
                Sort = bindingContext.ValueProvider.TryGetValueOrDefault(ListingsQueryStringParams.Sort, string.Empty),
                SearchEventType = GetActionType(),
                QueryExpression = string.IsNullOrEmpty(query) ? null : _parser.TryParse(query),
                IsLandingPage = false
            };
        }

        private SearchEventType GetActionType()
        {
            var uri = HttpContext.Current.Request.Url;
            var uriReferrer = HttpContext.Current.Request.UrlReferrer;

            // If it is a listing url...
            if (uriReferrer == null || (!uriReferrer.AbsolutePath.Contains("/results") && !uriReferrer.AbsolutePath.Contains("/beta-results")))
                return SearchEventType.Search;

            // Check if it's sorting
            var curSort = uri.GetQueryParameter(ListingsQueryStringParams.Sort);
            var prvSort = uriReferrer.GetQueryParameter(ListingsQueryStringParams.Sort);
            if (curSort != null && curSort != prvSort && (curSort != EditorialSortKeyValues.ListingPageDefaultSort || prvSort != null))
            {
                return SearchEventType.Sort;
            }

            // Check if it's paging                
            var curOffset = uri.GetQueryParameter(ListingsQueryStringParams.Offset);
            var prvOffset = uriReferrer.GetQueryParameter(ListingsQueryStringParams.Offset);
            if ((curOffset != null && curOffset != prvOffset) && (curOffset != "0" || prvOffset != null))
            {
                return SearchEventType.Pagination;
            }

            // otherwise refinement                
            return SearchEventType.Refinement;
        }
    }
}