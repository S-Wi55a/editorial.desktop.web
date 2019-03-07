using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema;
using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Models;
using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Shared;
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
                SeoSchemaSupport = true,
                Culture = new CultureInfo("en-au")
            });

            var dependencyResolver = Substitute.For<IDependencyResolver>();
            dependencyResolver.GetService<ITenantProvider<TenantInfo>>().Returns(_tenantProvider);
            DependencyResolver.SetResolver(dependencyResolver);

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
                ArticleType = ArticleType.News.ToString()
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
                Items = listItems
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
                Contributors = contributors
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
            var result = schemaDataBuilder.Build(article);
            var newsArticleSchema = (NewsArticleSchema)result;

            Assert.IsNull(newsArticleSchema.Image);
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
                Items = listItems
            };

            var schemaDataBuilder = new SchemaDataBuilder(_schemaSettings, _tenantProvider);
            var result = schemaDataBuilder.Build(article);
            var reviewArticleSchema = (ReviewArticleSchema)result;

            Assert.IsNull(reviewArticleSchema.ReviewRating);
        }

        [Test]
        public void TestExpertCategoryRatingWithOnlyOverallValues()
        {
            List<EditorialItem> listItems = new List<EditorialItem> { new EditorialItem()
            {
                Make = "Mazda",
                Model = "CX-5",
                Year = 2019
            }};

            EditorialExpertRating expertRating = new EditorialExpertRating()
            {
                Heading = "Overall Rating",
                SubHeading = "",
                OverallRating = 80,
                Items = new List<EditorialExpertRating.ExpertItem>() { }
            };

            var article = new ArticleDetailsDto
            {
                ArticleType = ArticleType.Review.ToString(),
                Items = listItems,
                ExpertRatings = expertRating
            };

            var schemaDataBuilder = new SchemaDataBuilder(_schemaSettings, _tenantProvider);
            var result = schemaDataBuilder.Build(article);
            var reviewArticleSchema = (ReviewArticleSchema)result;

            Assert.IsNotNull(reviewArticleSchema.ReviewRating);
            Assert.AreEqual(1, reviewArticleSchema.ReviewRating.Count());
            Assert.AreEqual(ReviewRatingValues.OverallBestRating, reviewArticleSchema.ReviewRating.First().BestRating);
            Assert.AreEqual(ReviewRatingValues.OverallWorstRating, reviewArticleSchema.ReviewRating.First().WorstRating);
            Assert.AreEqual(article.ExpertRatings.OverallRating, reviewArticleSchema.ReviewRating.First().RatingValue);
        }

        [Test]
        public void TestExpertCategoryRatingWithOverallAndCategoryValues()
        {
            List<EditorialItem> listItems = new List<EditorialItem> { new EditorialItem()
            {
                Make = "Mazda",
                Model = "CX-5",
                Year = 2019
            }};

            EditorialExpertRating expertRating = new EditorialExpertRating()
            {
                Heading = "Overall Rating",
                SubHeading = "",
                OverallRating = 80,
                Items = new List<EditorialExpertRating.ExpertItem>()
                {
                    new EditorialExpertRating.ExpertItem()
                    {
                        Category = "X-Factor",
                        Rating = 12
                    },
                    new EditorialExpertRating.ExpertItem()
                    {
                        Category = "Behind The Wheel",
                        Rating = 18
                    }
                }
            };

            var article = new ArticleDetailsDto
            {
                ArticleType = ArticleType.Review.ToString(),
                Items = listItems,
                ExpertRatings = expertRating
            };

            var schemaDataBuilder = new SchemaDataBuilder(_schemaSettings, _tenantProvider);
            var result = schemaDataBuilder.Build(article);
            var reviewArticleSchema = (ReviewArticleSchema)result;

            Assert.IsNotNull(reviewArticleSchema.ReviewRating);

            foreach (var item in reviewArticleSchema.ReviewRating)
            {
                if (item.ReviewAspect == article.ExpertRatings.Heading)
                {
                    Assert.AreEqual(ReviewRatingValues.OverallBestRating, item.BestRating);
                    Assert.AreEqual(ReviewRatingValues.OverallWorstRating, item.WorstRating);
                }
                else
                {
                    Assert.AreEqual(ReviewRatingValues.AttributeBestRating, item.BestRating);
                    Assert.AreEqual(ReviewRatingValues.AttributeWorstRating, item.WorstRating);
                }
            }
        }
    }
}
