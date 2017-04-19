using System.Threading.Tasks;
using Bolt.Common.Extensions;
using Csn.Hystrix.RestClient;
using Csn.Hystrix.RestClient.Dtos;
using Csn.Retail.Editorial.Web.Features.AlsoConsider;
using Csn.Retail.Editorial.Web.Features.MoreArticles;
using Csn.Retail.Editorial.Web.Features.Spec;
using Csn.Retail.Editorial.Web.Features.StockForSale;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Ingress.ServiceClient.Abstracts;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi
{
    public interface IEditorialApiProxy
    {
        Task<SmartServiceResponse<ArticleDetailsDto>> GetArticleAsync(EditorialApiInput input);
        Task<HystrixRestResponse<MoreArticlesDto>> GetLatestArticlesAsync(MoreArticlesQuery query);
        Task<HystrixRestResponse<StockForSaleDto>> GetStockListingAsync(StockForSaleQuery query);
        Task<HystrixRestResponse<SpecDto>> GetSpecAsync(SpecQuery query);
        Task<HystrixRestResponse<AlsoConsiderDto>> GetAlsoConsiderAsync(AlsoConsiderQuery query);


    }

    [AutoBind]
    public class EditorialApiProxy : IEditorialApiProxy
    {
        private const string HostName = "EditorialApiProxy";
        private const string ServiceName = "api-retail-editorial";
        private readonly IFluentHystrixRestClientFactory _restClient;
        private readonly ISmartServiceClient _smartClient;

        public EditorialApiProxy(IFluentHystrixRestClientFactory restClient, ISmartServiceClient smartClient)
        {
            _restClient = restClient;
            _smartClient = smartClient;
        }

        public Task<SmartServiceResponse<ArticleDetailsDto>> GetArticleAsync(EditorialApiInput input)
        {
            return _smartClient.Service(ServiceName)
                        .Path("v1/details/{0}/{1}/{2}/".FormatWith(input.ServiceName, input.ViewType, input.Id))
                        .GetAsync<ArticleDetailsDto>();
        }

        public async Task<HystrixRestResponse<MoreArticlesDto>> GetLatestArticlesAsync(MoreArticlesQuery query)
        {
            var response = await _restClient.HostName(HostName)
                .Path(query.Uri)
                .GetAsync<MoreArticlesDto>();

            return response;
        }

        public async Task<HystrixRestResponse<StockForSaleDto>> GetStockListingAsync(StockForSaleQuery query)
        {
            var response = await _restClient.HostName(HostName)
                .Path(query.Uri)
                .GetAsync<StockForSaleDto>();

            return response;
        }

        public async Task<HystrixRestResponse<SpecDto>> GetSpecAsync(SpecQuery query)
        {
            var response = await _restClient.HostName(HostName)
                .Path(query.Uri)
                .GetAsync<SpecDto>();

            return response;
        }
        public async Task<HystrixRestResponse<AlsoConsiderDto>> GetAlsoConsiderAsync(AlsoConsiderQuery query)
        {
            var response = await _restClient.HostName(HostName)
                .Path(query.Uri)
                .GetAsync<AlsoConsiderDto>();

            return response;
        }
    }
}