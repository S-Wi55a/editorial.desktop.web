using System.Net;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Details;
using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Ingress.ServiceClient.Abstracts;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Details
{
    [TestFixture]
    class GetArticleQueryHandlerTests
    {
        private IMapper _mapper;
        private IEditorialApiProxy _apiProxy;
        private ITenantProvider<TenantInfo> _tenantProvider;
        private IPageContextStore _pageContextStore;

        [SetUp]
        public void SetUp()
        {
            _tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();
            _mapper = Substitute.For<IMapper>();
            _pageContextStore = Substitute.For<IPageContextStore>();

            _apiProxy = Substitute.For<IEditorialApiProxy>();
            _apiProxy.GetArticleAsync(Arg.Any<EditorialApiInput>()).ReturnsForAnyArgs(Task.FromResult(new SmartServiceResponse<ArticleDetailsDto>()
            {
                IsCircuitFailed = false,
                HttpStatusCode = HttpStatusCode.OK,
                Data = new ArticleDetailsDto()
                {
                    SeoData = new Web.Features.Shared.Proxies.EditorialApi.SeoData()
                }
            }));
        }

        /// <summary>
        /// When looking at an article with preview enabled, we should always disable seo indexing of the article 
        /// </summary>
        [Test]
        public async Task SeoIndexingDisabledWhenPreview()
        {
            _tenantProvider.Current().ReturnsForAnyArgs(new TenantInfo()
            {
                Name = "carsales",
                AllowSeoIndexOfDetails = true
            });

            _mapper.Map<ArticleViewModel>(Arg.Any<ArticleDetailsDto>()).Returns(new ArticleViewModel() { SeoData = new Web.Features.Shared.Models.SeoData(){AllowSeoIndexing = true} });

            var queryHandler = new GetArticleQueryHandler(_apiProxy, _mapper, _tenantProvider);

            var result = await queryHandler.HandleAsync(new GetArticleQuery()
            {
                Id = "ED-ITM-001",
                IsPreview = true
            });

            Assert.IsFalse(result.ArticleViewModel.SeoData.AllowSeoIndexing);
        }

        /// <summary>
        /// When looking at an article with preview disabled, seo indexing should be enabled if enabled for the tenant  
        /// </summary>
        [Test]
        public async Task SeoIndexingEnabled_NotPreview()
        {
            _tenantProvider.Current().ReturnsForAnyArgs(new TenantInfo()
            {
                Name = "carsales",
                AllowSeoIndexOfDetails = true
            });

            _mapper.Map<ArticleViewModel>(Arg.Any<ArticleDetailsDto>()).Returns(new ArticleViewModel() { SeoData = new Web.Features.Shared.Models.SeoData() { AllowSeoIndexing = true } });

            var queryHandler = new GetArticleQueryHandler(_apiProxy, _mapper, _tenantProvider);

            var result = await queryHandler.HandleAsync(new GetArticleQuery()
            {
                Id = "ED-ITM-001",
                IsPreview = false
            });

            Assert.IsTrue(result.ArticleViewModel.SeoData.AllowSeoIndexing);
        }

        /// <summary>
        /// When looking at an article with preview disabled, seo indexing should be disabled if disabled for the tenant  
        /// </summary>
        [Test]
        public async Task SeoIndexingDisabled_NotPreview()
        {
            _tenantProvider.Current().ReturnsForAnyArgs(new TenantInfo()
            {
                Name = "carsales",
                AllowSeoIndexOfDetails = false
            });

            _mapper.Map<ArticleViewModel>(Arg.Any<ArticleDetailsDto>()).Returns(new ArticleViewModel() { SeoData = new Web.Features.Shared.Models.SeoData() { AllowSeoIndexing = false } });

            var queryHandler = new GetArticleQueryHandler(_apiProxy, _mapper, _tenantProvider);

            var result = await queryHandler.HandleAsync(new GetArticleQuery()
            {
                Id = "ED-ITM-001",
                IsPreview = false
            });

            Assert.IsFalse(result.ArticleViewModel.SeoData.AllowSeoIndexing);
        }
    }
}
