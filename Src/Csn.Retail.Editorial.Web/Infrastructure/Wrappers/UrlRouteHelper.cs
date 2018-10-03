using System.Web;

namespace Csn.Retail.Editorial.Web.Infrastructure.Wrappers
{
    public interface IUrlRouteHelper
    {
        string HttpRouteUrl(string routeName, object routeValues);
    }

    public class UrlRouteHelper : IUrlRouteHelper
    {
        public string HttpRouteUrl(string routeName, object routeValues)
        {
            var urlHelper = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);

            return urlHelper.HttpRouteUrl(routeName, routeValues);
        }
    }
}