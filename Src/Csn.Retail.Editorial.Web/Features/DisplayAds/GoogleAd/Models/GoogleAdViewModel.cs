namespace Csn.Retail.Editorial.Web.Features.DisplayAds.GoogleAd.Models
{
    public class GoogleAdViewModel
    {
        public string Description { get; set; }
        public string Dimensions { get; set; }
        public string AdNetworkCode;
        public string AdUnitId;
        public string AdSlotId;
        public DisplayAdsSource DisplayAdsSource { get; set; }
    }
}