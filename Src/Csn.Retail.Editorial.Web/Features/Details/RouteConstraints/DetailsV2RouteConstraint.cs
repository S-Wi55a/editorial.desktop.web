using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Csn.Retail.Editorial.Web.Features.Details.RouteConstraints
{
    public class DetailsV2RouteConstraint : IRouteConstraint
    {
        private static IDetailsV2RouteValidator _routeValidator;

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (_routeValidator == null)
            {
                _routeValidator = DependencyResolver.Current.GetService<IDetailsV2RouteValidator>();
            }

            if (!values.TryGetValue(parameterName, out var parameterValue)) return false;

            if (parameterValue == null || string.IsNullOrEmpty(parameterValue.ToString())) return false;

            return _routeValidator.IsValid(parameterValue.ToString());
        }
    }
}