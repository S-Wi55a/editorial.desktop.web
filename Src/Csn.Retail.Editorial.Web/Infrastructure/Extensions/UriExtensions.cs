using System;
using System.Web;

namespace Csn.Retail.Editorial.Web.Infrastructure.Extensions
{
    public static class UriExtensions
    {
        public static string GetQueryParameter(this Uri uri, string parameterName)
        {
            var queryParams = HttpUtility.ParseQueryString(uri.Query);

            return queryParams[parameterName];
        }
    }
}