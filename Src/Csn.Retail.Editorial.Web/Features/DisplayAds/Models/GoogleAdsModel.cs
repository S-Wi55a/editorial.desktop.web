using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds
{
    public class GoogleAdsModel : IDisplayAdsModel
    {
        public DisplayAdsSource DisplayAdsSource => DisplayAdsSource.GoogleAds;
    }
}