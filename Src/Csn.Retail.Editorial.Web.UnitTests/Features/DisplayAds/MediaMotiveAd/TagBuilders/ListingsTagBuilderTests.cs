﻿using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive.TagBuilders;
using Csn.Retail.Editorial.Web.Features.Shared.Constants;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.MediaMotiveAds.TagBuilders
{
    class ListingsTagBuilderTests
    {
        [Test]
        public void NoBreadCrumbs()
        {
            var contextStore = Substitute.For<IPageContextStore>();

            contextStore.Get().Returns(new ListingPageContext()
            {
                RyvussNavResult = new RyvussNavResultDto()
            });

            var breadCrumbTagBuilder = Substitute.For<IListingsBreadCrumbTagBuilder>();

            breadCrumbTagBuilder.BuildTags(Arg.Any<RyvussNavResultDto>()).Returns(new List<MediaMotiveTag>());

            var tagBuilder = new ListingsTagBuilder(contextStore, breadCrumbTagBuilder);

            var result = tagBuilder.Build(new MediaMotiveTagBuildersParams()
            {
                TileId = 3
            }).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(MediaMotiveAreaNames.EditorialResultsPage, result.First(t => t.Name == SasAdTags.SasAdTagKeys.Area).Values.First());
        }

        [Test]
        public void MakeButNoModelBreadCrumbs()
        {
            var contextStore = Substitute.For<IPageContextStore>();

            contextStore.Get().Returns(new ListingPageContext()
            {
                RyvussNavResult = new RyvussNavResultDto()
            });

            var breadCrumbTagBuilder = Substitute.For<IListingsBreadCrumbTagBuilder>();

            breadCrumbTagBuilder.BuildTags(Arg.Any<RyvussNavResultDto>()).Returns(new List<MediaMotiveTag>()
            {
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.Make, "honda")
            });

            var tagBuilder = new ListingsTagBuilder(contextStore, breadCrumbTagBuilder);

            var result = tagBuilder.Build(new MediaMotiveTagBuildersParams()
            {
                TileId = 3
            }).ToList();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(MediaMotiveAreaNames.EditorialResultsPage, result.First(t => t.Name == SasAdTags.SasAdTagKeys.Area).Values.First());
            Assert.AreEqual("honda", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Make).Values.First());
            Assert.AreEqual("honda", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Car).Values.First());
        }

        [Test]
        public void MakeAndModelBreadCrumbs()
        {
            var contextStore = Substitute.For<IPageContextStore>();

            contextStore.Get().Returns(new ListingPageContext()
            {
                RyvussNavResult = new RyvussNavResultDto()
            });

            var breadCrumbTagBuilder = Substitute.For<IListingsBreadCrumbTagBuilder>();

            breadCrumbTagBuilder.BuildTags(Arg.Any<RyvussNavResultDto>()).Returns(new List<MediaMotiveTag>()
            {
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.Make, "honda"),
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.Model, "civic")
            });

            var tagBuilder = new ListingsTagBuilder(contextStore, breadCrumbTagBuilder);

            var result = tagBuilder.Build(new MediaMotiveTagBuildersParams()
            {
                TileId = 3
            }).ToList();

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(MediaMotiveAreaNames.EditorialResultsPage, result.First(t => t.Name == SasAdTags.SasAdTagKeys.Area).Values.First());
            Assert.AreEqual("honda", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Make).Values.First());
            Assert.AreEqual("civic", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Model).Values.First());
            Assert.AreEqual("hondacivic", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Car).Values.First());
        }


        [Test]
        public void MakeNoModelButlMarketingGroupBreadCrumbs()
        {
            var contextStore = Substitute.For<IPageContextStore>();

            contextStore.Get().Returns(new ListingPageContext()
            {
                RyvussNavResult = new RyvussNavResultDto()
            });

            var breadCrumbTagBuilder = Substitute.For<IListingsBreadCrumbTagBuilder>();

            breadCrumbTagBuilder.BuildTags(Arg.Any<RyvussNavResultDto>()).Returns(new List<MediaMotiveTag>()
            {
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.MarketingGroup, "vclass"),
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.Make, "mercedez")
            });

            var tagBuilder = new ListingsTagBuilder(contextStore, breadCrumbTagBuilder);

            var result = tagBuilder.Build(new MediaMotiveTagBuildersParams()
            {
                TileId = 3
            }).ToList();

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(MediaMotiveAreaNames.EditorialResultsPage, result.First(t => t.Name == SasAdTags.SasAdTagKeys.Area).Values.First());
            Assert.AreEqual("mercedez", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Make).Values.First());
            Assert.AreEqual("mercedezvclass", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Car).Values.First());
        }

    }
}
