using System.Web;
using System.Web.Mvc;

namespace Csn.Retail.Editorial.Web.Infrastructure.Wrappers
{
    public interface IUrlRouteHelper
    {
        string HttpRouteUrl(string routeName, object routeValues);
    }

    public class UrlRouteHelper : IUrlRouteHelper
    {
        private readonly UrlHelper _urlHelper;

        public UrlRouteHelper()
        {
            _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        public string HttpRouteUrl(string routeName, object routeValues)
        {
            return _urlHelper.HttpRouteUrl(routeName, routeValues);
        }
    }
}