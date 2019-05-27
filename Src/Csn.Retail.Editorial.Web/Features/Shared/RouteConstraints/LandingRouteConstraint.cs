using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations.Providers;
using Csn.Retail.Editorial.Web.Features.Shared.Constants;

namespace Csn.Retail.Editorial.Web.Features.Shared.RouteConstraints
{
    public class LandingRouteConstraint : IRouteConstraint
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
                if (httpContext.Request.Params[EditorialQueryStringParams.Offset] != null 
                    || httpContext.Request.Params[EditorialQueryStringParams.Sort] != null) return false; // sorting or pagination results

                var slug = parameterValue == null || string.IsNullOrEmpty(parameterValue.ToString()) ? "" : parameterValue.ToString().Trim('/');

                var landingConfigSet = _landingConfigProvider.GetConfig(slug);
                return landingConfigSet.Result != null;
            }

            return false;
        }
    }
}