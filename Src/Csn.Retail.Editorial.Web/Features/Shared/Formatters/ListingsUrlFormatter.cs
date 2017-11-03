﻿using System.Web;

namespace Csn.Retail.Editorial.Web.Features.Shared.Formatters
{
    public static class ListingsUrlFormatter
    {
        public static string GetQueryStringParameters(string q, long offset, string sortOrder, string keyword)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(q)) queryParams["q"] = q;
            if (!string.IsNullOrEmpty(sortOrder)) queryParams["sortOrder"] = sortOrder;
            if (!string.IsNullOrEmpty(keyword)) queryParams["keyword"] = keyword;
            if (offset != 0) queryParams["offset"] = offset.ToString();

            return queryParams.ToString();
        }

        public static string GetQueryString(string q, long offset, string sortOrder, string keyword)
        {
            var queryParams = GetQueryStringParameters(q, offset,sortOrder,keyword);
            return string.IsNullOrEmpty(queryParams) ? string.Empty : "?" + queryParams;
        }

        public static string GetSeoUrl(string seofragment, long offset, string sortOrder)
        {
            var query = GetQueryStringParameters("", offset, sortOrder, "");
            return string.IsNullOrEmpty(query) ? seofragment : $"{seofragment}?{query}";
        }
    }
}