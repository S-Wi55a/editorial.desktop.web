using System.Threading.Tasks;
using Csn.Hystrix.RestClient;
using Csn.Hystrix.RestClient.Dtos;
using Csn.Retail.Editorial.Web.Features.MoreArticles;
using Csn.Retail.Editorial.Web.Features.Spec;
using Csn.Retail.Editorial.Web.Features.StockForSale;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi
{
    public interface IEditorialApiProxy
    {
        Task<HystrixRestResponse<ArticleDetailsDto>> GetArticleAsync(EditorialApiInput input);
        Task<HystrixRestResponse<MoreArticlesDto>> GetLatestArticlesAsync(MoreArticlesQuery query);
        Task<HystrixRestResponse<StockForSaleDto>> GetStockListingAsync(StockForSaleQuery query);
        Task<HystrixRestResponse<SpecDto>> GetSpecAsync(SpecQuery query);

    }

    [AutoBind]
    public class EditorialApiProxy : IEditorialApiProxy
    {
        private const string HostName = "EditorialApiProxy";
        private readonly IFluentHystrixRestClientFactory _restClient;

        public EditorialApiProxy(IFluentHystrixRestClientFactory restClient)
        {
            _restClient = restClient;
        }

        public async Task<HystrixRestResponse<ArticleDetailsDto>> GetArticleAsync(EditorialApiInput input)
        {
            var response = await _restClient.HostName(HostName)
                                    .Path("v1/details/{0}/{1}/{2}/", input.ServiceName, input.ViewType, input.Id)
                                    .GetAsync<ArticleDetailsDto>();
            return response;
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
    }
}