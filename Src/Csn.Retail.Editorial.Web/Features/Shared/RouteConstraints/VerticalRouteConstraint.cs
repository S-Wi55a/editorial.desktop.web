﻿using System;
using System.Web;
using System.Web.Routing;
using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.Shared.RouteConstraints
{
    public class VerticalRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.TryGetValue(parameterName, out var parameterValue)) return false;
            var vertical = parameterValue.ToString();
            return Enum.TryParse<Vertical>(vertical, true, out var _);
        }
    }
}