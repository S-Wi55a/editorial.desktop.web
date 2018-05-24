using System.Web.Mvc;
using Csn.Logging;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using ILogger = Csn.Logging.ILogger;

namespace Csn.Retail.Editorial.Web.Infrastructure.Filters
{
    public class UnknownTenantFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction) return;

            var tenantProvider = DependencyResolver.Current.GetService<ITenantProvider<TenantInfo>>();

            if (tenantProvider.Current().Name != "default") return;

            var logger = DependencyResolver.Current.GetService(typeof(IUnknownTenantLogger)) as UnknownTenantLogger;
            logger?.Log(filterContext.HttpContext.Request.Url?.ToString());

            filterContext.Result = new HttpStatusCodeResult(404);
        }
    }

    public interface IUnknownTenantLogger
    {
        void Log(string requestUrl);
    }

    public class UnknownTenantLogger : IUnknownTenantLogger
    {
        private readonly ILogger _logger;

        public UnknownTenantLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(string requestUrl)
        {
            _logger.Trace(requestUrl);
        }
    }
}