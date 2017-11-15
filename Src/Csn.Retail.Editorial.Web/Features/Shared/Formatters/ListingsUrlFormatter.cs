using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;

namespace Csn.Retail.Editorial.Web.Features.Shared.Formatters
{
    public static class ListingsUrlFormatter
    {
        public static string GetQueryString(string q = null, long offset = 0, string sortOrder = null,
            string keyword = null)
        {
            var queryParams = GetQueryStringParameters(q, offset, sortOrder, keyword);
            return string.IsNullOrEmpty(queryParams) ? string.Empty : "?" + queryParams;
        }

        public static string GetSeoUrl(string seofragment, long offset = 0, string sortOrder = null,
            string keyword = null)
        {
            var query = GetQueryStringParameters("", offset, sortOrder, "");
            return string.IsNullOrEmpty(query) ? seofragment : $"{seofragment}?{query}";
        }

        private static string GetQueryStringParameters(string q, long offset, string sortOrder, string keyword)
        {
            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(q)) queryParams["q"] = q;
            if (!string.IsNullOrEmpty(sortOrder)) queryParams["sortOrder"] = sortOrder;
            if (!string.IsNullOrEmpty(keyword)) queryParams["keyword"] = keyword;
            if (offset != 0) queryParams["offset"] = offset.ToString();

            return queryParams.ToQueryString();
        }
    }
}