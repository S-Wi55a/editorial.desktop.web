using System.Collections.Generic;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema;
using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using NSubstitute;
using NUnit.Framework;

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
                SeoSchemaSupport = true
            });

            _schemaSettings.ArticleTypesForNewsSchema.Returns("News,Video,Advice,Features,Video,Sponsored,Carpool");
            _schemaSettings.ArticleTypesForReviewSchema.Returns("Review,Reviews");
        }

        [Test]
        public void TestReviewSchemaBuildWithNoReviewItemsInArticle()
        {
            List<EditorialItem> listItems = new List<EditorialItem> { };

            var article = new ArticleDetailsDto
            {
                ArticleType = ArticleType.Review.ToString(),
                Items = listItems
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
            };

            var schemaBuilder = new SchemaDataBuilder(_schemaSettings, _tenantProvider);
            var result = schemaBuilder.BuildNews(article);

            Assert.IsNull(result.Headline);
            Assert.IsNull(result.DatePublished);
            Assert.IsNull(result.DateModified);
            Assert.IsNull(result.ArticleBody);
            Assert.IsNull(result.MainEntityOfPage);
            Assert.IsNull(result.Author);
            Assert.IsNull(result.Image);
        }

        [Test]
        public void TestReviewArticleSchemaMarkupWithNullArticleValues()
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
                Items = listItems
            };

            var schemaBuilder = new SchemaDataBuilder(_schemaSettings, _tenantProvider);
            var result = schemaBuilder.BuildReview(article);

            Assert.IsNull(result.Headline);
            Assert.IsNull(result.DatePublished);
            Assert.IsNull(result.DateModified);
            Assert.IsNull(result.ReviewBody);
            Assert.IsNull(result.MainEntityOfPage);
            Assert.IsNull(result.About);
            Assert.IsNull(result.ReviewRating);
            Assert.IsNull(result.Image);
        }

        [Test]
        public void TestAuthorMarkupWithNoAuthorsInArticle()
        {
            List<Contributor> contributors = new List<Contributor>(){};

            var article = new ArticleDetailsDto
                {
                    ArticleType = ArticleType.News.ToString(),
                    Contributors = contributors
                };

            var schemaDataBuilder = new SchemaDataBuilder(_schemaSettings, _tenantProvider);
            var result = schemaDataBuilder.BuildNews(article);

            Assert.IsNotNull(result.Author);
            Assert.That(result.Author, Is.TypeOf<Publisher>());
        }

        [Test]
        public void TestBodyCopyMarkupWithNoSummaryContent()
        {
            var article = new ArticleDetailsDto
            {
                ArticleType = ArticleType.News.ToString(),
                Headline = "Facelifted Volvo XC90 gets new mild hybrid tech",
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
            var result = schemaDataBuilder.BuildNews(article);

            Assert.IsNotNull(result);
            Assert.AreSame(article.Headline,result.Headline);
        }


        [Test]
        public void TestImageMarkupWithNoImagesInHeroSection()
        {
            List<Image> imageCatalogue = new List<Image>() { };

            var article = new ArticleDetailsDto
            {
                ArticleType = ArticleType.News.ToString(),
                HeroSection = new HeroSection()
                {
                    Images = imageCatalogue
                }
            };

            var schemaDataBuilder = new SchemaDataBuilder(_schemaSettings, _tenantProvider);
            var result = schemaDataBuilder.BuildNews(article);

            Assert.IsNull(result.Image);
        }

        [Test]
        public void TestExpertCategoryRatingWithNoRatingsInArticle()
        {
            var article = new ArticleDetailsDto
            {
                ArticleType = ArticleType.Review.ToString(),
            };

            var schemaDataBuilder = new SchemaDataBuilder(_schemaSettings, _tenantProvider);
            var result = schemaDataBuilder.BuildReview(article);

            Assert.IsNull(result.ReviewRating);
        }
    }
}
