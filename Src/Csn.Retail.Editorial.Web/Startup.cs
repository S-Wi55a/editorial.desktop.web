using System.Collections.Generic;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.Owin;
using Owin;
using System.Linq;
using Csn.Retail.Editorial.Web.Infrastructure.StartUpTasks;

[assembly: OwinStartupAttribute(typeof(Csn.Retail.Editorial.Web.Startup))]
namespace Csn.Retail.Editorial.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var mvcAssembly = typeof(MvcApplication).Assembly;

            var builder = new ContainerBuilder();
            builder.RegisterControllers(mvcAssembly);
            builder.RegisterAssemblyModules(mvcAssembly, typeof(Csn.SimpleCqrs.AutoFac.DispatchersModule).Assembly);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            RunStartUpTasks(container);
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
        }

        private void RunStartUpTasks(IContainer container)
        {
            var tasks = container.Resolve<IEnumerable<IStartUpTask>>() ?? Enumerable.Empty<IStartUpTask>();
            foreach (var startUpTask in tasks)
            {
                startUpTask.Run();
            }
        }
    }
}
