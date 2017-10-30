using System.Collections.Generic;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;

namespace Csn.Retail.Editorial.Web.Infrastructure.Utils
{
    public static class ListingsUrlBuilder
    {
        public static string Build(string query, string keyword, long offset, string sortOrder)
        {
            return "?{ConcatQueryParameters(keyword, sortOrder, offset, query)}";
        }

        public static string Build(string seoFragment, long offset, string sortOrder)
        {
            var queryparams = ConcatQueryParameters("", sortOrder, offset);
            var query = queryparams.IsNullOrEmpty() ? string.Empty : $"?{queryparams}";

            return  $"{seoFragment}{query}";
        }

        private static string ConcatQueryParameters(string keyword, string sortOrder, long offset,
            string nonSeoQuery = null)
        {
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(nonSeoQuery))
            {
                queryParams.Add(UrlParamsFormatter.GetQueryParam(nonSeoQuery));
            }
            if (offset > 0)
            {
                queryParams.Add(UrlParamsFormatter.GetOffsetParam(offset));
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                queryParams.Add(UrlParamsFormatter.GetKeywordParam(keyword));
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                queryParams.Add(UrlParamsFormatter.GetSortParam(sortOrder));
            }
            return $"{queryParams.Join("&")}";
        }
    }
}