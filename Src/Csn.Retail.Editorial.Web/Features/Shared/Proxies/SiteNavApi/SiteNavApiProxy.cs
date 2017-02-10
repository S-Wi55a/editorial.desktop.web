using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Csn.Hystrix.RestClient;
using Csn.Hystrix.RestClient.Dtos;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Features.SiteNav;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.UserContext;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.SiteNavApi
{
    public interface ISiteNavApiProxy
    {
        Task<HystrixRestResponse<SiteNavResponse>> GetSiteNavAsync(string site);
    }

    [AutoBind]
    public class SiteNavApiProxy : ISiteNavApiProxy
    {
        private const string HostName = "SiteNavApiProxy";

        private readonly IUserContext _userContext;
        private readonly IFluentHystrixRestClientFactory _restClient;


        public SiteNavApiProxy(IFluentHystrixRestClientFactory restClient, IUserContext userContext)
        {
            _restClient = restClient;
            _userContext = userContext;
        }

        public async Task<HystrixRestResponse<SiteNavResponse>> GetSiteNavAsync(string site)
        {
            var response = await _restClient.HostName(HostName)
                        .Path("navigation/{0}/", site)
                        .QueryParams("memberId", _userContext.CurrentUserId)
                        .GetAsync<SiteNavResponse>();

            return response;
        }
    }
}