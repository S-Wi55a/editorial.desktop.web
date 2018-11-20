using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.GlobalSite;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Errors
{
    public class ErrorsController : Controller
    {
        private readonly IUrlNotFoundLogger _urlNotFoundLogger;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public ErrorsController(IUrlNotFoundLogger urlNotFoundLogger, IEventDispatcher eventDispatcher, ITenantProvider<TenantInfo> tenantProvider)
        {
            _urlNotFoundLogger = urlNotFoundLogger;
            _eventDispatcher = eventDispatcher;
            _tenantProvider = tenantProvider;
        }

        /// <summary>
        /// Handles valid/known routes but unknown url fragments or not found articles
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult Error404Child()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;

            _urlNotFoundLogger.Log(HttpContext.Request.Url?.ToString());

            return View($"~/Features/Errors/Views/{_tenantProvider.Current().Culture}/Error404.cshtml");
        }

        /// <summary>
        /// Handles invalid/unknown routes in application and renders sitenav and analytics data with error message
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Error404CatchAll()
        {
            await _eventDispatcher.DispatchAsync(new ErrorPageRequestEvent());

            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;

            _urlNotFoundLogger.Log(HttpContext.Request.Url?.ToString());

            return View($"~/Features/Errors/Views/{_tenantProvider.Current().Culture}/Error404.cshtml");
        }

        [ChildActionOnly]
        public ActionResult ErrorGenericChild()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Response.TrySkipIisCustomErrors = true;

            _urlNotFoundLogger.Log(HttpContext.Request.Url?.ToString());

            return View($"~/Features/Errors/Views/{_tenantProvider.Current().Culture}/ErrorGeneric.cshtml");
        }

        public async Task<ActionResult> ErrorGeneric()
        {
            await _eventDispatcher.DispatchAsync(new ErrorPageRequestEvent());

            return ErrorGenericChild();
        }
    }

    public class ErrorPageRequestEvent : IEvent, IRequireGlobalSiteNav, IRequiredGoogleAnalyticsTrackingScript
    {
    }
}