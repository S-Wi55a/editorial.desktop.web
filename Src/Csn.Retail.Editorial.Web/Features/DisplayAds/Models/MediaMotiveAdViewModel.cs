using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.Models
{
    public class MediaMotiveAdViewModel : IDisplayAdsModel
    {
        public string TileId { get; set; }
        public string Description { get; set; }
        public string NoScriptImageUrl { get; set; }
        public string NoScriptUrl { get; set; }
        public string ScriptUrl { get; set; }

        public int Height { get; set; }
        public int Width { get; set; }

        public bool DataKruxRequired { get; set; }
        public DisplayAdsSource DisplayAdsSource { get; set; }
    }
}