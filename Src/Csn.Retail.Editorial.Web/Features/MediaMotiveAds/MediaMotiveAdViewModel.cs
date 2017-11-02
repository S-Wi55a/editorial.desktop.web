namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds
{
    public class MediaMotiveAdViewModel
    {
        public string TileId { get; set; }
        public string Description { get; set; }
        public string NoScriptImageUrl { get; set; }
        public string NoScriptUrl { get; set; }
        public string ScriptUrl { get; set; }

        public int Height { get; set; }
        public int Width { get; set; }

        public bool DataKruxRequired { get; set; }
    }
}