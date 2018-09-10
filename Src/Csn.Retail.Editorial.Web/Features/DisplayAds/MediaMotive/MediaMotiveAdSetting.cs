using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive
{
    public class MediaMotiveAdSetting
    {
        public int TileId { get; set; }
        public DisplayAdPlacements Description { get; set; }
        public bool DataKruxRequired { get; set; }
        public DisplayAdSizes DisplayAdSizes { get; set; }
        public List<string> NotSupportedArticleTypes { get; set; }
    }
}