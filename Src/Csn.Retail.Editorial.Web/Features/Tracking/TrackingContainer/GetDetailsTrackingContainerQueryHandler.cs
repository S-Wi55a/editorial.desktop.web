using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Constants;
using Csn.SimpleCqrs;
using Csn.WebMetrics.Core.Model;
using Csn.WebMetrics.Editorial.Interfaces;
using ArticleType = Csn.WebMetrics.Core.Model.ArticleType;
using IContextStore = Csn.Retail.Editorial.Web.Infrastructure.ContextStores.IContextStore;

namespace Csn.Retail.Editorial.Web.Features.Tracking.TrackingContainer
{
    [AutoBind]
    public class GetDetailsTrackingContainerQueryHandler : IQueryHandler<GetDetailsTrackingContainerQuery, IAnalyticsTrackingContainer>
    {
        private const string Key = "Tracking.Editorial";

        private readonly IEditorialDetailsTrackingContainerProvider _provider;
        private readonly IContextStore _contextStore;
        private readonly HttpContextBase _httpContext;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public GetDetailsTrackingContainerQueryHandler(IEditorialDetailsTrackingContainerProvider provider, IContextStore contextStore, HttpContextBase httpContextBase, ITenantProvider<TenantInfo> tenantProvider)
        {
            _provider = provider;
            _contextStore = contextStore;
            _httpContext = httpContextBase;
            _tenantProvider = tenantProvider;
        }

        public IAnalyticsTrackingContainer Handle(GetDetailsTrackingContainerQuery containerQuery)
        {
            return _contextStore.GetOrFetch(Key, () => GetTrackingContainer(containerQuery));
        }

        private IAnalyticsTrackingContainer GetTrackingContainer(GetDetailsTrackingContainerQuery containerQuery)
        {
            var article = containerQuery.Article;

            var trackingItem = new AnalyticsEditorialTrackingItem
            {
                ArticleType = GetArticleType(article),
                Category = article.Categories.Any() ? string.Join(";", article.Categories.Distinct()) : string.Empty,
                ItemProperties = new List<KeyValuePair<string, string>>(),
                LifeStyle = article.Lifestyles.Any() ? string.Join(";", article.Lifestyles.Distinct()) : string.Empty,
                Make = article.Items.Any() ? string.Join(";", article.Items.Select(i => i.Make ?? string.Empty).Distinct()) : string.Empty,
                Model = article.Items.Any() ? string.Join(";", article.Items.Select(i => i.Model ?? string.Empty).Distinct()) : string.Empty,
                MarketingGroup = article.Items.Any() ? string.Join(";", article.Items.Select(i => i.MarketingGroup ?? string.Empty).Distinct()) : string.Empty,
                NetworkId = article.NetworkId,
                PublishedDate = Convert.ToDateTime(article.DateAvailable).ToString("s"),
                Source = TrackingItemSource.Editorial,
                Title = article.Headline,
                Vertical = GetVertical(),
                YearPublished = Convert.ToDateTime(article.DateAvailable).Year.ToString(),
                PageType = containerQuery.PageType == "gallery" ? EditorialPageType.PhotoGallery : EditorialPageType.Editorial,
                CanonicalUrl = GetCanonicalUrl(article)
            };
            return _provider.GetContainer(trackingItem, _httpContext);
        }

        private string GetCanonicalUrl(ArticleViewModel article)
        {
            var tenant = _tenantProvider.Current().Name;
            switch (tenant)
            {
                case TenantNames.Carsales:
                    {
                        return article.SeoData.CanonicalUrl;
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }

        private ItemVerticalType GetVertical()
        {
            var tenant = _tenantProvider.Current().Name;
            switch (tenant)
            {
                case TenantNames.Carsales:
                    {
                        return ItemVerticalType.Car;
                    }
                case TenantNames.Trucksales:
                    {
                        return ItemVerticalType.Truck;
                    }
                case TenantNames.CaravanCampingSales:
                    {
                        return ItemVerticalType.Caravan;
                    }
                default:
                    {
                        return ItemVerticalType.Unknown;
                    }
            }
        }

        private ArticleType GetArticleType(ArticleViewModel article)
        {
            switch (article.ArticleType)
            {
                case "News":
                    return ArticleType.News;
                case "Review":
                    return ArticleType.Review;
                case "Car Advice":
                    return ArticleType.Advice;
                case "Video":
                    return ArticleType.Video;
                default:
                    return ArticleType.Unknown;
            }
        }
    }
}