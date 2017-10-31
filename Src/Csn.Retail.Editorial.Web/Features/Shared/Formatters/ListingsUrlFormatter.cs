using System.Web;

namespace Csn.Retail.Editorial.Web.Features.Shared.Formatters
{
    public static class ListingsUrlFormatter
    {
        public static string GetQueryString(string q, long offset, string sortOrder, string keyword)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(q)) queryParams["q"] = q;
            if (!string.IsNullOrEmpty(sortOrder)) queryParams["sortOrder"] = sortOrder;
            if (!string.IsNullOrEmpty(keyword)) queryParams["keyword"] = keyword;
            if (offset != 0) queryParams["offset"] = offset.ToString();

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? string.Empty : "?" + query;
        }

        public static string GetQueryString(string seofragment, long offset, string sortOrder)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(sortOrder)) queryParams["sortOrder"] = sortOrder;
            if (offset != 0) queryParams["offset"] = offset.ToString();

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? seofragment : $"{seofragment}?{query}";
        }
    }
}