using System.Web.Mvc;

namespace Csn.Retail.Editorial.Web.Infrastructure.Redirects
{
    public interface IRedirectStrategy
    {
        RedirectInstruction Apply(ActionExecutingContext filterContext);
        int Order { get; }
    }
}