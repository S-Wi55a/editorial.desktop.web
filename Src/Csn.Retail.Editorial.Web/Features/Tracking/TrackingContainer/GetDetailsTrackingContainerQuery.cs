using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Tracking.TrackingContainer
{
    public class GetDetailsTrackingContainerQuery : IQuery
    {
        public string Id { get; set; }
        public string PageType { get; set; }
    }
}