using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Models;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Helpers;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    public interface ISeoSchemaDataMapper
    {
        SeoSchemaMarkup Map(ArticleDetailsDto source);
    }

    [AutoBind]
    public class SeoSchemaDataMapper : ISeoSchemaDataMapper
    {
        private readonly SeoSchemaSettings _schemaSettings;
        private readonly ITenantProvider<TenantInfo> _tenantInfo;

        public SeoSchemaDataMapper(SeoSchemaSettings schemaSettings, ITenantProvider<TenantInfo> tenantInfo)
        {
            _schemaSettings = schemaSettings;
            _tenantInfo = tenantInfo;
        }

        public SeoSchemaMarkup Map(ArticleDetailsDto source)
        {

            if (!_tenantInfo.Current().SeoSchemaSupport) return new SeoSchemaMarkup() { };

            return SelectSchemaType(source);
        }

        private SeoSchemaMarkup SelectSchemaType(ArticleDetailsDto source)
        {
            
            //Review Schema
            if (_schemaSettings.ArticleTypesForReviewSchema.Contains(source.ArticleType))
            {
                return MapReviewArticleMarkup(source);
            }

            // News Schema
            if (_schemaSettings.ArticleTypesForNewsSchema.Contains(source.ArticleType))
            {
                return MapNewsArticleMarkup(source);
            }

            return new SeoSchemaMarkup() { };
        }


        private SeoSchemaMarkup MapNewsArticleMarkup(ArticleDetailsDto article)
        {

            return new NewsArticleSchema()
            {
                Headline = article.Headline,
                DatePublished = article.DateAvailable,
                DateModified = article.DateAvailable,
                ArticleBody = SchemaAttributeHelper.BodyCopyMarkup(article.ContentSections),
                MainEntityOfPage = MarkupHelper.GenerateFullUrlPath(_tenantInfo.Current().SiteDomain, article.DetailsPageUrlPath),
                Author = SchemaAttributeHelper.AuthorMarkup(article.Contributors, _tenantInfo.Current(), _schemaSettings.LogoImageUrlPath),
                Publisher = SchemaAttributeHelper.PublisherMarkup(_tenantInfo.Current(), _schemaSettings.LogoImageUrlPath),
                Image = SchemaAttributeHelper.ImageMarkup(article.HeroSection.Images)
            };
        }


        private SeoSchemaMarkup MapReviewArticleMarkup(ArticleDetailsDto article)
        {

            if (!article.Items.Any())
            {
                return MapNewsArticleMarkup(article);
            }

            return new ReviewArticleSchema()
            {
                Headline = article.Headline,
                DatePublished = article.DateAvailable,
                DateModified = article.DateAvailable,
                ReviewBody = SchemaAttributeHelper.BodyCopyMarkup(article.ContentSections),
                MainEntityOfPage = SchemaAttributeHelper.MainEntityOnPageMarkup(_tenantInfo.Current().SiteDomain, article.DetailsPageUrlPath),
                About = SchemaAttributeHelper.AboutMarkup(article.Headline),
                Author = SchemaAttributeHelper.AuthorMarkup(article.Contributors, _tenantInfo.Current(), _schemaSettings.LogoImageUrlPath),
                Publisher = SchemaAttributeHelper.PublisherMarkup(_tenantInfo.Current(), _schemaSettings.LogoImageUrlPath),
                ItemReviewed = SchemaAttributeHelper.ItemsReviewedSchemaMarkup(_tenantInfo.Current(), article.Items),
                ReviewRating = SchemaAttributeHelper.ExpertCategoryRatingsMarkup(article),
                Image = SchemaAttributeHelper.ImageMarkup(article.HeroSection.Images)
            };
            
        }
    }
}
