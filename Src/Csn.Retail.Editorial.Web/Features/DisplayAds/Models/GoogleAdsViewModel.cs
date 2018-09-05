using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.Models
{
    public class GoogleAdsViewModel : IDisplayAdsModel
    {
        public string Description { get; set; }
        public string Dimensions { get; set; }
        public string AdNetworkCode;
        public string AdUnitId;
        public string AdSlotId;
        public DisplayAdsSource DisplayAdsSource { get; set; }
    }
}