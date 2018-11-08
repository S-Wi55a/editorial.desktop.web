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

        /// <summary>
        /// When there are accented characters or unicode characters in the uri, Uri.AbsolutePath returns these characters in
        /// uri escaped format. This method simply returns the unescaped form
        /// </summary>
        public static string AbsolutePathUnescaped(this Uri uri)
        {
            return Uri.UnescapeDataString(uri.AbsolutePath);
        }
    }
}