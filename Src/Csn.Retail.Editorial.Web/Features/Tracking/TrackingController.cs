using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Tracking.TrackingContainer;
using Csn.Retail.Editorial.Web.Features.Tracking.WebMetricsScripts;
using Csn.SimpleCqrs;
using Csn.WebMetrics.Core.Model;

namespace Csn.Retail.Editorial.Web.Features.Tracking
{
    public class TrackingController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public TrackingController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }


        [ChildActionOnly]
        public ActionResult WebMetricsScripts(WebMetricsScriptsQuery query)
        {
            var model = _queryDispatcher.Dispatch<WebMetricsScriptsQuery, WebMetricsScriptsViewModel>(query);
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult HtmlTrackingDetails(GetDetailsTrackingContainerQuery query)
        {
            var container = _queryDispatcher.Dispatch<GetDetailsTrackingContainerQuery, IAnalyticsTrackingContainer>(query);

            if (container == null) return null;

            return PartialView("HtmlTracking", model: container.GenericHtmlTracking);
        }

        [ChildActionOnly]
        public ActionResult HtmlTrackingListing(GetListingTrackingContainerQuery containerQuery)
        {
            var container = _queryDispatcher.Dispatch<GetListingTrackingContainerQuery, IAnalyticsTrackingContainer>(containerQuery);

            if (container == null) return null;

            return PartialView("HtmlTracking", model: container.GenericHtmlTracking);
        }
    }
}