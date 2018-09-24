using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.DisplayAds;
using Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive;
using Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive.TagBuilders;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.MediaMotiveAds
{
    class MediaMotiveAdQueryHandlerTests
    {
        
        [Test]
        public void MultipleTagsWithDuplicates()
        {
            var tagBuilders = new List<IMediaMotiveTagBuilder>()
            {
                new TestTagBuilder()
            };
            var pageContextStore = Substitute.For<IPageContextStore>();

            var queryHandler = new MediaMotiveAdQueryHandler(tagBuilders, pageContextStore);

            //Act
            var result = queryHandler.Handle(new DisplayAdQuery()
            {
                AdPlacement = DisplayAdPlacements.Aside
            });

            Assert.AreEqual("//mm.carsales.com.au/carsales/jserver/make=honda/model=civic/make=bmw", result.ScriptUrl);
        }

        private class TestTagBuilder : IMediaMotiveTagBuilder
        {
            public IEnumerable<MediaMotiveTag> Build(MediaMotiveTagBuildersParams parameters)
            {
                return new List<MediaMotiveTag>()
                {
                    new MediaMotiveTag(SasAdTags.SasAdTagKeys.Make, "honda"),
                    new MediaMotiveTag(SasAdTags.SasAdTagKeys.Model, "civic"),
                    new MediaMotiveTag(SasAdTags.SasAdTagKeys.Make, "honda"),
                    new MediaMotiveTag(SasAdTags.SasAdTagKeys.Make, "bmw")
                };
            }

            public bool IsApplicable(MediaMotiveTagBuildersParams parameters)
            {
                return true;
            }
        }

        [Test]
        public void TeadsNotShownOnSponsoredArticles()
        {
            var tagBuilders = Substitute.For<List<IMediaMotiveTagBuilder>>();
            var pageContextStore = Substitute.For<IPageContextStore>();
            pageContextStore.Get().Returns(new DetailsPageContext
            {
                ArticleTypes = new List<string> {"Sponsored"}
            });

            var queryHandler = new MediaMotiveAdQueryHandler(tagBuilders, pageContextStore);

            //Act
            var result = queryHandler.Handle(new DisplayAdQuery()
            {
                AdPlacement = DisplayAdPlacements.TEADS
            });

            Assert.AreEqual(null, result);
        }

        [Test]
        public void SupportedAdUnitsShowingForSponsoredArticles()
        {
            var tagBuilders = Substitute.For<List<IMediaMotiveTagBuilder>>();
            var pageContextStore = Substitute.For<IPageContextStore>();

            pageContextStore.Set(new DetailsPageContext
            {
                ArticleTypes = new List<string> { "Sponsored" }
            });

            var queryHandler = new MediaMotiveAdQueryHandler(tagBuilders, pageContextStore);

            //Act
            var result = queryHandler.Handle(new DisplayAdQuery()
            {
                AdPlacement = DisplayAdPlacements.Leaderboard
            });

            Assert.AreEqual("1", result.TileId);
        }
    }
}
