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
    public class SchemaAttributeHelper
    {

        public static About AboutMarkup(string descriptor)
        {
            return new About()
            {
                Name = descriptor
            };
        }

        public static IContentContributor AuthorMarkup(List<Contributor> contributors, TenantInfo tenant, string LogoPathUrl)
        {
            if (contributors.Any())
            {
                return new Author()
                {
                    Name = contributors.FirstOrDefault().Name,
                    Url = $"https://{tenant.SiteDomain}{contributors.FirstOrDefault().LinkUrl}",
                    Image = new Image()
                    {
                        Url = contributors.FirstOrDefault().ImageUrl
                    }
                };

            }
            return new Publisher()
            {
                Name = tenant.TenantName,
                Logo = new Logo()
                {
                    Url = string.Format(LogoPathUrl, tenant.TenantName)
                },
            };
        }


        public static Publisher PublisherMarkup(TenantInfo tenant, string LogoPathUrl)
        {
            return new Publisher()
            {
                Name = tenant.TenantName,
                Logo = new Logo()
                {
                    Url = string.Format(LogoPathUrl, tenant.TenantName)
                }
            };
        }

        public static MainEntityOnPage MainEntityOnPageMarkup(string fullUrlPath)
        {
            return new MainEntityOnPage()
            {
                Id = fullUrlPath
            };
        }

        public static string BodyCopyMarkup(List<ContentSection> contentSections)
        {
            var bodyContent = contentSections.Where(section => section.Content.ToLower().Contains("<p>"));

            if (!bodyContent.Any())
            {
                return contentSections.FirstOrDefault().Content;
            }

            return Regex.Replace(bodyContent.FirstOrDefault().Content, "<[^>]*>", "");
        }

        
        public static IEnumerable<Image> ImageMarkup(List<Features.Shared.Proxies.EditorialApi.Image> imageCatalogue)
        {
            return !imageCatalogue.IsNullOrEmpty() ? imageCatalogue.Select(image => new Image() { Url = image.Url}) : null;
        }


        public static List<ItemReviewed> ItemsReviewedSchemaMarkup(TenantInfo tenant, List<EditorialItem> editorialItems)
        {
            List<ItemReviewed> itemsReviews = new List<ItemReviewed>();

            editorialItems.ForEach(item =>
            {
                itemsReviews.Add(new ItemReviewed()
                {
                    Type = tenant.SeoSchemaVehilceType,
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

        public static List<ReviewRating> ExpertCategoryRatingsMarkup(ArticleDetailsDto article)
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