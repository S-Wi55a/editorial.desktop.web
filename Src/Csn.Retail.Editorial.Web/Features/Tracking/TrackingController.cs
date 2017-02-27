using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Tracking.WebMetricsScripts;
using Csn.SimpleCqrs;

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
    }
}