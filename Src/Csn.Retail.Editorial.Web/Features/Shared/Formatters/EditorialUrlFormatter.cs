using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Constants;

namespace Csn.Retail.Editorial.Web.Features.Shared.Formatters
{
    public static class EditorialUrlFormatter
    {
        private static string ListingsBasePath = "/editorial/results";

        public static string GetPathAndQueryString(string q = null, long offset = 0, string sortOrder = null, string keyword = null)
        {
            var queryParams = GetQueryStringParameters(q, offset, sortOrder, keyword);
            var queryString = string.IsNullOrEmpty(queryParams) ? string.Empty : "?" + queryParams;

            return $"{ListingsBasePath}/{queryString}";
        }

        public static string GetQueryString(string action, string sort)
        {
            var queryParams = GetQueryStringParameters(action, 0, sort, string.Empty);
            return string.IsNullOrEmpty(queryParams) ? string.Empty : "?" + queryParams;
        }

        public static string GetSeoUrl(string seofragment, long offset = 0, string sortOrder = null)
        {
            var query = GetQueryStringParameters("", offset, sortOrder, "");
            var pathAndQuery = string.IsNullOrEmpty(query) ? seofragment : $"{seofragment}?{query}";

            return $"{ListingsBasePath}{pathAndQuery}";
        }

        public static string GetQueryParam(string q, long offset, string sortOrder)
        {
            return GetQueryStringParameters(q, offset, sortOrder, string.Empty);            
        }

        private static string GetQueryStringParameters(string q, long offset, string sortOrder, string keyword)
        {
            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(q)) queryParams[EditorialQueryStringParams.Query] = q;
            if (!string.IsNullOrEmpty(sortOrder)) queryParams[EditorialQueryStringParams.Sort] = sortOrder;
            if (!string.IsNullOrEmpty(keyword)) queryParams[EditorialQueryStringParams.Keywords] = keyword;
            if (offset != 0) queryParams[EditorialQueryStringParams.Offset] = offset.ToString();

            return queryParams.ToQueryString();
        }
    }
}