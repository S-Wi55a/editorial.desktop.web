namespace Csn.Retail.Editorial.Web.Features.Shared.Formatters
{
    public static class UrlParamsFormatter
    {
        public static string GetSortParam(string sortOrder)
        {
            return string.IsNullOrEmpty(sortOrder) ? string.Empty : $"sortOrder={sortOrder}";
        }

        public static string GetKeywordParam(string keyword)
        {
            return string.IsNullOrEmpty(keyword) ? string.Empty : $"keyword={keyword}";
        }

        public static string GetOffsetParam(long offset)
        {
            return offset <= 0 ? string.Empty : $"offset={offset}";
        }

        public static string GetQueryParam(string query)
        {
            return string.IsNullOrEmpty(query) ? string.Empty : $"q={query}";
        }
    }
}