using System;

namespace Csn.Retail.Editorial.Web.Features.Redirects.RedirectProviders
{
    public interface IRedirectProvider
    {
        RedirectInstruction GetRedirect(RedirectRule redirectRule, Uri uri);
    }
}