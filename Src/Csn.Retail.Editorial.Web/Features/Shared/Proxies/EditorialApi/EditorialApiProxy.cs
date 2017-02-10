using System.Threading.Tasks;
using Csn.Hystrix.RestClient;
using Csn.Hystrix.RestClient.Dtos;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi
{
    public interface IEditorialApiProxy
    {
        Task<HystrixRestResponse<ArticleDetailsDto>> GetArticleAsync(EditorialApiInput input);
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

        public Task<HystrixRestResponse<ArticleDetailsDto>> GetArticleAsync(EditorialApiInput input)
        {
            var response = _restClient.HostName(HostName)
                                    .Path("v1/details/{0}/{1}/{2}/", input.ServiceName, input.ViewType, input.Id)
                                    .GetAsync<ArticleDetailsDto>();
            return response;
        }
    }
}