using System.Web;
using System.Web.Routing;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    public class ArticleTypeRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values.TryGetValue(parameterName, out var parameterValue))
            {
                var articleType = parameterValue.ToString();
                var type = EditorialArticleTypes.GetEditorialArticleType(articleType);
                return type.HasValue;
            }
            return false;
        }
    }
}