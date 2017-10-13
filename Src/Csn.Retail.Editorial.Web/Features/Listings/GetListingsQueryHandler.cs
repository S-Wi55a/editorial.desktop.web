using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    [AutoBind]
    public class GetListingsQueryHandler : IAsyncQueryHandler<GetListingsQuery, GetListingsResponse>
    {
        private readonly IEditorialRyvussApiProxy _ryvussProxy;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly IMapper _mapper;

        public GetListingsQueryHandler(IEditorialRyvussApiProxy ryvussProxy, ITenantProvider<TenantInfo> tenantProvider, IMapper mapper)
        {
            _ryvussProxy = ryvussProxy;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
        }
        public async Task<GetListingsResponse> HandleAsync(GetListingsQuery query)
        {
            var result = await _ryvussProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInput()
            {
                Query = string.IsNullOrEmpty(query.Query) ? $"Service.{_tenantProvider.Current().Name}." : query.Query,
                Limit = 20,
                Offset = 0,
                SortOrder = "Latest",
                IncludeCount = true,
                IncludeSearchResults = true,
                NavigationName = "RetailNav",
                PostProcessors = new List<string> { "Retail", "FacetSort", "ShowZero" }
            });

            if (!result.IsSucceed)
            {
                return new GetListingsResponse()
                {
                    HttpStatusCode = result.HttpStatusCode
                };
            }

            var listingsViewModel = _mapper.Map<ListingsViewModel>(result.Data);


            return new GetListingsResponse()
            {
                ListingsViewModel = listingsViewModel
            };
        }
    }
}