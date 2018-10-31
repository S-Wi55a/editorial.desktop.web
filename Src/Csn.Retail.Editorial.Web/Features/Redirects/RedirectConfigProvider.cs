using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Csn.Logging;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.MultiTenant;
using Ingress.Serializers;

namespace Csn.Retail.Editorial.Web.Features.Redirects
{
    public interface IRedirectConfigProvider
    {
        Dictionary<string, Dictionary<string, RedirectRule>> Get();
        Dictionary<string, RedirectRule> Get(string tenantName);
    }

    [AutoBindAsSingleton]
    public class RedirectConfigProvider : IRedirectConfigProvider
    {
        private readonly ISerializer _serializer;
        private readonly ILogger _logger;
        private readonly ITenantListProvider _tenantListProvider;
        private readonly IEditorialRouteSettings _editorialRouteSettings;

        private static readonly Dictionary<string, Dictionary<string, RedirectRule>> _redirects = new Dictionary<string, Dictionary<string, RedirectRule>>();

        public RedirectConfigProvider(ISerializer serializer, 
                                ILogger logger, 
                                ITenantListProvider tenantListProvider, 
                                IEditorialRouteSettings editorialRouteSettings)
        {
            _serializer = serializer;
            _logger = logger;
            _tenantListProvider = tenantListProvider;
            _editorialRouteSettings = editorialRouteSettings;

            Init();
        }

        private void Init()
        {
            var fullpath = Path.Combine($"{HttpRuntime.AppDomainAppPath}{System.Configuration.ConfigurationManager.AppSettings["RedirectsConfigPath"]}");

            if (!File.Exists(fullpath))
            {
                _logger.Error("Could not load redirect configuration file with path:{0}", fullpath);
                return;
            }

            var content = File.ReadAllText(fullpath);
            var config = _serializer.Deserialize<RedirectConfigDto>(content);

            var commonRedirectRules = GetCommonRedirectRules(config);

            // loop through each tenant
            foreach (var tenantName in _tenantListProvider.GetTenants())
            {
                _redirects.Add(tenantName.ToLower(), GetRedirectRulesForTenant(config, commonRedirectRules, tenantName));
            }
        }

        private List<RedirectRule> GetCommonRedirectRules(RedirectConfigDto config)
        {
            config.Redirects.TryGetValue("common", out var commonRedirectRules);

            if (commonRedirectRules == null) return new List<RedirectRule>();

            return commonRedirectRules.Select(GetRedirectRuleWithBasePath).ToList();
        }

        private Dictionary<string, RedirectRule> GetRedirectRulesForTenant(RedirectConfigDto config, List<RedirectRule> commonRedirectRules, string tenantName)
        {
            var redirects = new Dictionary<string, RedirectRule>();

            foreach (var redirect in commonRedirectRules)
            {
                redirects.Add(redirect.MatchUrl, redirect);
            }

            // look for tenant specific redirects
            if (!config.Redirects.TryGetValue(tenantName.ToLower(), out var tenantRedirectRules)) return redirects;

            foreach (var redirect in tenantRedirectRules.Select(GetRedirectRuleWithBasePath))
            {
                if (redirects.ContainsKey(redirect.MatchUrl))
                {
                    // replace the existing redirect with the tenant specific redirect
                    redirects[redirect.MatchUrl] = redirect;
                    continue;
                }

                redirects.Add(redirect.MatchUrl, redirect);
            }

            return redirects;
        }

        /// <summary>
        /// Includes the configured base path in the match and redirect urls so they do not have to be included in the
        /// redirect config file
        /// </summary>
        private RedirectRule GetRedirectRuleWithBasePath(RedirectRule redirectRule)
        {
            var result = new RedirectRule()
            {
                MatchUrl = redirectRule.MatchUrl,
                RedirectTo = redirectRule.RedirectTo,
                IncludeQueryStringInRedirect = redirectRule.IncludeQueryStringInRedirect
            };

            var trimmedBasePath = _editorialRouteSettings.BasePath.Trim('/');

            if (!redirectRule.MatchUrl.Trim('/').StartsWith(trimmedBasePath))
            {
                result.MatchUrl = $"/{trimmedBasePath}/{redirectRule.MatchUrl.TrimStart('/')}";
            }

            if (!redirectRule.RedirectTo.Trim('/').StartsWith(trimmedBasePath))
            {
                result.RedirectTo = $"/{trimmedBasePath}/{redirectRule.RedirectTo.TrimStart('/')}";
            }

            return result;
        }

        public Dictionary<string, Dictionary<string, RedirectRule>> Get()
        {
            return _redirects;
        }

        public Dictionary<string, RedirectRule> Get(string tenantName)
        {
            return _redirects.TryGetValue(tenantName, out var result) ? result : null;
        }

        private class RedirectConfigDto
        {
            public Dictionary<string, List<RedirectRule>> Redirects { get; set; }
        }
    }
}