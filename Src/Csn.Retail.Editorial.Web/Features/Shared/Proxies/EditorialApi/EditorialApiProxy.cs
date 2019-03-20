using System.Threading.Tasks;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Ingress.ServiceClient.Abstracts;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi
{
    public interface IEditorialApiProxy
    {
        Task<SmartServiceResponse<ArticleDetailsDto>> GetArticleAsync(EditorialApiInput input);
        Task<SmartServiceResponse<object>> GetAsync(string uri);
    }

    [AutoBind]
    public class EditorialApiProxy : IEditorialApiProxy
    {
        private const string ServiceName = "api-retail-editorial";
        private readonly ISmartServiceClient _smartClient;

        public EditorialApiProxy(ISmartServiceClient smartClient)
        {
            _smartClient = smartClient;
        }

        public Task<SmartServiceResponse<ArticleDetailsDto>> GetArticleAsync(EditorialApiInput input)
        {
            return _smartClient.Service(ServiceName)
                        .Path("v1/details/{0}/desktop/{1}/?isPreview={2}".FormatWith(input.ServiceName, input.Id, input.IsPreview))
                        .GetAsync<ArticleDetailsDto>();
        }

        public Task<SmartServiceResponse<object>> GetAsync(string uri)
        {
            return _smartClient.Service(ServiceName)
                .Path(uri)
                .GetAsync<object>();
        }
    }
}