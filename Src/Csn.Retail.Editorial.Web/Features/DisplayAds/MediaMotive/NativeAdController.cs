using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive.Services;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive
{
    public class NativeAdController : Controller
    {
        private readonly INativeAdService _nativeAdService;

        public NativeAdController(INativeAdService nativeAdService)
        {
            _nativeAdService = nativeAdService;
        }

        [ChildActionOnly]
        public ActionResult Index()
        {
            
            var nativeAd = _nativeAdService.GetNativeAdvert();
            
            return new ContentResult { Content = nativeAd };
        }
    }
}