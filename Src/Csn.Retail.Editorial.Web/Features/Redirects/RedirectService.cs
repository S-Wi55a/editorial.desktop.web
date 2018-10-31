using System.Web.Mvc;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Redirects;

namespace Csn.Retail.Editorial.Web.Features.Redirects
{
    public interface IRedirectService
    {
        RedirectResult GetRedirect();
        bool CurrentRequestRequiresRedirect();
    }

    [AutoBindAsPerRequest]
    public class RedirectService : IRedirectService
    {
        private readonly IRedirectConfigProvider _redirectConfigProvider;
        private readonly IRequestContextWrapper _requestContextWrapper;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public RedirectService(IRedirectConfigProvider redirectConfigProvider, 
                            IRequestContextWrapper requestContextWrapper, 
                            ITenantProvider<TenantInfo> tenantProvider)
        {
            _redirectConfigProvider = redirectConfigProvider;
            _requestContextWrapper = requestContextWrapper;
            _tenantProvider = tenantProvider;
        }

        public RedirectResult GetRedirect()
        {
            var redirectRule = GetRedirectRuleForRequest();

            if (redirectRule == null) return null;

            return new RedirectResult(redirectRule.GetRedirectUrl(_requestContextWrapper.Url), true);
        }

        public bool CurrentRequestRequiresRedirect()
        {
            var redirectRule = GetRedirectRuleForRequest();

            if (redirectRule == null) return false;

            return true;
        }

        private RedirectRule GetRedirectRuleForRequest()
        {
            if (!_redirectConfigProvider.Get(_tenantProvider.Current().Name.ToLower()).TryGetValue(_requestContextWrapper.Url.AbsolutePath, out var redirectRule))
            {
                return null;
            }

            return redirectRule;
        }
    }
}