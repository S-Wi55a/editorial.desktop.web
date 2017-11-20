using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bolt.Common.Extensions;

namespace Csn.Retail.Editorial.Web.Infrastructure.Extensions
{
    public static class UriExtensions
    {
        public static string GetQueryParameter(this Uri uri, string parameterName)
        {
            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            return queryParams[parameterName];
        }

        public static string ToQueryString(this Dictionary<string, string> queryParams)
        {
            return queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}").Join("&");
        }
    }
}