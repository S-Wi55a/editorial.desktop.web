using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive
{
    public class MediaMotiveAdSetting
    {
        public int TileId { get; set; }
        public DisplayAdsTypes Description { get; set; }
        public bool DataKruxRequired { get; set; }
        public AdSize AdSize { get; set; }
        public List<string> NotSupportedArticleTypes { get; set; }
    }
}