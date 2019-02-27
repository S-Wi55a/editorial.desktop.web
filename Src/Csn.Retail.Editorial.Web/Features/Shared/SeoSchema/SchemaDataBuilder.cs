using System;
using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Models;
using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Shared;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using Csn.Retail.Editorial.Web.Culture;

namespace Csn.Retail.Editorial.Web.Features.Shared.SeoSchema
{
    public interface ISchemaDataBuilder
    {
        ISeoSchema Build(ArticleDetailsDto article);
    }

    [AutoBind]
    public class SchemaDataBuilder : ISchemaDataBuilder
    {
        private readonly ISeoSchemaSettings _schemaSettings;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public SchemaDataBuilder(ISeoSchemaSettings schemaSettings, ITenantProvider<TenantInfo> tenantProvider)
        {
            _schemaSettings = schemaSettings;
            _tenantProvider = tenantProvider;
        }
        
        public ISeoSchema Build(ArticleDetailsDto article)
        {
            if (!_tenantProvider.Current().SeoSchemaSupport) return null;

            // REVIEW SCHEMA
            if (_schemaSettings.ArticleTypesForReviewSchema.Contains(article.ArticleType))
            {
                if (!article.Items.Any()) return BuildNews(article);

                return BuildReview(article);
            } 

            // NEWS SCHEMA
            if (_schemaSettings.ArticleTypesForNewsSchema.Contains(article.ArticleType))
            {
                return BuildNews(article);
            }

            return null;
        }

        private NewsArticleSchema BuildNews(ArticleDetailsDto article)
        {
            return new NewsArticleSchema()
            {
                InLanguage = LanguageResourceValueProvider.GetValue(LanguageConstants.LanguageCode),
                Headline = article.Headline,
                DatePublished = article.DateAvailable,
                DateModified = article.DateAvailable,
                ArticleBody = GetBodyCopyMarkup(article),
                MainEntityOfPage = GetMainEntityOnPageMarkup(article),
                Author = GetAuthorMarkup(article),
                Publisher = GetPublisherMarkup(),
                Image = GetImageMarkup(article)
            };
        }

        private ReviewArticleSchema BuildReview(ArticleDetailsDto article)
        {

            return new ReviewArticleSchema()
            {
                InLanguage = LanguageResourceValueProvider.GetValue(LanguageConstants.LanguageCode),
                Headline = article.Headline,
                DatePublished = article.DateAvailable,
                DateModified = article.DateAvailable,
                ReviewBody = GetBodyCopyMarkup(article),
                MainEntityOfPage = GetMainEntityOnPageMarkup(article),
                About = GetAboutMarkup(article),
                Author = GetAuthorMarkup(article),
                Publisher = GetPublisherMarkup(),
                ItemReviewed = GetItemsReviewedMarkup(article),
                ReviewRating = GetExpertCategoryRatingsMarkup(article),
                Image = GetImageMarkup(article)
            };
        }

        private About GetAboutMarkup(ArticleDetailsDto article)
        {
            if (article?.Headline == null) return null;

            return new About()
            {
                Name = article.Headline
            };
        }

        private IContentContributor GetAuthorMarkup(ArticleDetailsDto article)
        {
            var protocolAndDomain = $"{_tenantProvider.Current().UrlProtocol}://{_tenantProvider.Current().SiteDomain}";

            if (article?.Contributors == null) return null;

            if (article.Contributors.Any())
            {
                return new Author()
                {
                    Name = article.Contributors.First().Name,
                    Url = $"{protocolAndDomain}{article.Contributors.First().LinkUrl}",
                    Image = new ImageEntity()
                    {
                        Url = article.Contributors.First().ImageUrl
                    }
                };
            }

            return new Publisher()
            {
                Name = _tenantProvider.Current().TenantName,
                Logo = new Logo()
                {
                    Url = string.Format(_schemaSettings.LogoImageUrlPath, _tenantProvider.Current().TenantName)
                },
            };
        }

        private Publisher GetPublisherMarkup()
        {
            return new Publisher()
            {
                Name = _tenantProvider.Current().TenantName,
                Logo = new Logo()
                {
                    Url = string.Format(_schemaSettings.LogoImageUrlPath, _tenantProvider.Current().TenantName)
                }
            };
        }

        private MainEntityOnPage GetMainEntityOnPageMarkup(ArticleDetailsDto article)
        {
            var protocolAndDomain = $"{_tenantProvider.Current().UrlProtocol}://{_tenantProvider.Current().SiteDomain}";

            if (article?.DetailsPageUrlPath == null) return null;

            return new MainEntityOnPage()
            {
                Id = $"{protocolAndDomain}{article.DetailsPageUrlPath}"
            };
        }

        private string GetBodyCopyMarkup(ArticleDetailsDto article)
        {
            if (article?.ContentSections == null) return null;

            var bodyContent = article.ContentSections.FirstOrDefault((section) => section.Content.StartsWith("<p>", StringComparison.CurrentCultureIgnoreCase));

            if (bodyContent == null || !article.ContentSections.Any())
            {
                return article.Headline;
            }

            return Regex.Replace(bodyContent.Content, "<[^>]*>", "");
        }

        private IEnumerable<ImageEntity> GetImageMarkup(ArticleDetailsDto article)
        {
            if (article?.HeroSection == null) return null;

            var imageCatalogue = article.HeroSection.Images;
            return !imageCatalogue.IsNullOrEmpty() ? imageCatalogue.Select(image => new ImageEntity() { Url = image.Url }) : null;
        }

        private IEnumerable<ItemReviewed> GetItemsReviewedMarkup(ArticleDetailsDto article)
        {
            if (article?.Items == null) return null;

            return article.Items.Select(item => 
                new ItemReviewed()
                {
                    Type = _tenantProvider.Current().SeoSchemaVehicleType,
                    Name = item.Make,
                    Model = !item.Model.IsNullOrEmpty() ? item.Model : null,
                    ModelDate = (item.Year != 0) ? item.Year.ToString() : null,
                    Brand = new Brand()
                    {
                        Name = item.Make
                    }
                }
            );
        }

        private IEnumerable<ReviewRating> GetExpertCategoryRatingsMarkup(ArticleDetailsDto article)
        {
            var expertRatings = new List<ReviewRating>(){};

            if (article?.ExpertRatings == null) return null;

            expertRatings.Add(new ReviewRating()
            {
                ReviewAspect = article.ExpertRatings.Heading,
                RatingValue = article.ExpertRatings.OverallRating,
                BestRating = ReviewRatingValues.OverallBestRating,
                WorstRating = ReviewRatingValues.OverallWorstRating
            });

            article.ExpertRatings.Items.ForEach(item =>
            {
                expertRatings.Add(new ReviewRating()
                {
                    ReviewAspect = item.Category,
                    RatingValue = item.Rating,
                    BestRating = ReviewRatingValues.AttributeBestRating,
                    WorstRating = ReviewRatingValues.AttributeWorstRating
                });
            });

            return expertRatings;
        }
    }
}