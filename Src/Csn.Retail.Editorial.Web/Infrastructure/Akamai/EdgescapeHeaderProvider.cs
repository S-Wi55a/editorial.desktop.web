using System;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Ingress.Web.Common.Abstracts;

namespace Csn.Retail.Editorial.Web.Infrastructure.Akamai
{
    public interface IAkamaiEdgescapeHeaderProvider
    {
        string GetCountryCode();
    }

    [AutoBind]
    public class AkamaiEdgescapeHeaderProvider : IAkamaiEdgescapeHeaderProvider
    {
        private readonly IHttpRequestWrapper _requestWrapper;

        private static readonly char[] SplitChars = new[] { ',' };

        private const string AkamaiEdgescapeHeader = "X-Akamai-Edgescape";
        private const string CountryCodeEdgeKey = "country_code";

        public AkamaiEdgescapeHeaderProvider(IHttpRequestWrapper requestWrapper)
        {
            _requestWrapper = requestWrapper;
        }

        public string GetCountryCode()
        {
            return GetIsoCode(CountryCodeEdgeKey);
        }

        private string GetIsoCode(string key)
        {
            if (_requestWrapper.QueryParams.TryGetValue("gdprRegion", out var region)) return region;

            if (_requestWrapper.Headers == null) return string.Empty;

            if (_requestWrapper.Headers.TryGetValue(AkamaiEdgescapeHeader, out var edgeLocation))
            {
                var data = edgeLocation.Split(SplitChars, StringSplitOptions.RemoveEmptyEntries);

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