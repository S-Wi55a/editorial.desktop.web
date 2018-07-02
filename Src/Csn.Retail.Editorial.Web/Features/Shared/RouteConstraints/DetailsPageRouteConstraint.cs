using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Text.RegularExpressions;


namespace Csn.Retail.Editorial.Web.Features.Shared.RouteConstraints
{
    public class DetailsPageRouteConstraint : IRouteConstraint
    {

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.TryGetValue(parameterName, out var parameterValue)) return false;

            if (parameterValue == null || string.IsNullOrEmpty(parameterValue.ToString())) return false;
            var urlSegments = parameterValue.ToString().Trim('/').Split('/');
            return urlSegments.Length >= 2 && SatisfyRouteConstraints(urlSegments.First(), urlSegments.Last());
        }


        private bool SatisfyRouteConstraints(string initialroutePrefix, string articleIdSegment)
        {
            var articleIdSegmentRegex = new Regex(".*-\\d+/?$", RegexOptions.CultureInvariant);

            return DetailsPageUrlprefix.Any(a => a == initialroutePrefix) &&
                   articleIdSegmentRegex.IsMatch(articleIdSegment);
        }


        private static readonly List<string> DetailsPageUrlprefix = new List<string>
        {
            "features",
            "riding-advice",
            "tips",
            "tow-tests",
            "motoracing",
            "engine-reviews",
            "reviews",
            "news",
            "advice",
            "videos",
            "products"
        };
    }
}