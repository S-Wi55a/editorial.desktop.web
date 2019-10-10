using System;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Wrappers;

namespace Csn.Retail.Editorial.Web.Infrastructure.Akamai
{
    public interface IAkamaiEdgescapeHeaderProvider
    {
        string GetCountryCode();
    }

    [AutoBind]
    public class AkamaiEdgescapeHeaderProvider : IAkamaiEdgescapeHeaderProvider
    {
        private readonly IRequestWrapper _requestWrapper;

        private static readonly char[] SplitChars = new[] { ',' };

        private const string CountryCodeEdgeKey = "country_code";

        public AkamaiEdgescapeHeaderProvider(IRequestWrapper requestWrapper)
        {
            _requestWrapper = requestWrapper;
        }

        public string GetCountryCode()
        {
            return GetIsoCode(CountryCodeEdgeKey);
        }

        private string GetIsoCode(string key)
        {
            if (!string.IsNullOrEmpty(_requestWrapper.GdprRegion)) return _requestWrapper.GdprRegion;

            if (!string.IsNullOrEmpty(_requestWrapper.AkamaiEdgescape))
            {
                var data = _requestWrapper.AkamaiEdgescape.Split(SplitChars, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in data)
                {
                    var keyValue = item.Split('=');
                    if (keyValue.Length == 2 && keyValue[0].IsSame(key))
                    {
                        return keyValue[1].Trim();
                    }
                }
            }

            return string.Empty;
        }
    }
}