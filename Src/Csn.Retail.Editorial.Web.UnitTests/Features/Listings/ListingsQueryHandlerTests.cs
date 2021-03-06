﻿using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Listings;
using Csn.Retail.Editorial.Web.Features.Listings.Mappings;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.Mappers;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Features.Shared.Services;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
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
            var routeSettings = Substitute.For<IEditorialRouteSettings>();
            routeSettings.BasePath.Returns("/editorial/");
            routeSettings.ResultsSegment.Returns("results");
            var mapper = Substitute.For<IMapper>();
            var contextStore = Substitute.For<IPageContextStore>();
            var paginationHelper = Substitute.For<IPaginationHelper>();
            var sortingHelper = Substitute.For<ISortingHelper>();
            var expressionParser = Substitute.For<IExpressionParser>();
            var expressionFormatter = Substitute.For<IExpressionFormatter>();
            var polarNativeAdd = Substitute.For<IPolarNativeAdsDataMapper>();
            var listingInsightsDataMapper = Substitute.For<IListingInsightsDataMapper>();
            var seoDataMapper = Substitute.For<ISeoDataMapper>();
            var ryvussDataService = Substitute.For<IRyvussDataService>();

            tenantProvider.Current().Returns(new TenantInfo()
            {
                Name = "carsales"
            });

            // this bit is required because we have some static classes that use service locator pattern
            var dependencyResolver = Substitute.For<IDependencyResolver>();
            dependencyResolver.GetService<ITenantProvider<TenantInfo>>().Returns(tenantProvider);
            dependencyResolver.GetService<IEditorialRouteSettings>().Returns(routeSettings);

            DependencyResolver.SetResolver(dependencyResolver);

            ryvussDataService.GetNavAndResults(Arg.Any<string>(), Arg.Any<bool>()).Returns(Task.FromResult(
                new RyvussNavResultDto
                {
                    INav = new RyvussNavDto()
                }));
            
            mapper.Map<NavResult>(Arg.Any<RyvussNavResultDto>(), Arg.Any<Action<IMappingOperationOptions>>()).Returns(new NavResult { INav = new Nav()});

            var queryHandler = new GetListingsQueryHandler(tenantProvider, mapper, paginationHelper,
                sortingHelper, contextStore, expressionParser, expressionFormatter, polarNativeAdd, listingInsightsDataMapper, seoDataMapper, ryvussDataService);
            var expression = new FacetExpression("Service", "Carsales").And(new KeywordExpression("Keyword", "honda"));
            expressionParser.Parse(Arg.Any<string>()).Returns(expression);
            expressionFormatter.Format(Arg.Any<Expression>()).Returns("Service.CarSales.");

            //Act
            await queryHandler.HandleAsync(new GetListingsQuery {Keywords = "honda"});

            //Assert
            contextStore.Received().Set(Arg.Any<IPageContext>());
        }
    }
}