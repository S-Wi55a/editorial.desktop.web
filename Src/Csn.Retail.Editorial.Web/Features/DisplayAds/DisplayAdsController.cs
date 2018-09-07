using System;
using System.Web.Mvc;
using Csn.MultiTenant;
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

            if (viewData == null) return Content(string.Empty);

            //if (viewData.DisplayAdsSource == DisplayAdsSource.MediaMotive)
            //{
                return PartialView($"~/Features/DisplayAds/{viewData.DisplayAdsSource}/Views/Index.cshtml", viewData);
            //}

            //return PartialView("~/Features/DisplayAds/GoogleAd/Views/GoogleAd.cshtml", (GoogleAdViewModel)viewData);
        }

        [ChildActionOnly]
        public ActionResult RenderDisplayAdsHeader()
        {
            // NOTE: if there are other partials required then just create a parent partial to contain them all
            if(_tenantProvider.Current().UseGoogleAds)
                return PartialView("Partials/GoogleAd/_GoogleAd");
            else
                return Content(string.Empty);
        }
        
        [ChildActionOnly]
        public ActionResult RenderDisplayAdsFooter()
        {
            // NOTE: if there are other partials required then just create a parent partial to contain them all
            if (_tenantProvider != null && _tenantProvider.Current().UseMediaMotive)
                return PartialView("Partials/Mediamotive/_Krux");
            else
                return Content(string.Empty);
        }
    }
}