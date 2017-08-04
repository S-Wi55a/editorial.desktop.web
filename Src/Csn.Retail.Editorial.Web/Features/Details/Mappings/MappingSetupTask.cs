using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using SocialMetaData = Csn.Retail.Editorial.Web.Features.Details.Models.SocialMetaData;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    [AutoBind]
    public class MappingSetupTask : IMappingSetupTask
    {
        private readonly IHeroSectionMapper _heroSectionMapper;
        private readonly ISeoDataMapper _seoDataMapper;
        private readonly IPolarNativeAdsDataMapper _polarNativeAdsMapper;
        private readonly ISpecDataMapper _specDataMapper;
        private readonly IUseDropCaseMapper _useDropCaseMapper;

        public MappingSetupTask(IHeroSectionMapper heroSectionMapper,
                                ISeoDataMapper seoDataMapper,
                                IPolarNativeAdsDataMapper polarNativeAdsMapper,
                                ISpecDataMapper specDataMapper,
                                IUseDropCaseMapper useDropCaseMapper)
        {
            _heroSectionMapper = heroSectionMapper;
            _seoDataMapper = seoDataMapper;
            _polarNativeAdsMapper = polarNativeAdsMapper;
            _specDataMapper = specDataMapper;
            _useDropCaseMapper = useDropCaseMapper;
        }
        public void Run(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ArticleDetailsDto, ArticleViewModel>()
                .ForMember(dest => dest.HeroSection, opt => opt.MapFrom(src => _heroSectionMapper.Map(src)))
                .ForMember(dest => dest.UseDropCase, opt => opt.MapFrom(src => _useDropCaseMapper.Map(src)))
                .ForMember(dest => dest.SeoData, opt => opt.MapFrom(src => _seoDataMapper.Map(src.SeoData)))
                .ForMember(dest => dest.SpecData, opt => opt.MapFrom(src => _specDataMapper.Map(src.SpecData)))
                .ForMember(dest => dest.PolarNativeAdsData, opt => opt.MapFrom(src => _polarNativeAdsMapper.Map(src)))
                .ForMember(dest => dest.MoreArticleData, opt => opt.MapFrom(src => src.MoreArticleData));

            // Social Meta Data
            cfg.CreateMap<Shared.Proxies.EditorialApi.SocialMetaData, SocialMetaData>();

            // ProCon
            cfg.CreateMap<Shared.Proxies.EditorialApi.ProCon, Models.ProCon>();
            cfg.CreateMap<Shared.Proxies.EditorialApi.ProCon.Con, Models.ProCon.Con>();
            cfg.CreateMap<Shared.Proxies.EditorialApi.ProCon.Pro, Models.ProCon.Pro>();

            // Expert Ratings
            cfg.CreateMap<Shared.Proxies.EditorialApi.EditorialExpertRating, Models.EditorialExpertRating>();
            cfg.CreateMap<Shared.Proxies.EditorialApi.EditorialExpertRating.ExpertItem, Models.EditorialExpertRating.ExpertItem>();

            // Disqus
            cfg.CreateMap<Shared.Proxies.EditorialApi.DisqusData, Models.DisqusData>();

            // Google Analytics Data
            cfg.CreateMap<GoogleAnalyticsDetailsDto, GoogleAnalyticsDetailsData>();
        }
    }
}