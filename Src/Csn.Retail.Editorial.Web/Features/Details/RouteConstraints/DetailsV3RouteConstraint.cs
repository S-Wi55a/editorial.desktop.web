using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Csn.Retail.Editorial.Web.Features.Details.RouteConstraints
{
    public class DetailsV3RouteConstraint : IRouteConstraint
    {
        private static IDetailsV3RouteValidator _routeValidator;

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (_routeValidator == null)
            {
                _routeValidator = DependencyResolver.Current.GetService<IDetailsV3RouteValidator>();
            }

            if (!values.TryGetValue(parameterName, out var parameterValue)) return false;

            if (parameterValue == null || string.IsNullOrEmpty(parameterValue.ToString())) return false;

            return _routeValidator.IsValid(parameterValue.ToString());
        }
    }
}