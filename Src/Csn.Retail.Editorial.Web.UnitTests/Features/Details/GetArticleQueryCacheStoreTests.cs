using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Details;
using Csn.Retail.Editorial.Web.Features.Details.CacheStores;
using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Ingress.Cache;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Details
{
    [TestFixture]
    class GetArticleQueryCacheStoreTests
    {
        /// <summary>
        /// Tests that cache is always bypassed when preview
        /// </summary>
        [Test]
        public void CacheIsBypassedForPreview()
        {
            var cacheStore = Substitute.For<ICacheStore>();
            var tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();

            var articleQueryCacheStore = new GetArticleQueryCacheStore(cacheStore, tenantProvider);

            var result = articleQueryCacheStore.GetAsync(new GetArticleQuery()
            {
                Id = "TEST",
                IsPreview = true
            }, (q) =>
            {
                return Task.FromResult(new GetArticleResponse()
                {
                    ArticleViewModel = null
                });
            });

            // now check that a call to the cache was not made
            cacheStore.DidNotReceive().GetAsync<ArticleViewModel>(Arg.Any<string>());
            cacheStore.DidNotReceive().SetAsync(Arg.Any<string>(), Arg.Any<ArticleViewModel>(), Arg.Any<CacheExpiredIn>());

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Tests that the article is retrieved from cache when the article is already cached and not in preview
        /// </summary>
        [Test]
        public async Task CacheIsUsedWhenNotPreview()
        {
            var cacheStore = Substitute.For<ICacheStore>();
            var tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();

            tenantProvider.Current().ReturnsForAnyArgs(new TenantInfo()
            {
                Name = "carsales"
            });

            cacheStore.GetAsync<ArticleViewModel>(Arg.Any<string>())
                .Returns(Task.FromResult(new MayBe<ArticleViewModel>(new ArticleViewModel())));

            var articleQueryCacheStore = new GetArticleQueryCacheStore(cacheStore, tenantProvider);

            var result = await articleQueryCacheStore.GetAsync(new GetArticleQuery()
            {
                Id = "ED",
                IsPreview = false
            }, (q) =>
            {
                return Task.FromResult(new GetArticleResponse()
                {
                    ArticleViewModel = new ArticleViewModel()
                });
            });

            // now check that a call to the cache was not made
            cacheStore.Received().GetAsync<ArticleViewModel>(Arg.Any<string>());

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Tests that a previously uncached article gets cached when not in preview
        /// </summary>
        [Test]
        public async Task ArticleIsCachedWhenNotPreview()
        {
            var cacheStore = Substitute.For<ICacheStore>();
            var tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();

            tenantProvider.Current().ReturnsForAnyArgs(new TenantInfo()
            {
                Name = "carsales"
            });

            // cache returns no result
            cacheStore.GetAsync<ArticleViewModel>(Arg.Any<string>())
                .Returns(Task.FromResult(new MayBe<ArticleViewModel>(null, false)));

            var articleQueryCacheStore = new GetArticleQueryCacheStore(cacheStore, tenantProvider);

            var result = await articleQueryCacheStore.GetAsync(new GetArticleQuery()
            {
                Id = "ED",
                IsPreview = false
            }, (q) =>
            {
                return Task.FromResult(new GetArticleResponse()
                {
                    ArticleViewModel = new ArticleViewModel()
                });
            });

            // now check that a call to the cache was not made
            cacheStore.Received().GetAsync<ArticleViewModel>(Arg.Any<string>());

            // data retrieved should get cached
            cacheStore.Received().SetAsync(Arg.Any<string>(), Arg.Any<ArticleViewModel>(), Arg.Any<CacheExpiredIn>());

            Assert.IsNotNull(result);
        }
    }
}
