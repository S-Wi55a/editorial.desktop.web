using System;

namespace Csn.Retail.Editorial.Web.Features.Redirects
{
    public static class RedirectRuleExtensions
    {
        public static string GetRedirectUrl(this RedirectRule input, Uri url)
        {
            if (input.IncludeQueryStringInRedirect && !string.IsNullOrEmpty(url.Query))
            {

            }

            return input.RedirectTo;
        }
    }
}