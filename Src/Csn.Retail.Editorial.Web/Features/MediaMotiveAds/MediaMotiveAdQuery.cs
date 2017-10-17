using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds
{
    public class MediaMotiveAdQuery : IQuery
    {
        public string TileId { get; set; }
    }
}