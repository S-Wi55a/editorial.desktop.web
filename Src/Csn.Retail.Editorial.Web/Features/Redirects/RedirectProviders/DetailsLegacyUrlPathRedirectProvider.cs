using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Expresso.Expressions;
using Expresso.Syntax;

namespace Csn.Retail.Editorial.Web.Features.Redirects.RedirectProviders
{
    [AutoBindSelf]
    public class DetailsLegacyUrlPathRedirectProvider : IRedirectProvider
    {
        private readonly IEditorialRyvussApiProxy _editorialRyvussApiProxy;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly IExpressionFormatter _expressionFormatter;

        public DetailsLegacyUrlPathRedirectProvider(IEditorialRyvussApiProxy editorialRyvussApiProxy, 
                                                ITenantProvider<TenantInfo> tenantProvider, 
                                                IExpressionFormatter expressionFormatter)
        {
            _editorialRyvussApiProxy = editorialRyvussApiProxy;
            _tenantProvider = tenantProvider;
            _expressionFormatter = expressionFormatter;
        }

        public string GetRedirectUrl(RedirectRule redirectRule, Uri uri)
        {
            // need to make sure that the regex is a match
            var regex = new Regex(redirectRule.MatchRule, RegexOptions.IgnoreCase);

            var absolutePathUnescaped = uri.AbsolutePathUnescaped();

            var matches = regex.Matches(absolutePathUnescaped);

            if (matches.Count != 1) return null;

            // we use ryvuss to try and find a match
            var result = _editorialRyvussApiProxy.Get<RyvussRedirectResponse>(new EditorialRyvussInput()
            {
                Query = _expressionFormatter.Format(new FacetExpression("Service", _tenantProvider.Current().Name).And(new FacetExpression("LegacyUrlPath", absolutePathUnescaped))),
                IncludeSearchResults = true,
                Limit = 1,
                Offset = 0,
                IncludeCount = true
            });

            if (result.IsSucceed && result.Data?.Count != 1) return null;

            var hit = result.Data.SearchResults?.FirstOrDefault();

            if (hit == null || string.IsNullOrEmpty(hit.DetailsPageUrlPath)) return null;

            return hit.DetailsPageUrlPath;
        }

        private class RyvussRedirectResponse
        {
            public int  Count { get; set; }
            public List<EditorialItem> SearchResults { get; set; }
        }

        private class EditorialItem
        {
            public string Id { get; set; }
            public string DetailsPageUrlPath { get; set; }

        }
    }
}