using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.Shared.RouteConstraints
{
    public class VerticalRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.TryGetValue(parameterName, out var parameterValue) || parameterValue == null ||
                DependencyResolver.Current.GetService<ITenantProvider<TenantInfo>>().Current().TenantName != "redbook")
                return false;
            var vertical = parameterValue.ToString().Trim('/');

            return vertical.Length > 1 && Enum.TryParse(vertical, true, out Vertical _);
        }
    }
}