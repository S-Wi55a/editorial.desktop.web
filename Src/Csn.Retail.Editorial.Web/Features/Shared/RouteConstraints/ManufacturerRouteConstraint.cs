using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations.Providers;

namespace Csn.Retail.Editorial.Web.Features.Shared.RouteConstraints
{
    public class ManufacturerRouteConstraint : IRouteConstraint
    {
        private static ILandingConfigProvider _articleTypeLookup;

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (_articleTypeLookup == null)
            {
                _articleTypeLookup = DependencyResolver.Current.GetService<ILandingConfigProvider>();
            }

            if (values.TryGetValue(parameterName, out var parameterValue))
            {
                var articleType = parameterValue.ToString();
                var type = _articleTypeLookup.LoadConfig(articleType);
                return !string.IsNullOrEmpty(type.Result?.Type);
            }

            return false;
        }
    }
}