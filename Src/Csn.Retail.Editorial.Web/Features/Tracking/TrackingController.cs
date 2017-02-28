using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Tracking.TrackingQuery;
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
        public ActionResult HtmlTrackingDetails(GetDetailsTrackingQuery query)
        {
            var container = _queryDispatcher.Dispatch<GetDetailsTrackingQuery, IAnalyticsTrackingContainer>(query);
            if (container == null) return null;
            return PartialView("HtmlTracking", model: container.GenericHtmlTracking);
        }

        public ActionResult GoogleAnalyticsTrackingDetails(GetGoogleAnalyticsDetailsTrackingQuery query)
        {
            var container = _queryDispatcher.Dispatch<GetGoogleAnalyticsDetailsTrackingQuery, IAnalyticsTrackingContainer>(query);
            if (container == null) return null;
            return PartialView("HtmlTracking", model: container.GenericHtmlTracking);
        }
    }
}