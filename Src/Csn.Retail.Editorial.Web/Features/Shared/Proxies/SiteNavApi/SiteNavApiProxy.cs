using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Csn.Hystrix.RestClient;
using Csn.Hystrix.RestClient.Dtos;
using Csn.Retail.Editorial.Web.Features.Shared.GlobalSite;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Features.SiteNav;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.UserContext;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.SiteNavApi
{
    public interface ISiteNavApiProxy
    {
        HystrixRestResponse<SiteNavResponse> GetSiteNav(string site);
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

        public HystrixRestResponse<SiteNavResponse> GetSiteNav(string site)
        {
            try
            {
                var client = _restClient.HostName(HostName).Path($"navigation/{site}")
                    .QueryParams("memberId", _userContext.CurrentUserId);

                var response = client.Get<SiteNavResponse>();
                return response;

            }
            catch (Exception exc)
            {
                return null;
            }
        }
    }
}