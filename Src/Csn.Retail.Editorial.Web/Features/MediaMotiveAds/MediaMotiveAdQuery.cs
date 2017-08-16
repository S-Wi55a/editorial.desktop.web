using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds
{
    public class MediaMotiveAdQuery : IQuery
    {
        public string TileId { get; set; }
        public MediaMotiveData MediaMotiveData { get; set; }
    }
}