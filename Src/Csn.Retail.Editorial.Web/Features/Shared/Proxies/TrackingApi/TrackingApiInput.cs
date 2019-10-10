namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.TrackingApi
{
    public class TrackingApiInput
    {
        public string ApplicationName { get; set; }
        public bool IncludeNielsen { get; set; }
        public bool IncludeGoogleSem { get; set; }
        public string ClientGeoCountryCode { get; set; }
    }
}