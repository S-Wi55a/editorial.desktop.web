using System;
using System.Text.RegularExpressions;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;

namespace Csn.Retail.Editorial.Web.Features.Redirects.RedirectProviders
{
    [AutoBindSelf]
    public class RegexRedirectProvider : IRedirectProvider
    {
        public string GetRedirectUrl(RedirectRule redirectRule, Uri uri)
        {
            var regex = new Regex(redirectRule.MatchRule, RegexOptions.IgnoreCase);

            var absolutePathUnescaped = uri.AbsolutePathUnescaped();

            var matches = regex.Matches(absolutePathUnescaped);

            if (matches.Count != 1) return null;

            return Regex.Replace(absolutePathUnescaped, redirectRule.MatchRule, redirectRule.RedirectInstruction);
        }
    }
}