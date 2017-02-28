using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Tracking.TrackingQuery
{
    public class GetDetailsTrackingQuery : IQuery
    {
        public ArticleViewModel Article { get; set; }
        public string PageType { get; set; }
    }
}