using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds.Services;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds
{
    public class NativeAdController : Controller
    {
        private readonly INativeAdService _nativeAdService;

        public NativeAdController(INativeAdService nativeAdService)
        {
            _nativeAdService = nativeAdService;
        }

        [ChildActionOnly]
        [Route("NativeAd")]
        public ActionResult Index()
        {
            
            var nativeAd = _nativeAdService.GetNativeAdvert();
            
            return new ContentResult { Content = nativeAd };
        }
    }
}