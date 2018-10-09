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
            // we can keep this instance static because we are only interested in the routing config
            // which does not change from one request to another
            _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        public string HttpRouteUrl(string routeName, object routeValues)
        {
            return _urlHelper.HttpRouteUrl(routeName, routeValues);
        }
    }
}