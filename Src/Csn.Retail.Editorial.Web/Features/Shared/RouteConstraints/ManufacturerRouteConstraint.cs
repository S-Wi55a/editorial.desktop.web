using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations.Providers;

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
                var articleType = parameterValue.ToString();
                var landingConfigSet = _landingConfigProvider.LoadConfig(articleType);
                return !string.IsNullOrEmpty(landingConfigSet.Result?.Type);
            }

            return false;
        }
    }
}