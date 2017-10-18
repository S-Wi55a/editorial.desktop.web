using System.Net;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Listings;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Ingress.ServiceClient.Abstracts;
using NSubstitute;
using NUnit.Framework;
using IContextStore = Ingress.ContextStores.IContextStore;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Listings
{
    class ListingsQueryHandlerTests
    {
        [Test]
        public async Task EnsureSearchResultsStoredInContextCache()
        {
            var ryvussProxy = Substitute.For<IEditorialRyvussApiProxy>();
            var tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();
            var mapper = Substitute.For<IMapper>();
            var contextStore = Substitute.For<IContextStore>();

            tenantProvider.Current().Returns(new TenantInfo()
            {
                Name = "carsales"
            });

            ryvussProxy.GetAsync<RyvussNavResultDto>(Arg.Any<EditorialRyvussInput>()).Returns(Task.FromResult(new SmartServiceResponse<RyvussNavResultDto>()
            {
                Data = new RyvussNavResultDto()
                {
                    INav = new RyvussNavDto()
                },
                HttpStatusCode = HttpStatusCode.OK
            }));

            var queryHandler = new GetListingsQueryHandler(ryvussProxy, tenantProvider, mapper, contextStore);

            await queryHandler.HandleAsync(new GetListingsQuery());

            contextStore.Received().Set(Arg.Any<string>(), Arg.Any<object>());
        }
    }
}
