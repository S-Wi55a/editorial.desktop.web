using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Features.Details;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Expresso.Sanitisation;
using Expresso.Syntax;
using Expresso.Syntax.Binary;
using Expresso.Syntax.Rose;

namespace Csn.Retail.Editorial.Web.Infrastructure.Redirects
{
    public class EditorialMetadataDto
    {
        public Metadata Metadata { get; set; }
    }

    public class Metadata
    {
        // ReSharper disable once InconsistentNaming - needed for Ryvus response parsing
        public string query { get; set; }

        public string Seo { get; set; }
    }

    [AutoBind]
    public class LegacyUrlRedirectStrategy : IRedirectStrategy
    {
        private readonly IDetailsRedirectLogger _redirectLogger;
        private readonly IExpressionFormatter _formatter;
        private readonly IEditorialRyvussApiProxy _editorialRyvussApiProxy;

        public LegacyUrlRedirectStrategy(IDetailsRedirectLogger redirectLogger,
            IEditorialRyvussApiProxy editorialRyvussApiProxy)
        {
            _redirectLogger = redirectLogger;
            _formatter = new RoseTreeFormatter(new RoseTreeSanitiser());
            _editorialRyvussApiProxy = editorialRyvussApiProxy;
        }

        public RedirectInstruction Apply(ActionExecutingContext filterContext)
        {
            var originalQuery = filterContext.HttpContext.Request?.QueryString["q"] ??
                                filterContext.HttpContext.Request?.QueryString?.ToString();

            if (filterContext.RequestContext?.HttpContext?.Request?.Url != null &&
                IsRyvussBinaryTreeSyntax(originalQuery))
            {
                return GetRedirectionInstructions(filterContext);
            }

            return RedirectInstruction.None;
        }

        public int Order => 1;

        #region"Internal functions"  

        public RedirectInstruction GetRedirectionInstructions(ActionExecutingContext filterContext)
        {
            var query = filterContext.HttpContext.Request?.QueryString["q"] ??
                        filterContext.HttpContext.Request?.QueryString.ToString();

            var response = _editorialRyvussApiProxy.Get<EditorialMetadataDto>(query);
            if (response.IsSucceed && response.Data.Metadata != null)
            {
                var url = GetRedirectionSlug(filterContext, response.Data.Metadata);
                if (!url.IsNullOrEmpty())
                {
                    _redirectLogger.Log(
                        $"{filterContext.HttpContext.Request?.QueryString.ToString()} redirected to {url}");
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

        public bool IsRyvussBinaryTreeSyntax(string query)
        {
            var parser = new FlatBinaryTreeParser(new BinaryTreeSanitiser());
            try
            {
                var parsed = parser.Parse(query);
                return true;
            }
            catch
            {
                //An exception shows it isn't parsed by binary - therefore is V4
            }
            return false;
        }

        public string GetRedirectionSlug(ActionExecutingContext filterContext, Metadata queryMetaData)
        {
            var offset = filterContext.HttpContext.Request?.QueryString["offset"] ?? "";
            var sortOrder = filterContext.HttpContext.Request?.QueryString["sort"] ?? "";
            var keyword = filterContext.HttpContext.Request?.QueryString["Keywords"] ?? "";
            var url = "";
            var isSeo = false;

            if (!queryMetaData.Seo.IsNullOrEmpty() && keyword.IsNullOrEmpty())
            {
                isSeo = true;
                url = $"{queryMetaData.Seo}";
            }
            else if (!queryMetaData.query.IsNullOrEmpty())
            {
                url = $"?Q={queryMetaData.query}";
            }

            if (!url.IsNullOrEmpty())
                url = url + GetQueryParametersForSlug(isSeo, keyword, offset, sortOrder);

            return url;
        }

        public string GetQueryParametersForSlug(bool isSeo, string keyword, string offset, string sortOrder)
        {
            if (!isSeo)
            {
                return (offset.IsNullOrEmpty() ? "" : $"&offset={offset}") +
                       (sortOrder.IsNullOrEmpty() ? "" : $"&sortOrder={sortOrder}") +
                       (keyword.IsNullOrEmpty() ? "" : $"&keyword={keyword}");
            }
            var seoSlug = (!offset.IsNullOrEmpty() || !sortOrder.IsNullOrEmpty() ? "?" : "") +
                          (offset.IsNullOrEmpty() ? "" : $"offset={offset}");
            if (sortOrder != "")
                seoSlug = seoSlug + (offset.IsNullOrEmpty() ? "" : "&") + $"sortOrder={sortOrder}";

            return seoSlug;
        }

        #endregion"Internal functions"  
    }
}