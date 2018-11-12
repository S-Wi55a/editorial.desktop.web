using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Infrastructure.Wrappers;

namespace Csn.Retail.Editorial.Web.Features.Redirects
{
    public class RedirectController : Controller
    {
        private readonly IRedirectService _redirectService;
        private readonly IRedirectLogger _redirectLogger;
        private readonly IUrlRouteHelper _urlRouteHelper;

        public RedirectController(IRedirectService redirectService, IRedirectLogger redirectLogger, IUrlRouteHelper urlRouteHelper)
        {
            _redirectService = redirectService;
            _redirectLogger = redirectLogger;
            _urlRouteHelper = urlRouteHelper;
        }

        public ActionResult Redirect()
        {
            var redirect = _redirectService.GetRedirect();

            _redirectLogger.Log(redirect, HttpContext.Request.Url?.AbsolutePath);

            if (redirect != null && redirect.RedirectResult != null)
            {
                return redirect.RedirectResult;
            }

            return new RedirectResult(_urlRouteHelper.HttpRouteUrl(RouteNames.Mvc.LandingHome, null), true);
        }
    }
}