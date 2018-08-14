using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations.Providers;
using Csn.Retail.Editorial.Web.Features.Shared.Constants;

namespace Csn.Retail.Editorial.Web.Features.Shared.RouteConstraints
{
    public class ManufacturerRouteConstraint : IRouteConstraint
    {
        private static ILandingConfigProvider _landingConfigProvider;

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (_landingConfigProvider == null)
            {
                _landingConfigProvider = DependencyResolver.Current.GetService<ILandingConfigProvider>();
            }

            if (values.TryGetValue(parameterName, out var parameterValue))
            {
                if (parameterValue == null || string.IsNullOrEmpty(parameterValue.ToString()) || 
                    httpContext.Request.Params[EditorialQueryStringParams.Offset] != null || httpContext.Request.Params[EditorialQueryStringParams.Sort] != null) return false; // 'LandingHome' to Handle /editorial/ route
                var manufacturer = parameterValue.ToString().Trim('/');
                var landingConfigSet = _landingConfigProvider.LoadConfig(manufacturer);
                return !string.IsNullOrEmpty(landingConfigSet.Result?.Type);
            }

            return false;
        }
    }
}