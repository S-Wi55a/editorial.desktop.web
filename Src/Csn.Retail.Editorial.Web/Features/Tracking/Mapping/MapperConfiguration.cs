using System.Linq;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Listings.Constants;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Features.Shared.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.WebMetrics.Core.Model;

namespace Csn.Retail.Editorial.Web.Features.Tracking.Mapping
{
    [AutoBind]
    public class MappingSetupTask : IMappingSetupTask
    {
        public void Run(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SearchResultDto, AnalyticsEditorialTrackingItem>()
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.GetNumericId()))
                .ForMember(dest => dest.NetworkId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => TrackingItemSource.Editorial))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Categories != null ? src.Categories.FirstOrDefault() : null))
                .ForMember(dest => dest.LifeStyle, opt => opt.MapFrom(src => src.Lifestyles != null ? src.Lifestyles.FirstOrDefault() : null))
                .ForMember(dest => dest.Make, opt => opt.MapFrom(src => ItemsListingMapper.MapMakes(src)))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => ItemsListingMapper.MapModels(src)))
                .ForMember(dest => dest.MarketingGroup, opt => opt.Ignore())
                .ForMember(dest => dest.CanonicalUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ArticleType, opt => opt.Ignore())
                .ForMember(dest => dest.Vertical, opt => opt.Ignore())
                .ForMember(dest => dest.PageType, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Headline))
                .ForMember(dest => dest.YearPublished, opt => opt.MapFrom(src => src.DateAvailable.ToString("yyyy")))
                .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => src.DateAvailable))
                .ForMember(dest => dest.ItemProperties, opt => opt.Ignore()); // the logic for this is in TrackingPropertiesBuilder if we ever need it

            cfg.CreateMap<RyvussSearch, AnalyticsSearchContext>()
                .ForMember(c => c.ItemsPerPage, c => c.UseValue(PageItemsLimit.ListingPageItemsLimit))
                .ForMember(c => c.PageNumber, c => c.MapFrom(src => (src.SearchContext.Offset / PageItemsLimit.ListingPageItemsLimit) + 1))
                .ForMember(c => c.PageType, c => c.Ignore())
                .ForMember(c => c.SearchType, c => c.Ignore())
                .ForMember(c => c.SearchList, c => c.Ignore())
                .ForMember(c => c.TotalSearchResults, c => c.MapFrom(src => src.RyvussNavResult.Count))
                .ForMember(c => c.RangeFilters, c => c.Ignore())
                .ForMember(c => c.SearchFilters, c => c.MapFrom(src => SearchFilterMapper.Map(src.RyvussNavResult)))
                .ForMember(c => c.EventType, c => c.UseValue(SearchEventType.Search)) // hard code for now.....will have to use search context instead
                .ForMember(c => c.EventSource, c => c.Ignore())
                .ForMember(c => c.EventArg, c => c.Ignore()); 
        }
    }
}