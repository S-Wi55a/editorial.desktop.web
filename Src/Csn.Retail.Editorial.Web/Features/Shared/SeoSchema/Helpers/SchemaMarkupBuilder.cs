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
using System.Web;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;

namespace Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Helpers
{
    public interface ISchemaMarkupBuilder
    {
        SeoSchemaBase Build(ArticleDetailsDto article);
    }

    [AutoBind]
    public class SchemaMarkupBuilder : ISchemaMarkupBuilder
    {
        private readonly ISeoSchemaSettings _schemaSettings;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public SchemaMarkupBuilder(ISeoSchemaSettings schemaSettings, ITenantProvider<TenantInfo> tenantProvider)
        {
            _schemaSettings = schemaSettings;
            _tenantProvider = tenantProvider;
        }
        
        public SeoSchemaBase Build(ArticleDetailsDto article)
        {
            // REVIEW SCHEMA
            if (_schemaSettings.ArticleTypesForReviewSchema.Contains(article.ArticleType) && article.Items.Any())
            {
                return BuildReview(article);
            }

            // NEWS SCHEMA
            if (_schemaSettings.ArticleTypesForNewsSchema.Contains(article.ArticleType))
            {
                return BuildNews(article);
            }

            return new SeoSchemaBase(){ };
        }


        public NewsArticleSchema BuildNews(ArticleDetailsDto article)
        {
            return new NewsArticleSchema()
            {
                inLanguage = _tenantProvider.Current().LanguageCode,
                Headline = article.Headline,
                DatePublished = article.DateAvailable,
                DateModified = article.DateAvailable,
                ArticleBody = GetBodyCopyMarkup(article),
                MainEntityOfPage = GetMainEntityOnPageMarkup(HttpContext.Current.Request.Url.AbsoluteUri),
                Author = GetAuthorMarkup(article),
                Publisher = GetPublisherMarkup(),
                Image = GetImageMarkup(article)
            };
        }

        public ReviewArticleSchema BuildReview(ArticleDetailsDto article)
        {
            return new ReviewArticleSchema()
            {
                inLanguage = _tenantProvider.Current().LanguageCode,
                Headline = article.Headline,
                DatePublished = article.DateAvailable,
                DateModified = article.DateAvailable,
                ReviewBody = GetBodyCopyMarkup(article),
                MainEntityOfPage = GetMainEntityOnPageMarkup(HttpContext.Current.Request.Url.AbsoluteUri),
                About = GetAboutMarkup(article),
                Author = GetAuthorMarkup(article),
                Publisher = GetPublisherMarkup(),
                ItemReviewed = GetItemsReviewedSchemaMarkup(article),
                ReviewRating = GetExpertCategoryRatingsMarkup(article),
                Image = GetImageMarkup(article)
            };
        }

        private About GetAboutMarkup(ArticleDetailsDto article)
        {
            return new About()
            {
                Name = article.Headline
            };
        }

        private IContentContributor GetAuthorMarkup(ArticleDetailsDto article)
        {
            if (article.Contributors.Any())
            {
                return new Author()
                {
                    Name = article.Contributors.FirstOrDefault().Name,
                    Url = $"https://{_tenantProvider.Current().SiteDomain}{article.Contributors.FirstOrDefault().LinkUrl}",
                    Image = new ImageEntity()
                    {
                        Url = article.Contributors.FirstOrDefault().ImageUrl
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

        private MainEntityOnPage GetMainEntityOnPageMarkup(string fullUriPath)
        {
            return new MainEntityOnPage()
            {
                Id = fullUriPath
            };
        }


        private string GetBodyCopyMarkup(ArticleDetailsDto article)
        {
            var bodyContent = article.ContentSections.Where(section => section.Content.ToLower().Contains("<p>"));

            if (!bodyContent.Any())
            {
                return article.ContentSections.FirstOrDefault().Content;
            }

            return Regex.Replace(bodyContent.FirstOrDefault().Content, "<[^>]*>", "");
        }

        private IEnumerable<ImageEntity> GetImageMarkup(ArticleDetailsDto article)
        {
            var imageCatalogue = article.HeroSection.Images;
            return !imageCatalogue.IsNullOrEmpty() ? imageCatalogue.Select(image => new ImageEntity() { Url = image.Url }) : null;
        }

        private List<ItemReviewed> GetItemsReviewedSchemaMarkup(ArticleDetailsDto article)
        {
            List<ItemReviewed> itemsReviews = new List<ItemReviewed>();

            article.Items.ForEach(item =>
            {
                itemsReviews.Add(new ItemReviewed()
                {
                    Type = _tenantProvider.Current().SeoSchemaVehicleType,
                    Name = item.Make,
                    Model = item.Model,
                    ModelDate = item.Year,
                    Brand = new Brand()
                    {
                        Name = item.Make
                    }
                });
            });

            return itemsReviews;
        }

        private List<ReviewRating> GetExpertCategoryRatingsMarkup(ArticleDetailsDto article)
        {
            var expertRatings = new List<ReviewRating> { };

            try
            {
                expertRatings.Add(new ReviewRating()
                {
                    ReviewAspect = article.ExpertRatings.Heading,
                    RatingValue = article.ExpertRatings.OverallRating,
                    BestRating = ReviewRatingValues.OverallBestRating,
                    WorstRating = ReviewRatingValues.OverallWorstRating
                });

                article.ExpertRatings.Items.ForEach(rating =>
                {
                    expertRatings.Add(new ReviewRating()
                    {
                        ReviewAspect = rating.Category,
                        RatingValue = rating.Rating,
                        BestRating = ReviewRatingValues.AttributeBestRating,
                        WorstRating = ReviewRatingValues.AttributelWorstRating
                    });
                });
            }
            catch
            {
                return null;
            }

            return expertRatings;
        }

        
    }
}