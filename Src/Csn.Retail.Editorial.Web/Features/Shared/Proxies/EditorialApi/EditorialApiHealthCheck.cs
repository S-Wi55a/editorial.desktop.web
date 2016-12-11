using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.HealthChecks;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi
{
    [AutoBind]
    public class EditorialApiHealthChecker : IHealthChecker
    {
        private readonly IEditorialApiProxy _proxy;

        public EditorialApiHealthChecker(IEditorialApiProxy proxy)
        {
            _proxy = proxy;
        }

        public async Task<IHealthCheckResult> CheckAsync()
        {
            var result = await _proxy.GetArticleAsync(new EditorialApiInput
            {
                ServiceName = "carsales",
                ViewType = "desktop",
                Id = "ed-itm-51707"
            });

            return new HealthCheckResult
            {
                IsHealthy = (result.Succeed || result.StatusCode == HttpStatusCode.NotFound),
                FailureSeverity = FailureSeverity.Fatal,
                Name = "EditorialApi",
                Details = result.Result
            };
        }
    }
}