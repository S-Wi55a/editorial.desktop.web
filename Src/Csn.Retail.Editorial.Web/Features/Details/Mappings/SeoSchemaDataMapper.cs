using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Models;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Helpers;
using System.Web;
using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Shared;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    public interface ISeoSchemaDataMapper
    {
        SeoSchemaMarkupBase Map(ArticleDetailsDto source);
    }

    [AutoBind]
    public class SeoSchemaDataMapper : ISeoSchemaDataMapper
    {
        private readonly ISeoSchemaSettings _schemaSettings;
        private readonly ITenantProvider<TenantInfo> _tenantInfo;
        
        public SeoSchemaDataMapper(ISeoSchemaSettings schemaSettings, ITenantProvider<TenantInfo> tenantInfo)
        {
            _schemaSettings = schemaSettings;
            _tenantInfo = tenantInfo;
        }

        public SeoSchemaMarkupBase Map(ArticleDetailsDto source)
        {
            if (!_tenantInfo.Current().SeoSchemaSupport) return new SeoSchemaMarkupBase() { };
            return SelectSchemaType(source);
        }

        private SeoSchemaMarkupBase SelectSchemaType(ArticleDetailsDto source)
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
            return new SeoSchemaMarkupBase() { };
        }

        private SeoSchemaMarkupBase MapNewsArticleMarkup(ArticleDetailsDto article)
        {
            var builder = new SchemaMarkupBuilder(_tenantInfo.Current(), article);

            return new NewsArticleSchema()
            {
                inLanguage = _tenantInfo.Current().LanguageCode,
                Headline = article.Headline,
                DatePublished = article.DateAvailable,
                DateModified = article.DateAvailable,
                ArticleBody = builder.BodyCopyMarkup(),
                MainEntityOfPage = builder.MainEntityOnPageMarkup(HttpContext.Current.Request.Url.AbsoluteUri),
                Author = builder.AuthorMarkup(_schemaSettings.LogoImageUrlPath),
                Publisher = builder.PublisherMarkup(_schemaSettings.LogoImageUrlPath),
                Image = builder.ImageMarkup()
            };
        }

        private SeoSchemaMarkupBase MapReviewArticleMarkup(ArticleDetailsDto article)
        {
            var builder = new SchemaMarkupBuilder(_tenantInfo.Current(), article);

            if (!article.Items.Any())
            {
                return MapNewsArticleMarkup(article);
            }

            return new ReviewArticleSchema()
            {
                inLanguage = _tenantInfo.Current().LanguageCode,
                Headline = article.Headline,
                DatePublished = article.DateAvailable,
                DateModified = article.DateAvailable,
                ReviewBody = builder.BodyCopyMarkup(),
                MainEntityOfPage = builder.MainEntityOnPageMarkup(HttpContext.Current.Request.Url.AbsoluteUri),
                About = builder.AboutMarkup(),
                Author = builder.AuthorMarkup(_schemaSettings.LogoImageUrlPath),
                Publisher = builder.PublisherMarkup(_schemaSettings.LogoImageUrlPath),
                ItemReviewed = builder.ItemsReviewedSchemaMarkup(),
                ReviewRating = builder.ExpertCategoryRatingsMarkup(),
                Image = builder.ImageMarkup()
            };   
        }
    }   
}
