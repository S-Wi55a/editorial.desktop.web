using System.Collections.Generic;
using System.Web.Mvc;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Constants;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;

namespace Csn.Retail.Editorial.Web.Features.Shared.Helpers
{
    //TODO: Refactoring and cleanup required for this class
    public static class ListingUrlHelper
    {
        private static string ListingsBasePath(string query = "", bool includeResultsSegment = false)
        {
            // slap on the wrist! We are using service locator because of difficulty getting dependency injection on automapper resolvers
            // to work without massive code refactor
            var tenantProvider = DependencyResolver.Current.GetService<ITenantProvider<TenantInfo>>();

            if (!string.IsNullOrEmpty(query) || includeResultsSegment)
                return $"/editorial{(string.IsNullOrEmpty(tenantProvider.Current().Vertical) ? "" : $"/{tenantProvider.Current().Vertical.ToLower()}")}/results";

            return $"/editorial{(string.IsNullOrEmpty(tenantProvider.Current().Vertical) ? "" : $"/{tenantProvider.Current().Vertical.ToLower()}")}";
        }

        public static string GetPathAndQueryString(string q = null, long offset = 0, string sortOrder = null, string keyword = null, bool includeResultsSegment = false)
        {
            var queryParams = GetQueryStringParameters(q, offset, sortOrder, keyword);
            var queryString = string.IsNullOrEmpty(queryParams) ? "/" : "/?" + queryParams;

            return $"{ListingsBasePath(queryString, includeResultsSegment)}{queryString}";
        }

        public static string GetQueryString(string action, string sort)
        {
            var queryParams = GetQueryStringParameters(action, 0, sort, string.Empty);
            return string.IsNullOrEmpty(queryParams) ? string.Empty : "?" + queryParams;
        }

        public static string GetSeoUrl(string seofragment, long offset = 0, string sortOrder = null)
        {
            var query = GetQueryStringParameters("", offset, sortOrder, "");

            var pathAndQuery = string.IsNullOrEmpty(seofragment) 
                ? (string.IsNullOrEmpty(query) ? "" : "?" + query)
                : $"{seofragment}{(string.IsNullOrEmpty(query) ? "" : "?" + query)}";
            return $"{ListingsBasePath()}{pathAndQuery}";
        }

        public static string GetPageAndSortPathAndQuery(string q = null, long offset = 0, string sortOrder = null, string keyword = null, string seoFragment = "")
        {
            var queryParams = GetQueryStringParameters(string.IsNullOrEmpty(seoFragment) ? q : null , offset, sortOrder, keyword);
            var queryString = string.IsNullOrEmpty(seoFragment)
                ? (string.IsNullOrEmpty(queryParams) ? "/" : "/?" + queryParams)
                : $"{seoFragment}{(string.IsNullOrEmpty(queryParams) ? "/" : "?" + queryParams)}";
            var includeResultsSegment =
                string.IsNullOrEmpty(seoFragment) && string.IsNullOrEmpty(q) || !string.IsNullOrEmpty(q);
            return $"{ListingsBasePath(includeResultsSegment: includeResultsSegment)}{queryString}";
        }


        private static string GetQueryStringParameters(string q, long offset, string sortOrder, string keyword)
        {
            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(q)) queryParams[EditorialQueryStringParams.Query] = q;
            if (!string.IsNullOrEmpty(sortOrder)) queryParams[EditorialQueryStringParams.Sort] = sortOrder;
            if (!string.IsNullOrEmpty(keyword)) queryParams[EditorialQueryStringParams.Keywords] = keyword;
            if (offset != 0) queryParams[EditorialQueryStringParams.Offset] = offset.ToString();

            return queryParams.ToQueryString();
        }
    }
}