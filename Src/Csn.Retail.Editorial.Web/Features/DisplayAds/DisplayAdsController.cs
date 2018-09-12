using System.Web.Mvc;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.DisplayAds.GoogleAd.Models;
using Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive.Models;
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
        public ActionResult RenderDisplayAd(DisplayAdQuery query)
        {
            if (_tenantProvider.Current().UseGoogleAd)
            {
                return RenderGoogleAd(query);
            }

            return RenderMediaMotiveAd(query);
        }

        private ActionResult RenderGoogleAd(DisplayAdQuery query)
        {
            var viewData = _queryDispatcher.Dispatch<DisplayAdQuery, GoogleAdViewModel>(query);

            if (viewData == null) return Content(string.Empty);

            return PartialView($"~/Features/DisplayAds/GoogleAd/Views/Index.cshtml", viewData);
        }

        private ActionResult RenderMediaMotiveAd(DisplayAdQuery query)
        {
            var viewData = _queryDispatcher.Dispatch<DisplayAdQuery, MediaMotiveAdViewModel>(query);

            if (viewData == null) return Content(string.Empty);

            return PartialView($"~/Features/DisplayAds/MediaMotive/Views/Index.cshtml", viewData);
        }

        [ChildActionOnly]
        public ActionResult RenderDisplayAdsHeader()
        {
            if(_tenantProvider.Current().UseGoogleAd)
                return PartialView("Partials/GoogleAd/_GoogleAd");
            else
                return Content(string.Empty);
        }
        
        [ChildActionOnly]
        public ActionResult RenderDisplayAdsFooter()
        {
            if (_tenantProvider.Current().UseMediaMotive)
                return PartialView("Partials/Mediamotive/_Krux");
            else
                return Content(string.Empty);
        }
    }
}