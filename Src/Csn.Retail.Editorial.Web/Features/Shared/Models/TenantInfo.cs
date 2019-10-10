using System;
using System.Globalization;
using System.Linq;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.DisplayAds;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public class TenantInfo : ITenant
    {
        public string Name { get; set; }
        public string GoogleAnalyticsApp { get; set; }
        public string HotjarTracking { get; set; }
        public string RyvusNavName { get; set; }
        public CultureInfo Culture { get; set; }
        public string SiteDomain { get; set; }
        public string UrlProtocol { get; set; }
        public string ListingsAlternateUrl { get; set; }
        public string TenantName { get; set; }
        public string RyvusServiceProjection { get; set; }
        public bool HasLandingPageConfiguration { get; set; }

        #region Seo
        public bool SupportsSeoFriendlyListings { get; set; }
        public string DefaultPageTitle { get; set; }
        public string SeoSchemaVehicleType { get; set; }
        #endregion

        #region Polar Native Ads
        public bool DisplayPolarAds { get; set; }
        public string PolarSitePropertyId { get; set; }
        public string PolarSiteName { get; set; }
        #endregion

        #region Display Ads (Media Motive & Google Ads)
        public DisplayAdsSource DisplayAdsSource { get; set; }
        public string MediaMotiveAccountId { get; set; }
        public string MediaMotiveUrl { get; set; }
        public string KruxId { get; set; }
        public string NativeAdPlacement { get; set; }
        public string GoogleAdsNetworkCode { get; set; }

        public bool UseMediaMotive => DisplayAdsSource == DisplayAdsSource.MediaMotive;
        public bool UseGoogleAd => DisplayAdsSource == DisplayAdsSource.GoogleAd;
        #endregion

        #region Content

        public string SiteLogoText { get; set; }
        public string Favicon { get; set; }
        #endregion

        #region Disqus

        public string DisqusSource { get; set; }
        
        #endregion

        #region Redbook
        
        public string Vertical { get; set; }

        #endregion

        #region Tracking

        public bool IncludeNielsen { get; set; }

        #endregion

        public bool IsAuTenant()
        {
            return AuTenants.Any(a => a.Equals(Name, StringComparison.CurrentCultureIgnoreCase));
        }

        private static readonly string[] AuTenants =
        {
            "carsales",
            "bikesales",
            "constructionsales",
            "boatsales",
            "trucksales",
            "caravancampingsales",
            "farmmachinerysales",
            "redbook"
        };
    }
}