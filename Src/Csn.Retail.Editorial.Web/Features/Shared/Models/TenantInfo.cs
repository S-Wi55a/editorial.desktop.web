using Csn.MultiTenant;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public class TenantInfo : ITenant
    {
        public string Name { get; set; }
        public string SiteNavPath { get; set; }
        public string GoogleAnalyticsApp { get; set; }
        public string HotjarTracking { get; set; }
        public bool AllowSeoIndexOfDetails { get; set; }
        public bool AllowSeoIndexOfListing { get; set; }
        public string MobiSiteUrl { get; set; }

        #region Polar Native Ads
        public bool DisplayPolarAds { get; set; }
        public string PolarSitePropertyId { get; set; }
        public string PolarSiteName { get; set; }
        #endregion

        #region Mediamotive
        public string MediaMotiveAccountId { get; set; }
        public string MediaMotiveUrl { get; set; }
        public string KruxId { get; set; }
        #endregion

        #region Content

        public string SiteLogoText { get; set; }
        #endregion

        #region Disqus

        public string DisqusSource { get; set; }
        #endregion
    }
}