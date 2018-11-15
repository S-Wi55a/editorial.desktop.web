using System;

namespace Csn.Retail.Editorial.Web.Features.Redirects.RedirectProviders
{
    public interface IRedirectProvider
    {
        string GetRedirectUrl(RedirectRule redirectRule, Uri uri);
    }
}