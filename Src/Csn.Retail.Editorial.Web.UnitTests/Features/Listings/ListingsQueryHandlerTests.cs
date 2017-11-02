using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Listings;
using Csn.Retail.Editorial.Web.Features.Listings.Mappings;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds.Mappers;
using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Expresso.Expressions;
using Expresso.Syntax;
using Ingress.ServiceClient.Abstracts;
using NSubstitute;
using NUnit.Framework;
using IContextStore = Ingress.ContextStores.IContextStore;
using IMapper = Csn.Retail.Editorial.Web.Infrastructure.Mappers.IMapper;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Listings
{
    class ListingsQueryHandlerTests
    {
        [Test]
        public async Task EnsureSearchResultsStoredInContextCache()
        {
            //Arrange
            var ryvussProxy = Substitute.For<IEditorialRyvussApiProxy>();
            var tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();
            var mapper = Substitute.For<IMapper>();
            var contextStore = Substitute.For<IContextStore>();
            var paginationHelper = Substitute.For<IPaginationHelper>();
            var sortingHelper = Substitute.For<ISortingHelper>();
            var expressionParser = Substitute.For<IExpressionParser>();
            var expressionFormatter = Substitute.For<IExpressionFormatter>();
            var polarNativeAdd = Substitute.For<IPolarNativeAdsDataMapper>();
            var sponsoredLinksDataMapper = Substitute.For<ISponsoredLinksDataMapper>();
            tenantProvider.Current().Returns(new TenantInfo()
            {
                Name = "carsales",
                AdUnits = new List<string> { "Title3" }
            });

            ryvussProxy.GetAsync<RyvussNavResultDto>(Arg.Any<EditorialRyvussInput>()).Returns(Task.FromResult(
                new SmartServiceResponse<RyvussNavResultDto>()
                {
                    Data = new RyvussNavResultDto()
                    {
                        INav = new RyvussNavDto()
                    },
                    HttpStatusCode = HttpStatusCode.OK
                }));
            mapper.Map<NavResult>(Arg.Any<RyvussNavResultDto>(), Arg.Any<Action<IMappingOperationOptions>>()).Returns(new NavResult());

            var queryHandler = new GetListingsQueryHandler(ryvussProxy, tenantProvider, mapper, paginationHelper,
                sortingHelper, contextStore, expressionParser, expressionFormatter, polarNativeAdd, sponsoredLinksDataMapper);
            var expression = new FacetExpression("Service", "Carsales").And(new KeywordExpression("Keyword", "honda"));
            expressionParser.Parse(Arg.Any<string>()).Returns(expression);
            expressionFormatter.Format(Arg.Any<Expression>()).Returns("Service.CarSales.");
            

            //Act
            await queryHandler.HandleAsync(new GetListingsQuery {Keyword = "honda"});

            //Assert
            contextStore.Received().Set(Arg.Any<string>(), Arg.Any<object>());
        }
    }
}