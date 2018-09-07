using System;
using System.Web.Mvc;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.DisplayAds.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds
{
    [AutoBind]
    public class DisplayAdsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public DisplayAdsController(IQueryDispatcher queryDispatcher, ITenantProvider<TenantInfo> tenantProvider)
        {
            _queryDispatcher = queryDispatcher;
            _tenantProvider = tenantProvider;
        }

        [ChildActionOnly]
        public ActionResult DisplayAds(DisplayAdsQuery query)
        {
            var viewData = _queryDispatcher.Dispatch<DisplayAdsQuery, IDisplayAdsModel>(query);

            if (viewData == null) return Content(String.Empty);

            if (viewData.DisplayAdsSource == DisplayAdsSource.MediaMotive)
            {
                return PartialView($"~/Features/DisplayAds/Views/MediaMotive.cshtml", (MediaMotiveAdViewModel)viewData);
            }

            return PartialView("~/Features/DisplayAds/Views/GoogleAds.cshtml", (GoogleAdsViewModel)viewData);
        }

        [ChildActionOnly]
        public ActionResult RenderHeader()
        {
            if (_tenantProvider.Current().UseGoogleAds)
            {
                return PartialView($"~/Features/Shared/Views/Partials/_GoogleAds");
            }

            return Content(String.Empty);
        }

        [ChildActionOnly]
        public ActionResult RenderFooter()
        {
            if (_tenantProvider.Current().UseMediaMotive)
            {
                // NOTE: if there are other partials required then just create a parent partial to contain them all
                return PartialView($"~/Features/Shared/Views/Partials/Mediamotive/Krux");
            }

            return Content(String.Empty);
        }
    }
}