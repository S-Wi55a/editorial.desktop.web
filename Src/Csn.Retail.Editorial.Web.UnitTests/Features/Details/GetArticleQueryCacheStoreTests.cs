using System.Net;
using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Details;
using Csn.Retail.Editorial.Web.Features.Details.CacheStores;
using Csn.Retail.Editorial.Web.Features.Details.Models;
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
            var cacheStore = Substitute.For<IArticleDetailsCacheStore>();

            var articleQueryCacheStore = new GetArticleQueryCacheStore(cacheStore);

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
            cacheStore.DidNotReceive().GetAsync(Arg.Any<string>());
            cacheStore.DidNotReceive().StoreAsync(Arg.Any<GetArticleResponse>());

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Tests that the article is retrieved from cache when the article is already cached and not in preview
        /// </summary>
        [Test]
        public async Task CacheIsUsedWhenNotPreview()
        {
            var cacheStore = Substitute.For<IArticleDetailsCacheStore>();

            cacheStore.GetAsync(Arg.Any<string>())
                .Returns(Task.FromResult(new MayBe<GetArticleResponse>(new GetArticleResponse())));

            var articleQueryCacheStore = new GetArticleQueryCacheStore(cacheStore);

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
            await cacheStore.Received().GetAsync(Arg.Any<string>());

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Tests that a previously uncached article gets cached when not in preview
        /// </summary>
        [Test]
        public async Task ArticleIsCachedWhenNotPreview()
        {
            var cacheStore = Substitute.For<IArticleDetailsCacheStore>();

            // cache returns no result
            cacheStore.GetAsync(Arg.Any<string>())
                .Returns(Task.FromResult(new MayBe<GetArticleResponse>(null, false)));

            var articleQueryCacheStore = new GetArticleQueryCacheStore(cacheStore);

            var result = await articleQueryCacheStore.GetAsync(new GetArticleQuery()
            {
                Id = "ED",
                IsPreview = false
            }, (q) =>
            {
                return Task.FromResult(new GetArticleResponse()
                {
                    ArticleViewModel = new ArticleViewModel(),
                    HttpStatusCode = HttpStatusCode.OK
                });
            });

            // now check that a call to the cache was not made
            await cacheStore.Received().GetAsync(Arg.Any<string>());

            // data retrieved should get cached
            await cacheStore.Received().StoreAsync(Arg.Any<GetArticleResponse>());

            Assert.IsNotNull(result);
        }
    }
}
