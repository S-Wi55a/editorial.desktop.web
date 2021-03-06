﻿using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Expresso.Sanitisation;
using Expresso.Syntax;
using Expresso.Syntax.Binary;

namespace Csn.Retail.Editorial.Web.Features.Listings.Helpers
{
    public interface ILegacyListingsRedirectHelper
    {
        string GetRedirectionUrl(string query, long offset, string sortOrder, string keyword);
    }
    
    [AutoBind]
    public class LegacyListingsRedirectHelper : ILegacyListingsRedirectHelper
    {
        private readonly IEditorialRyvussApiProxy _editorialRyvussApiProxy;
        private readonly IExpressionFormatter _expressionFormatter;

        public LegacyListingsRedirectHelper(IEditorialRyvussApiProxy editorialRyvussApiProxy,
            IExpressionFormatter expressionFormatter)
        {
            _editorialRyvussApiProxy = editorialRyvussApiProxy;
            _expressionFormatter = expressionFormatter;
        }

        public string GetRedirectionUrl(string query, long offset, string sortOrder, string keyword)
        {
            var formattedQuery = query;

            if (!string.IsNullOrEmpty(keyword))
            {
                var parser = new FlatBinaryTreeParser(new BinaryTreeSanitiser());
                formattedQuery = _expressionFormatter.Format(parser.Parse(query).AppendOrUpdateKeywords(keyword));
            }

            var response = _editorialRyvussApiProxy.Get<EditorialSeoDto>(new EditorialRyvussInput()
            {
                Query = formattedQuery,
                IncludeMetaData = true
            });

            if (!response.IsSucceed) return null;

            return GetRedirectionUrl(response.Data, keyword, offset, sortOrder);
        }

        private string GetRedirectionUrl(EditorialSeoDto seoDto, string keyword, long offset, string sortOrder)
        {
            if (seoDto.HasSeo())
                return ListingUrlHelper.GetSeoUrl(seoDto.Metadata.Seo, offset, sortOrder);

            return ListingUrlHelper.GetPathAndQueryString(seoDto.Metadata.query,offset, sortOrder, keyword);
        }
    }
}