using System;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.DisplayAds.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds
{
    [AutoBind]
    public class DisplayAdsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        public DisplayAdsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [ChildActionOnly]
        public ActionResult MediaMotive(DisplayAdsQuery query)
        {
            var viewData = _queryDispatcher.Dispatch<DisplayAdsQuery, IDisplayAdsModel>(query);

            if (viewData == null) return Content(String.Empty);
            
            return PartialView("~/Features/DisplayAds/Views/MediaMotive.cshtml", (MediaMotiveAdViewModel)viewData);
        }

        [ChildActionOnly]
        public ActionResult GoogleAds(DisplayAdsQuery query)
        {
            var viewData = _queryDispatcher.Dispatch<DisplayAdsQuery, IDisplayAdsModel>(query);

            if (viewData == null) return Content(String.Empty);
            
            return PartialView("~/Features/DisplayAds/Views/GoogleAds.cshtml", (GoogleAdsViewModel)viewData);
        }
    }
}