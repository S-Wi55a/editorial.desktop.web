using System;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds;
using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.Models
{
    public class GoogleAdsViewModel : IDisplayAdsModel
    {
        public string Description { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string AdNetworkCode;
        public string AdUnitId;
        public string AdSlotId;
        public DisplayAdsSource DisplayAdsSource { get; set; }
    }
}