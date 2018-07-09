using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Csn.Retail.Editorial.Web.Features.Listings.ModelBinders;

namespace Csn.Retail.Editorial.Web.Features.Shared.RouteConstraints
{
    public class ArticleTypeRouteConstraint : IRouteConstraint
    {
        private static IArticleTypeLookup _articleTypeLookup;

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (_articleTypeLookup == null)
            {
                _articleTypeLookup = DependencyResolver.Current.GetService<IArticleTypeLookup>();
            }
            
            if (values.TryGetValue(parameterName, out var parameterValue))
            {
                var articleType = parameterValue.ToString().Trim('/');
                var type = _articleTypeLookup.GetArticleTypeFromSlug(articleType);
                return type.HasValue;
            }

            return false;
        }
    }
}