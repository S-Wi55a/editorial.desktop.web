using System.Web;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Features.Details;
using Csn.Retail.Editorial.Web.Features.Errors;
using Csn.Retail.Editorial.Web.Features.Listings.Logger;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Expresso.Sanitisation;
using Expresso.Syntax;
using Expresso.Syntax.Binary;
using Expresso.Syntax.Rose;

namespace Csn.Retail.Editorial.Web.Infrastructure.Redirects
{
    [AutoBind]
    public class LegacyListingsUrlRedirectStrategy : IRedirectStrategy
    {
        private readonly IUrlRedirectionLogger _redirectLogger;
        private readonly IEditorialRyvussApiProxy _editorialRyvussApiProxy;
        private readonly ILegacyListingsRedirectHelper _legacyListingsRedirectHelper;

        public LegacyListingsUrlRedirectStrategy(IUrlRedirectionLogger redirectLogger,
            IEditorialRyvussApiProxy editorialRyvussApiProxy,
            ILegacyListingsRedirectHelper legacyListingsRedirectHelper)
        {
            _redirectLogger = redirectLogger;
            _editorialRyvussApiProxy = editorialRyvussApiProxy;
            _legacyListingsRedirectHelper = legacyListingsRedirectHelper;
        }

        public RedirectInstruction Apply(ActionExecutingContext filterContext)
        {
            // this also handles ?Make={make} and ?Type={type} pages since these
            // can be converted to binary expressions
            var originalQuery = filterContext.HttpContext.Request?.QueryString["q"] ??
                                filterContext.HttpContext.Request?.QueryString?.ToString();

            if (filterContext.RequestContext?.HttpContext?.Request?.Url != null &&
                originalQuery.IsRyvussBinaryTreeSyntax())
            {
                var url = GetRedirectionUrl(filterContext, originalQuery);
                if (!url.IsNullOrEmpty())
                {
                    _redirectLogger.Log(filterContext.HttpContext.Request.Url?.ToString());
                    return new RedirectInstruction
                    {
                        Url =
                            url, //Does double rediect: To be checked with merged code $"editorial/beta-results/{url}",
                        IsPermanent = true
                    };
                }
            }
            return RedirectInstruction.None;
        }

        public int Order => 1;

        #region"Internal functions"  

        private string GetRedirectionUrl(ActionExecutingContext filterContext, string query)
        {
            long offset = 0;
            long.TryParse(filterContext.HttpContext.Request?.QueryString["offset"] ?? "", out offset);
            var sortOrder = filterContext.HttpContext.Request?.QueryString["sort"] ?? "";
            var keyword = filterContext.HttpContext.Request?.QueryString["Keywords"] ?? "";

            return _legacyListingsRedirectHelper.GetRedirectionUrl(query, offset, sortOrder, keyword);
        }

        #endregion"Internal functions"  
    }
}