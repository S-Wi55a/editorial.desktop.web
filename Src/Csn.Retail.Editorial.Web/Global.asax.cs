using System.Web.Mvc;
using System.Web.Http;
using Csn.Retail.Editorial.Web.Infrastructure.ModelBinders;

namespace Csn.Retail.Editorial.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            //RegisterModelBinders();
        }

        //private static void RegisterModelBinders()
        //{
        //    ModelBinders.Binders.DefaultBinder = new SmartModelBinder(DependencyResolver.Current.GetServices<IFilteredModelBinder>());
        //}
    }
}
