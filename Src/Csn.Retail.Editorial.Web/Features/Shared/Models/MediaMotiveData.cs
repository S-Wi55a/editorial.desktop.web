using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public class MediaMotiveData
    {
        public string KruxId { get; set; }
        public string MediaMotiveDomain { get; set; }
        public List<MMItem> MediaMotiveItem { get; set; }

        public class MMItem
        {
            public string TileId { get; set; }
            public string TileDescription { get; set; }
            public string DataKruxRequired { get; set; }
            public string TileUri { get; set; }
            public TileUrl TileUrls { get; set; }

            public class TileUrl
            {
                public string JServerUrl { get; set; }
                public string AdClickUrl { get; set; }
                public string IServerUrl { get; set; }
            }
        }
    }
}