namespace Csn.Retail.Editorial.Web.Features.Shared.Formatters
{
    public static class UrlParamsFormatter
    {
        public static string GetSortParam(string sortOrder)
        {
            return string.IsNullOrEmpty(sortOrder) ? string.Empty : $"&sortOrder={sortOrder}";
        }

        public static string GetKeywordParam(string keyword)
        {
            return string.IsNullOrEmpty(keyword) ? string.Empty : $"&keyword={keyword}";
        }

        public static string GetQueryParam(string q)
        {
            return string.IsNullOrEmpty(q) ? string.Empty : $"?q={q}";
        }
    }
}