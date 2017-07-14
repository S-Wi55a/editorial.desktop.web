using System;
using System.Web.Mvc;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds
{
    public class MediaMotiveAdsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        public MediaMotiveAdsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [ChildActionOnly]
        [Route("MediaMotiveAds")]
        public ActionResult Index(MediaMotiveAdQuery query)
        {
            var viewData = _queryDispatcher.Dispatch<MediaMotiveAdQuery, MediaMotiveAdViewModel>(query);

            if (viewData == null) return Content(String.Empty);

            return PartialView(viewData);
        }
    }
}