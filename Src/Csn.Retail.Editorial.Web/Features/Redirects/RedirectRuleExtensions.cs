using System;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Redirects.RedirectProviders;

namespace Csn.Retail.Editorial.Web.Features.Redirects
{
    public static class RedirectRuleExtensions
    {
        public static RedirectInstruction GetRedirectInstruction(this RedirectRule input, Uri uri)
        {
            var redirectProvider = input.GetRedirectProvider();
            var redirectUrl = redirectProvider?.GetRedirectUrl(input, uri);

            if (string.IsNullOrEmpty(redirectUrl)) return null;

            if (input.IncludeQueryStringInRedirect && !string.IsNullOrEmpty(uri.Query))
            {
                redirectUrl = $"{redirectUrl}{uri.Query}";
            }

            return new RedirectInstruction()
            {
                Name = input.Name,
                RuleType = input.RuleType,
                RedirectResult = new RedirectResult(redirectUrl, true)
            };
        }

        public static IRedirectProvider GetRedirectProvider(this RedirectRule redirectRule)
        {
            switch (redirectRule.RuleType)
            {
                case RedirectRuleType.Regex:
                    return DependencyResolver.Current.GetService<RegexRedirectProvider>();
                case RedirectRuleType.DetailsLegacyUrlPaths:
                    return DependencyResolver.Current.GetService<DetailsLegacyUrlPathRedirectProvider>();
                default: return null;
            }
        }
    }
}