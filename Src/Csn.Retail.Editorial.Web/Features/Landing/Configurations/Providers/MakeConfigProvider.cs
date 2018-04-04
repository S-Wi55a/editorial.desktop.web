using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.Landing.Configurations.Providers
{
    public class MakeConfigProvider
    {
        private static Dictionary<string, List<string>> _configuredMakes;

        public static IList<string> GetConfiguredMakes(string tenantName) => _configuredMakes[tenantName];

        public static void SetConfiguredMakes(string tenantName, List<string> configuredMakes)
        {
            var lockObj = new object();
            lock (lockObj)
            {
                if(_configuredMakes == null)
                    _configuredMakes = new Dictionary<string, List<string>>();

                _configuredMakes[tenantName] = configuredMakes;
            }
        }
    }
}