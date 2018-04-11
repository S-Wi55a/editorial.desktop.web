using System.Collections.Generic;
using System.Linq;

namespace Csn.Retail.Editorial.Web.Features.Landing.Configurations.Providers
{
    public class MakeConfigProvider
    {
        private static Dictionary<string, List<string>> _configuredMakes;

        public static IList<string> GetConfiguredMakes(string tenantName) => _configuredMakes != null? _configuredMakes[tenantName] : new List<string>();

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