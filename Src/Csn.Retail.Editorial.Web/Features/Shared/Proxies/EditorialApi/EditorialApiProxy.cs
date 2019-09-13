using System.Threading.Tasks;
using Bolt.Common.Extensions;
using Csn.Logging;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Redirects;
using Ingress.ServiceClient.Abstracts;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi
{
    public interface IEditorialApiProxy
    {
        Task<SmartServiceResponse<ArticleDetailsDto>> GetArticleAsync(EditorialApiInput input);
        Task<SmartServiceResponse<ArticleDetailsDto>> GetPreviewAsync(EditorialApiInput input);
        Task<SmartServiceResponse<object>> GetAsync(string uri);
    }

    [AutoBind]
    public class EditorialApiProxy : IEditorialApiProxy
    {
        private const string ServiceName = "api-retail-editorial";
        private readonly ISmartServiceClient _smartClient;
        private readonly ILogger _logger;
        private readonly IRequestContextWrapper _requestContext;

        public EditorialApiProxy(ISmartServiceClient smartClient, ILogger logger, IRequestContextWrapper requestContext)
        {
            _smartClient = smartClient;
            _logger = logger;
            _requestContext = requestContext;
        }

        public Task<SmartServiceResponse<ArticleDetailsDto>> GetArticleAsync(EditorialApiInput input)
        {
            _logger.Trace($"GetArticleAsync(): {_requestContext.Url.ToString()}");

            return _smartClient.Service(ServiceName)
                        .Path("v1/details/{0}/desktop/{1}/?isPreview={2}".FormatWith(input.ServiceName, input.Id, input.IsPreview))
                        .GetAsync<ArticleDetailsDto>();
        }

        public Task<SmartServiceResponse<ArticleDetailsDto>> GetPreviewAsync(EditorialApiInput input)
        {
            _logger.Trace($"GetPreviewAsync(): {_requestContext.Url.ToString()}");

            return _smartClient.Service(ServiceName)
                .Path("v1/preview/{0}/desktop/{1}".FormatWith(input.ServiceName, input.Id))
                .GetAsync<ArticleDetailsDto>();
        }

        public Task<SmartServiceResponse<object>> GetAsync(string uri)
        {
            _logger.Trace($"GetAsync(): {_requestContext.Url.ToString()}");

            return _smartClient.Service(ServiceName)
                .Path(uri)
                .GetAsync<object>();
        }
    }
}