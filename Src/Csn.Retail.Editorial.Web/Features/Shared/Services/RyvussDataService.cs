using System.Collections.Generic;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Listings.Constants;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Shared.Services
{
    public interface IRyvussDataService
    {
        Task<RyvussNavResultDto> GetRefinements(RefinementsQuery query);
        Task<RyvussNavResultDto> GetNavAndResults(string query, bool includeResults, string sort = "", int offset = 0);
        Task<RyvussNavResultDto> GetResults(string query, int offset, string sort);
    }

    [AutoBind]
    public class RyvussDataService : IRyvussDataService
    {
        private readonly IEditorialRyvussApiProxy _ryvussProxy;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public RyvussDataService(IEditorialRyvussApiProxy ryvussProxy, ITenantProvider<TenantInfo> tenantProvider)
        {
            _ryvussProxy = ryvussProxy;
            _tenantProvider = tenantProvider;
        }

        public async Task<RyvussNavResultDto> GetNavAndResults(string query, bool includeResults, string sort = "", int offset = 0)
        {
            var postProcessors = new List<string> { "Retail", "FacetSort" };
            AddPostprocessors(postProcessors);

            return await GetRyvusProxyResults(query, postProcessors, includeResults, sort, offset);
        }

        public async Task<RyvussNavResultDto> GetResults(string query, int offset, string sort)
        {
            var result = await _ryvussProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInput
            {
                Query = query,
                IncludeSearchResults = true,
                Offset = offset,
                Limit = 7,
                SortOrder = sort,
                IncludeCount = true
            });

            return !result.IsSucceed ? null : result.Data;
        }

        public async Task<RyvussNavResultDto> GetRefinements(RefinementsQuery query)
        {
            var postProcessors = new List<string> { "FacetSort", $"RetailAspectRefinements({query.RefinementAspect},{query.ParentExpression})" };

            AddPostprocessors(postProcessors);

            return await GetRyvusProxyResults(query.Q, postProcessors);
        }

        #region Private
        private async Task<RyvussNavResultDto> GetRyvusProxyResults(string query, List<string> postProcessors, bool includeResults = false, string sort = "", int offset = 0)
        {
            var ryvusInput = new EditorialRyvussInput
            {
                Query = query,
                IncludeCount = true,
                IncludeSearchResults = includeResults,
                ControllerName = _tenantProvider.Current().SupportsSeoFriendlyListings ? $"seo-{_tenantProvider.Current().Name}" : null,
                ServiceProjectionName = _tenantProvider.Current().SupportsSeoFriendlyListings ? _tenantProvider.Current().ServiceProjection : "",
                NavigationName = _tenantProvider.Current().RyvusNavName,
                PostProcessors = postProcessors,
                IncludeMetaData = true
            };

            if (includeResults)
            {
                ryvusInput.Offset = offset;
                ryvusInput.Limit = PageItemsLimit.ListingPageItemsLimit;
                ryvusInput.SortOrder = EditorialSortKeyValues.IsValidSort(sort) ? sort : EditorialSortKeyValues.ListingPageDefaultSort;
            }

            var result = await _ryvussProxy.GetAsync<RyvussNavResultDto>(ryvusInput);

            return !result.IsSucceed ? null : result.Data;
        }

        private void AddPostprocessors(ICollection<string> postProcessors)
        {
            if (_tenantProvider.Current().SupportsSeoFriendlyListings)
            {
                postProcessors.Add("Seo");
                postProcessors.Add("HideAspect(Service)");
            }
            else
            {
                postProcessors.Add("ShowZero");
            }
            postProcessors.Add("RenderRefinements");
        }
        #endregion
    }
}