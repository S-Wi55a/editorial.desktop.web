using System.Threading.Tasks;
using Csn.Hystrix.RestClient;
using Csn.Hystrix.RestClient.Dtos;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi
{
    public interface IEditorialApiProxy
    {
        Task<HystrixRestResponse<T>> GetAsync<T>(EditorialApiInput input);
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

        public Task<HystrixRestResponse<T>> GetAsync<T>(EditorialApiInput input)
        {
            return _restClient.HostName(HostName)
                                    .Path("v1/details/{0}/{1}/{2}/", input.ServiceName, input.ViewType, input.Id)
                                    .GetAsync<T>();
        }
    }

    public class EditorialApiInput
    {
        public string ServiceName { get; set; }
        public string ViewType { get; set; }
        public string Id { get; set; }
    }
}