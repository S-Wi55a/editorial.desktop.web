using System;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds
{
    public class DisplayAdsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        public DisplayAdsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [ChildActionOnly]
        public ActionResult Index(DisplayAdsQuery query)
        {
            var viewData = _queryDispatcher.Dispatch<DisplayAdsQuery, IDisplayAdsModel>(query);

            if (viewData == null) return Content(String.Empty);

            //return PartialView("~/Features/MediaMotiveAds/Views/Index.cshtml", viewData);
        }
    }
}