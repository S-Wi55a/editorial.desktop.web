using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Tracking.TrackingQuery
{
    public class GetGoogleAnalyticsDetailsTrackingQuery : IQuery
    {
        public ArticleViewModel Article { get; set; }
    }
}