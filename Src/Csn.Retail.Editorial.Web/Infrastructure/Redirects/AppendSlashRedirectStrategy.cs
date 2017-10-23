using System.Web;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Infrastructure.Redirects
{
    [AutoBind]
    public class AppendSlashRedirectStrategy : IRedirectStrategy
    {
        public RedirectInstruction Apply(ActionExecutingContext filterContext)
        {
            var path = filterContext.RequestContext.HttpContext.Request.Url?.AbsolutePath;

            // skip if url has trailing slash or extension (.aspx, .html, .pdf etc.)
            if (path != null && !path.EndsWith("/") && !System.IO.Path.HasExtension(path))
            {
                return new RedirectInstruction
                {
                    Url = $"{VirtualPathUtility.AppendTrailingSlash(path)}{filterContext.RequestContext.HttpContext.Request.Url?.Query}",
                    IsPermanent = true
                };
            }

            return RedirectInstruction.None;
        }

        public int Order => 1;
    }
}