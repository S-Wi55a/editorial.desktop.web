using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Models;
using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Shared;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;
using Image = Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Models.Image;
using System.Text.RegularExpressions;

namespace Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Helpers
{
    public class SchemaMarkupBuilder
    {

        TenantInfo _tenant { get; set; }
        ArticleDetailsDto _article { get; set; }

        public SchemaMarkupBuilder(TenantInfo tenant, ArticleDetailsDto article)
        {
            _tenant = tenant;
            _article = article;
        }

        public About AboutMarkup()
        {
            return new About()
            {
                Name = _article.Headline
            };
        }

        public IContentContributor AuthorMarkup(string LogoPathUrl)
        {
            if (_article.Contributors.Any())
            {
                return new Author()
                {
                    Name = _article.Contributors.FirstOrDefault().Name,
                    Url = $"https://{_tenant.SiteDomain}{_article.Contributors.FirstOrDefault().LinkUrl}",
                    Image = new Image()
                    {
                        Url = _article.Contributors.FirstOrDefault().ImageUrl
                    }
                };

            }
            return new Publisher()
            {
                Name = _tenant.TenantName,
                Logo = new Logo()
                {
                    Url = string.Format(LogoPathUrl, _tenant.TenantName)
                },
            };
        }

        public Publisher PublisherMarkup(string LogoPathUrl)
        {
            return new Publisher()
            {
                Name = _tenant.TenantName,
                Logo = new Logo()
                {
                    Url = string.Format(LogoPathUrl, _tenant.TenantName)
                }
            };
        }

        public MainEntityOnPage MainEntityOnPageMarkup(string fullUriPath)
        {
            return new MainEntityOnPage()
            {
                Id = fullUriPath
            };
        }

        public string BodyCopyMarkup()
        {
            var bodyContent = _article.ContentSections.Where(section => section.Content.ToLower().Contains("<p>"));

            if (!bodyContent.Any())
            {
                return _article.ContentSections.FirstOrDefault().Content;
            }

            return Regex.Replace(bodyContent.FirstOrDefault().Content, "<[^>]*>", "");
        }

        public IEnumerable<Image> ImageMarkup()
        {
            var imageCatalogue = _article.HeroSection.Images;
            return !imageCatalogue.IsNullOrEmpty() ? imageCatalogue.Select(image => new Image() { Url = image.Url }) : null;
        }

        public List<ItemReviewed> ItemsReviewedSchemaMarkup()
        {
            List<ItemReviewed> itemsReviews = new List<ItemReviewed>();

            _article.Items.ForEach(item =>
            {
                itemsReviews.Add(new ItemReviewed()
                {
                    Type = _tenant.SeoSchemaVehicleType,
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

        public List<ReviewRating> ExpertCategoryRatingsMarkup()
        {
            var expertRatings = new List<ReviewRating> { };

            try
            {
                expertRatings.Add(new ReviewRating()
                {
                    ReviewAspect = _article.ExpertRatings.Heading,
                    RatingValue = _article.ExpertRatings.OverallRating,
                    BestRating = ReviewRatingValues.OverallBestRating,
                    WorstRating = ReviewRatingValues.OverallWorstRating
                });

                _article.ExpertRatings.Items.ForEach(rating =>
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