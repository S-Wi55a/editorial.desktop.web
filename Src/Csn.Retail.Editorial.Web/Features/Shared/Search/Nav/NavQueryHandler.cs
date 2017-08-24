using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Nav
{
    [AutoBind]
    public class NavQueryHandler : IAsyncQueryHandler<NavQuery, NavResult>
    {
        private readonly IEditorialRyvussApiProxy _ryvussProxy;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly IMapper _mapper;

        public NavQueryHandler(IEditorialRyvussApiProxy ryvussProxy, ITenantProvider<TenantInfo> tenantProvider, IMapper mapper)
        {
            _ryvussProxy = ryvussProxy;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
        }

        public async Task<NavResult> HandleAsync(NavQuery query)
        {
            var result = await _ryvussProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInputNew()
            {
                Query = string.IsNullOrEmpty(query.Query) ? $"Service.{_tenantProvider.Current().Name}." : query.Query,
                Limit = 10,
                Offset = 0,
                SortOrder = "Latest",
                IncludeCount = true,
                IncludeSearchResults = true,
                NavigationName = "RetailNav",
                PostProcessors = new List<string> { "Retail", "ShowZero"}
            });

            var resultData = !result.IsSucceed ? null : result.Data;

            return resultData == null ? null : _mapper.Map<NavResult>(resultData);
        }
    }

    #region Ryvuss Dto

    public class RyvussNavResultDto
    {
        public int Count { get; set; }
        public RyvussNavDto INav { get; set; }
        public List<SearchResultDto> SearchResults { get; set; }
    }

    public class RyvussNavDto
    {
        public List<RyvussNavNodeDto> Nodes { get; set; }
        public List<BreadCrumbDto> BreadCrumbs { get; set; }
    }

    public class RyvussNavNodeDto
    {
        public bool IsSelected { get; set; }
        public string MultiSelectMode { get; set; }
        public List<FacetNodeDto> Facets { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }

    public class FacetNodeDto
    {
        public bool IsSelected { get; set; }
        public string Value { get; set; }
        public string DisplayValue { get; set; }
        public string Action { get; set; }
        public int Count { get; set; }
        public string Expression { get; set; }
        public FacetNodeMetaDataDto MetaData { get; set; }
    }

    public class FacetNodeMetaDataDto
    {
        public List<bool> IsRefineable { get; set; }
    }

    public class BreadCrumbDto
    {
        public string Aspect { get; set; }
        public string AspectDisplay { get; set; }
        public string Facet { get; set; }
        public string FacetDisplay { get; set; }
        public string RemoveAction { get; set; }
    }

    public class SearchResultDto
    {
        public string Id { get; set; }
        public string Slug { get; set; }
        public string Headline { get; set; }
        public DateTime DateAvailable { get; set; }
        public string PhotoPath { get; set; }
    }

    #endregion
}