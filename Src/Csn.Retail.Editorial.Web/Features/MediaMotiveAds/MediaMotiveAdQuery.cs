using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds
{
    public class MediaMotiveAdQuery : IQuery
    {
        public int TileId { get; set; }
        public AdSize AdSize { get; set; }
    }
}