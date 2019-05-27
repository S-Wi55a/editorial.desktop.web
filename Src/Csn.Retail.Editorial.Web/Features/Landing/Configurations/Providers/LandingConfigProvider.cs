using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Ingress.Cache;
using Ingress.Serializers;

namespace Csn.Retail.Editorial.Web.Features.Landing.Configurations.Providers
{
    public interface ILandingConfigProvider
    {
        Task<LandingConfigurationSet> GetConfig(string slug);
    }

    [AutoBind]
    public class LandingConfigProvider : ILandingConfigProvider
    {
        private readonly ISerializer _serializer;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly ICacheStore _cacheStore;
        private readonly string _landingConfigPath = System.Configuration.ConfigurationManager.AppSettings["LandingConfigPath"];
        private readonly string _cacheKey = "editorial:desk:{0}:{1}:landingconfig";
        private readonly string _buildVersion = System.Configuration.ConfigurationManager.AppSettings["BuildVersion"];
        private readonly TimeSpan _localCacheDuration = new TimeSpan(0, 60, 0);
        private readonly TimeSpan _distributedCacheDuration = new TimeSpan(0, 0, 0);

        public LandingConfigProvider(ISerializer serializer, ITenantProvider<TenantInfo> tenantProvider, ICacheStore cacheStore)
        {
            _serializer = serializer;
            _tenantProvider = tenantProvider;
            _cacheStore = cacheStore;
        }

        public async Task<LandingConfigurationSet> GetConfig(string slug)
        {
            if (!_tenantProvider.Current().HasLandingPageConfiguration) return null;

            var cacheKey = _cacheKey.FormatWith(_buildVersion, _tenantProvider.Current().Name);

            var cachedConfig = await _cacheStore.GetAsync<LandingConfig>(cacheKey);
            if (cachedConfig.HasValue)
            {
                return cachedConfig.Value.Configs.FirstOrDefault(a => a.Slug == slug);
            }

            var fullpath = Path.Combine($"{HttpRuntime.AppDomainAppPath}{_landingConfigPath}{_tenantProvider.Current().Name}.landingconfig.json");

            if (!File.Exists(fullpath))
            {
                return new LandingConfigurationSet();
            }

            var content = File.ReadAllText(fullpath);
            var landingConfig = _serializer.Deserialize<LandingConfig>(content);

            await _cacheStore.SetAsync(cacheKey, landingConfig, new CacheExpiredIn(_localCacheDuration, _distributedCacheDuration));

            return landingConfig.Configs.FirstOrDefault(a => a.Slug == slug);
        }
    }
}