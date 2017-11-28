using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Tracking.TrackingContainer
{
    public class GetListingTrackingContainerQuery : IQuery
    {
        public string SearchAction { get; set; }
    }
}