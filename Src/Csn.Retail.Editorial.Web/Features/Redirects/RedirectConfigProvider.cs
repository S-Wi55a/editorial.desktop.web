using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Csn.Logging;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.MultiTenant;
using Ingress.Serializers;

namespace Csn.Retail.Editorial.Web.Features.Redirects
{
    public interface IRedirectConfigProvider
    {
        List<RedirectRule> Get(string tenantName);
    }

    [AutoBindAsSingleton]
    public class RedirectConfigProvider : IRedirectConfigProvider
    {
        private readonly ISerializer _serializer;
        private readonly ILogger _logger;
        private readonly ITenantListProvider _tenantListProvider;

        private static readonly Dictionary<string, List<RedirectRule>> _redirects = new Dictionary<string, List<RedirectRule>>();

        public RedirectConfigProvider(ISerializer serializer, 
                                ILogger logger, 
                                ITenantListProvider tenantListProvider)
        {
            _serializer = serializer;
            _logger = logger;
            _tenantListProvider = tenantListProvider;

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

            return commonRedirectRules;
        }

        private List<RedirectRule> GetRedirectRulesForTenant(RedirectConfigDto config, List<RedirectRule> commonRedirectRules, string tenantName)
        {
            var redirects = new List<RedirectRule>();

            // look for tenant specific redirects
            if (config.Redirects.TryGetValue(tenantName.ToLower(), out var tenantRedirectRules))
            {
                redirects.AddRange(tenantRedirectRules);
            }

            redirects.AddRange(commonRedirectRules);
            return redirects;
        }

        public List<RedirectRule> Get(string tenantName)
        {
            return _redirects.TryGetValue(tenantName, out var result) ? result : null;
        }

        private class RedirectConfigDto
        {
            public Dictionary<string, List<RedirectRule>> Redirects { get; set; }
        }
    }
}