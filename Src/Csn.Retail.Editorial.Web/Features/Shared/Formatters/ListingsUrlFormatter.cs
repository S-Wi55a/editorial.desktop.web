﻿using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.Retail.Editorial.Web.Features.Listings.Constants;

namespace Csn.Retail.Editorial.Web.Features.Shared.Formatters
{
    public static class ListingsUrlFormatter
    {
        private static string ListingsBasePath = "/editorial/beta-results";

        public static string GetPathAndQueryString(string q = null, long offset = 0, string sortOrder = null, string keyword = null)
        {
            var queryParams = GetQueryStringParameters(q, offset, sortOrder, keyword);
            var queryString = string.IsNullOrEmpty(queryParams) ? string.Empty : "?" + queryParams;

            return $"{ListingsBasePath}/{queryString}";
        }

        public static string GetSeoUrl(string seofragment, long offset = 0, string sortOrder = null)
        {
            var query = GetQueryStringParameters("", offset, sortOrder, "");
            var pathAndQuery = string.IsNullOrEmpty(query) ? seofragment : $"{seofragment}?{query}";

            return $"{ListingsBasePath}{pathAndQuery}";
        }

        private static string GetQueryStringParameters(string q, long offset, string sortOrder, string keyword)
        {
            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(q)) queryParams[ListingsQueryStringParams.Query] = q;
            if (!string.IsNullOrEmpty(sortOrder)) queryParams[ListingsQueryStringParams.Sort] = sortOrder;
            if (!string.IsNullOrEmpty(keyword)) queryParams[ListingsQueryStringParams.Keywords] = keyword;
            if (offset != 0) queryParams[ListingsQueryStringParams.Offset] = offset.ToString();

            return queryParams.ToQueryString();
        }
    }
}