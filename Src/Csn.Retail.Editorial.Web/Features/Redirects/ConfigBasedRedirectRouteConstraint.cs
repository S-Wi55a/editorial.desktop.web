using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Csn.Retail.Editorial.Web.Features.Redirects
{
    public class ConfigBasedRedirectRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var redirectService = DependencyResolver.Current.GetService<IRedirectService>();

            return redirectService.CurrentRequestRequiresRedirect();
        }
    }
}