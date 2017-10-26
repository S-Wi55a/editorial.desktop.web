using System.Web;

namespace Csn.Retail.Editorial.Web.Features.Shared.Formatters
{
    public static class UrlParamsFormatter
    {
        public static string GetParams(string q = "", long offset = 0, string sortOrder = "", string keyword = "")
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(q)) queryParams["q"] = q;
            if (!string.IsNullOrEmpty(sortOrder)) queryParams["sortOrder"] = sortOrder;
            if (!string.IsNullOrEmpty(keyword)) queryParams["keyword"] = keyword;
            if (offset != 0) queryParams["offset"] = offset.ToString();

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? string.Empty : "?" + query;
        }
    }
}