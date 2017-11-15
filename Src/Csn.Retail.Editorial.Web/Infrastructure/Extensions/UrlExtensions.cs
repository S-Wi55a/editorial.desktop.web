using System;
using System.Collections.Generic;
using System.Linq;
using Bolt.Common.Extensions;

namespace Csn.Retail.Editorial.Web.Infrastructure.Extensions
{
    public static class UrlExtensions
    {
        public static string ToQueryString(this Dictionary<string, string> queryParams)
        {
            return queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}").Join("&");
        }
    }
}