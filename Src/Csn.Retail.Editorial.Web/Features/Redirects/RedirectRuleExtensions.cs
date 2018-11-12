using System;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Redirects.RedirectProviders;

namespace Csn.Retail.Editorial.Web.Features.Redirects
{
    public static class RedirectRuleExtensions
    {
        public static RedirectInstruction GetRedirectInstruction(this RedirectRule input, Uri uri)
        {
            IRedirectProvider redirectProvider = null;

            switch (input.RuleType)
            {
                case RedirectRuleType.Regex:
                    redirectProvider = DependencyResolver.Current.GetService<RegexRedirectProvider>();
                    break;
                case RedirectRuleType.DetailsLegacyUrlPaths:
                    redirectProvider = DependencyResolver.Current.GetService<DetailsLegacyUrlPathRedirectProvider>();
                    break;
            }

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
    }
}