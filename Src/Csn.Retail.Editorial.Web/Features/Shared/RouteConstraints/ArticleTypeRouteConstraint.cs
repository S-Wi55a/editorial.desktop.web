using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Csn.Retail.Editorial.Web.Features.Listings.ModelBinders;

namespace Csn.Retail.Editorial.Web.Features.Shared.RouteConstraints
{
    public class ArticleTypeRouteConstraint : IRouteConstraint
    {
        private static IArticleTypeListingLookup _articleTypeLookup;

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (_articleTypeLookup == null)
            {
                _articleTypeLookup = DependencyResolver.Current.GetService<IArticleTypeListingLookup>();
            }
            
            if (values.TryGetValue(parameterName, out var parameterValue) && parameterValue != null)
            {
                var articleType = parameterValue.ToString().Trim('/');
                var type = _articleTypeLookup.GetArticleTypeFromSlug(articleType);
                return type.HasValue;
            }

            return false;
        }
    }
}