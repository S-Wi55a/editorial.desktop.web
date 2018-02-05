using System.Collections.Generic;
using System.Globalization;
using Csn.MultiTenant;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public class TenantInfo : ITenant
    {
        public string Name { get; set; }
        public string SiteNavPath { get; set; }
        public string GoogleAnalyticsApp { get; set; }
        public string HotjarTracking { get; set; }
        public string RyvusNavName { get; set; }
        public CultureInfo Culture { get; set; }
        public string SiteDomain { get; set; }
        public string UrlProtocol { get; set; }
        public string ListingsAlternateUrl { get; set; }

        #region Seo
        public bool AllowSeoIndexOfDetails { get; set; }
        public bool AllowSeoIndexOfListing { get; set; }
        public bool SupportsSeoFriendlyListings { get; set; }
        public string DefaultPageTitle { get; set; }
        public bool AllowSeoIndexOfLanding { get; set; }
        #endregion

        #region Polar Native Ads
        public bool DisplayPolarAds { get; set; }
        public string PolarSitePropertyId { get; set; }
        public string PolarSiteName { get; set; }
        #endregion

        #region Mediamotive
        public string MediaMotiveAccountId { get; set; }
        public string MediaMotiveUrl { get; set; }
        public string KruxId { get; set; }
        public List<string> AdUnits { get; set; }
        public string NativeAdPlacement { get; set; }

        #endregion

        #region Content

        public string SiteLogoText { get; set; }
        public string Favicon { get; set; }
        #endregion

        #region Disqus

        public string DisqusSource { get; set; }
        
        #endregion

        #region Redbook
        public string TenantName { get; set; }
        public string ServiceProjection { get; set; }
        public string Vertical { get; set; }
        #endregion
    }
}