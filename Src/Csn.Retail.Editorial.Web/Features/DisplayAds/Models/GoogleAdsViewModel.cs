using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.Models
{
    public class GoogleAdsViewModel : IDisplayAdsModel
    {
        public string SlotId;
        public string Description { get; set; }
        public DisplayAdsSource DisplayAdsSource { get; set; }
    }
}