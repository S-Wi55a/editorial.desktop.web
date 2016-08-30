using System.Web.Mvc;
using Csn.MultiTenant.Mvc5;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.StartUpTasks;

namespace Csn.Retail.Editorial.Web
{
    [AutoBind]
    public class SetupViewEngineTask : IStartUpTask
    {
        private readonly MultiTenantViewEngine<TenantInfo> _viewEngine;

        public SetupViewEngineTask(MultiTenantViewEngine<TenantInfo> viewEngine)
        {
            _viewEngine = viewEngine;
        }

        public void Run()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(_viewEngine);
        }
    }
}