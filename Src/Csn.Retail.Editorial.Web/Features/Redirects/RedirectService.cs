using System.Linq;
using System.Web.Mvc;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Redirects.RedirectProviders;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Redirects;

namespace Csn.Retail.Editorial.Web.Features.Redirects
{
    public interface IRedirectService
    {
        RedirectInstruction GetRedirect();
        bool CurrentRequestRequiresRedirect();
    }

    [AutoBindAsPerRequest]
    public class RedirectService : IRedirectService
    {
        private readonly IRedirectConfigProvider _redirectConfigProvider;
        private readonly IRequestContextWrapper _requestContextWrapper;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        private RedirectInstruction _redirectInstruction;

        public RedirectService(IRedirectConfigProvider redirectConfigProvider,
                            IRequestContextWrapper requestContextWrapper,
                            ITenantProvider<TenantInfo> tenantProvider)
        {
            _redirectConfigProvider = redirectConfigProvider;
            _requestContextWrapper = requestContextWrapper;
            _tenantProvider = tenantProvider;
        }

        public RedirectInstruction GetRedirect()
        {
            if (_redirectInstruction != null) return _redirectInstruction;

            _redirectInstruction = GetRedirectInstructionForRequest() ?? new RedirectInstruction();

            return _redirectInstruction;
        }

        public bool CurrentRequestRequiresRedirect()
        {
            var redirectRules = _redirectConfigProvider.Get(_tenantProvider.Current().Name.ToLower());
            IRedirectProvider redirectRuleProvider;
            
            return redirectRules != null && redirectRules.Any() && redirectRules.Any(rule =>
            {
                redirectRuleProvider = rule.GetRegiRedirectProvider();
                return redirectRuleProvider.Matches(rule, _requestContextWrapper.Url);
            });
        }

        private RedirectInstruction GetRedirectInstructionForRequest()
        {
            var redirectRules = _redirectConfigProvider.Get(_tenantProvider.Current().Name.ToLower());

            if (redirectRules == null || !redirectRules.Any()) return null;

            // loop through and try to find a match
            foreach (var redirectRule in redirectRules)
            {
                var redirectInstruction = redirectRule.GetRedirectInstruction(_requestContextWrapper.Url);

                if (redirectInstruction != null)
                {
                    return redirectInstruction;
                }
            }

            return null;
        }
    }

    public class RedirectInstruction
    {
        public RedirectRuleType RuleType { get; set; }
        public string Name { get; set; }
        public RedirectResult RedirectResult { get; set; }
    }
}