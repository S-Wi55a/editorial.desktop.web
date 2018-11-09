using System.Web.Mvc;
using Csn.Logging;

namespace Csn.Retail.Editorial.Web.Infrastructure.Filters
{
    public class GlobalErrorHandlerAttributeFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var logger = DependencyResolver.Current.GetService<ILogger>();
            logger.Error(filterContext.Exception, filterContext.Exception.Message);
        }
    }
}