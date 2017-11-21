using System.Web;
using Csn.Retail.Editorial.Web.Features.Listings.Constants;

namespace Csn.Retail.Editorial.Web.Features.Shared.Formatters
{
    public static class ListingsUrlFormatter
    {
        private static string GetQueryStringParameters(string q, long offset, string sortOrder, string keyword)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(q)) queryParams[ListingsQueryStringParams.Query] = q;
            if (!string.IsNullOrEmpty(sortOrder)) queryParams[ListingsQueryStringParams.Sort] = sortOrder;
            if (!string.IsNullOrEmpty(keyword)) queryParams[ListingsQueryStringParams.Keywords] = keyword;
            if (offset != 0) queryParams[ListingsQueryStringParams.Offset] = offset.ToString();

            return queryParams.ToString();
        }

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
    }
}