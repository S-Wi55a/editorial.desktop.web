using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Listings;
using Csn.Retail.Editorial.Web.Features.Listings.Mappings;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds.Mappers;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.Mappers;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Features.Shared.Services;
using Expresso.Expressions;
using Expresso.Syntax;
using NSubstitute;
using NUnit.Framework;
using IMapper = Csn.Retail.Editorial.Web.Infrastructure.Mappers.IMapper;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Listings
{
    class ListingsQueryHandlerTests
    {
        [Test]
        public async Task EnsureSearchResultsStoredInContextCache()
        {
            //Arrange
            var tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();
            var mapper = Substitute.For<IMapper>();
            var contextStore = Substitute.For<ISearchResultContextStore>();
            var paginationHelper = Substitute.For<IPaginationHelper>();
            var sortingHelper = Substitute.For<ISortingHelper>();
            var expressionParser = Substitute.For<IExpressionParser>();
            var expressionFormatter = Substitute.For<IExpressionFormatter>();
            var polarNativeAdd = Substitute.For<IPolarNativeAdsDataMapper>();
            var sponsoredLinksDataMapper = Substitute.For<ISponsoredLinksDataMapper>();
            var listingInsightsDataMapper = Substitute.For<IListingInsightsDataMapper>();
            var seoDataMapper = Substitute.For<ISeoDataMapper>();
            var ryvussDataService = Substitute.For<IRyvussDataService>();
            tenantProvider.Current().Returns(new TenantInfo()
            {
                Name = "carsales",
                AdUnits = new List<string> { "Title3" }
            });

            ryvussDataService.GetNavAndResults(Arg.Any<string>(), Arg.Any<bool>()).Returns(Task.FromResult(
                new RyvussNavResultDto
                {
                    INav = new RyvussNavDto()
                }));
            
            mapper.Map<NavResult>(Arg.Any<RyvussNavResultDto>(), Arg.Any<Action<IMappingOperationOptions>>()).Returns(new NavResult { INav = new Nav()});

            var queryHandler = new GetListingsQueryHandler(tenantProvider, mapper, paginationHelper,
                sortingHelper, contextStore, expressionParser, expressionFormatter, polarNativeAdd, sponsoredLinksDataMapper, listingInsightsDataMapper, seoDataMapper, ryvussDataService);
            var expression = new FacetExpression("Service", "Carsales").And(new KeywordExpression("Keyword", "honda"));
            expressionParser.Parse(Arg.Any<string>()).Returns(expression);
            expressionFormatter.Format(Arg.Any<Expression>()).Returns("Service.CarSales.");

            //Act
            await queryHandler.HandleAsync(new GetListingsQuery {Keywords = "honda"});

            //Assert
            contextStore.Received().Set(Arg.Any<SearchContext>());
        }
    }
}