using System;
using System.Web.Mvc;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds
{
    public class MediaMotiveDetailsAdController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        public MediaMotiveDetailsAdController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [ChildActionOnly]
        [Route("MediaMotiveDetailsAd")]
        public ActionResult Index(MediaMotiveDetailsAdQuery query)
        {
            var viewData = _queryDispatcher.Dispatch<MediaMotiveDetailsAdQuery, MediaMotiveAdViewModel>(query);

            if (viewData == null) return Content(String.Empty);

            return PartialView("~/Features/MediaMotiveAds/Views/Index.cshtml", viewData);
        }
    }
}