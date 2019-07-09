using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema;
using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using NSubstitute;
using NUnit.Framework;
using SeoData = Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi.SeoData;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Shared.SeoSchema
{
    [TestFixture]
    class SchemaDataBuilderTests
    {
        private ISeoSchemaSettings _schemaSettings;
        private ITenantProvider<TenantInfo> _tenantProvider;

        [SetUp]
        public void SetUp()
        {
            _schemaSettings = Substitute.For<ISeoSchemaSettings>();
            _tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();

            _tenantProvider.Current().Returns(new TenantInfo()
            {
                Name = "carsales",
                SiteDomain = "carsales.com.au",
                Culture = new CultureInfo("en-au")
            });

            var dependencyResolver = Substitute.For<IDependencyResolver>();
            dependencyResolver.GetService<ITenantProvider<TenantInfo>>().Returns(_tenantProvider);
            DependencyResolver.SetResolver(dependencyResolver);

            _schemaSettings.ArticleTypesForReviewSchema.Returns("Review,Reviews");
        }

        [Test]
        public void TestReviewSchemaBuildWithNoReviewItemsInArticle()
        {
            List<EditorialItem> listItems = new List<EditorialItem> { };

            var article = new ArticleDetailsDto
            {
                ArticleType = ArticleType.Review.ToString(),
                Items = listItems,
                SeoData = new SeoData() { ImageUrls = new List<string>(){ }}
            };

            var schemaBuilder = new SchemaDataBuilder(_schemaSettings, _tenantProvider);
            var result = schemaBuilder.Build(article);

            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NewsArticleSchema>());
        }

        [Test]
        public void TestSchemaBuildWithUnknownArticleType()
        {
            var article = new ArticleDetailsDto
            {
                ArticleType = "unknown-article-type",
                SeoData = new SeoData() { }
            };

            var schemaBuilder = new SchemaDataBuilder(_schemaSettings, _tenantProvider);
            var result = schemaBuilder.Build(article);

            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<NewsArticleSchema>());
        }


        [Test]
        public void TestNewsArticleSchemaMarkupWithNullArticleValues()
        {
            var article = new ArticleDetailsDto
            {
                ArticleType = ArticleType.News.ToString(),
                SeoData = new SeoData() { }
            };

            var schemaBuilder = new SchemaDataBuilder(_schemaSettings, _tenantProvider);
            var result = schemaBuilder.Build(article);
            var newsArticleSchema = (NewsArticleSchema)result;

            Assert.IsNotNull(newsArticleSchema);
            Assert.IsNull(newsArticleSchema.Headline);
            Assert.IsNull(newsArticleSchema.DatePublished);
            Assert.IsNull(newsArticleSchema.DateModified);
            Assert.IsNull(newsArticleSchema.ArticleBody);
            Assert.IsNull(newsArticleSchema.MainEntityOfPage);
            Assert.IsNull(newsArticleSchema.Image);
        }

        [Test]
        public void TestReviewArticleSchemaMarkupWithNoValues()
        {
            List<EditorialItem> listItems = new List<EditorialItem> { new EditorialItem()
            {
                Make = "Mazda",
                Model = "CX-5",
                Year = 2019
            }};

            var article = new ArticleDetailsDto
            {
                ArticleType = ArticleType.Review.ToString(),
                Items = listItems,
                SeoData = new SeoData() { }
            };

            var schemaBuilder = new SchemaDataBuilder(_schemaSettings, _tenantProvider);
            var result = schemaBuilder.Build(article);
            var reviewArticleSchema = (ReviewArticleSchema) result;

            Assert.AreEqual("Review", result.Type);
            Assert.IsNotNull(reviewArticleSchema);
            Assert.IsNull(reviewArticleSchema.Headline);
            Assert.IsNull(reviewArticleSchema.DatePublished);
            Assert.IsNull(reviewArticleSchema.DateModified);
            Assert.IsNull(reviewArticleSchema.ReviewBody);
            Assert.IsNull(reviewArticleSchema.MainEntityOfPage);
            Assert.IsNull(reviewArticleSchema.About);
            Assert.IsNull(reviewArticleSchema.ReviewRating);
            Assert.IsNull(reviewArticleSchema.Image);
        }

        [Test]
        public void TestAuthorMarkupWithNoAuthorsInArticle()
        {
            List<Contributor> contributors = new List<Contributor>() { };

            var article = new ArticleDetailsDto
            {
                ArticleType = ArticleType.News.ToString(),
                Contributors = contributors,
                SeoData = new SeoData() { }
            };

            var schemaDataBuilder = new SchemaDataBuilder(_schemaSettings, _tenantProvider);
            var result = schemaDataBuilder.Build(article);
            var newsArticleSchema = (NewsArticleSchema) result;

            Assert.IsNotNull(newsArticleSchema.Author);
            Assert.That(newsArticleSchema.Author, Is.TypeOf<Publisher>());
        }


        [Test]
        public void TestBodyCopyMarkupWithNoSummaryContent()
        {
            var article = new ArticleDetailsDto
            {
                ArticleType = ArticleType.News.ToString(),
                Headline = "Facelifted Volvo XC90 gets new mild hybrid tech",
                SeoData = new SeoData() { },
                ContentSections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        SectionType = ContentSectionType.Html,
                        Content = "<h2>Backstage pass to the biggest one-make automotive party of the year and the smorgasbord of stars on this red carpet will have you salivating. Alongside me holding up the score cards are senior motoring.com.au scribes Mike Sinclair, Marton Pettendy and Andrea Matthews, providing a good cross-section of experience and input. Truth be known, they're also there to make sure I don't spend two days just lapping in the M4 GTS</h2>"
                    },
                    new ContentSection
                    {
                        SectionType = ContentSectionType.Html,
                        Content = "<h1>The automotive party</h1>"
                    }
                }
            };

            var schemaDataBuilder = new SchemaDataBuilder(_schemaSettings, _tenantProvider);
            var result = schemaDataBuilder.Build(article);
            var newsArticleSchema = (NewsArticleSchema) result;

            Assert.IsNotNull(result);
            Assert.AreSame(article.Headline, newsArticleSchema.ArticleBody);
        }

        [Test]
        public void TestExpertCategoryRatingWithNoValues()
        {
            List<EditorialItem> listItems = new List<EditorialItem> { new EditorialItem()
            {
                Make = "Mazda",
                Model = "CX-5",
                Year = 2019
            }};

            var article = new ArticleDetailsDto
            {
                ArticleType = ArticleType.Review.ToString(),
                Items = listItems,
                SeoData = new SeoData() { }
            };

            var schemaDataBuilder = new SchemaDataBuilder(_schemaSettings, _tenantProvider);
            var result = schemaDataBuilder.Build(article);
            var reviewArticleSchema = (ReviewArticleSchema)result;

            Assert.IsNull(reviewArticleSchema.ReviewRating);
        }
    }
}
