using System.Collections.Generic;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.Owin;
using Owin;
using System.Linq;
using System.Web.Http;
using Autofac.Integration.WebApi;
using Csn.Retail.Editorial.Web.Infrastructure.StartUpTasks;
using Ingress.Autofac;

[assembly: OwinStartupAttribute(typeof(Csn.Retail.Editorial.Web.Startup))]
namespace Csn.Retail.Editorial.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var mvcAssembly = typeof(MvcApplication).Assembly;

            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            builder.RegisterControllers(mvcAssembly);
            builder.RegisterAssemblyModules(mvcAssembly, typeof(Csn.SimpleCqrs.AutoFac.DispatchersModule).Assembly);
            builder.RegisterApiControllers(mvcAssembly);
            builder.RegisterHttpRequestMessage(config);
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterModelBinders(mvcAssembly);
            builder.RegisterModelBinderProvider();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            RunStartUpTasks(container);
            ReactConfig.Configure(container);
            container.RunBootstrapperTasks();
            
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
