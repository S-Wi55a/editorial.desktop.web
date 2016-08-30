using System.Web.Mvc;
using Csn.Logging;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Filters;
using Csn.Retail.Editorial.Web.Infrastructure.StartUpTasks;

namespace Csn.Retail.Editorial.Web
{
    [AutoBind]
    public class SetupGlobalFilterTask : IStartUpTask
    {
        public void Run()
        {
            GlobalFilters.Filters.Add(new GlobalErrorHandlerAttributeFilter());
        }
    }
}
