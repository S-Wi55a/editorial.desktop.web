using System.Web.Mvc;
using System.Web.Http;

namespace Csn.Retail.Editorial.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
        }
    }
}
