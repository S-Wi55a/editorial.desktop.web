using System.Linq;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
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
        private readonly ISeoSchemaDataMapper _seoSchemaDataMapper;
        private readonly IPolarNativeAdsDataMapper _polarNativeAdsMapper;
        private readonly IUseDropCaseMapper _useDropCaseMapper;
        private readonly IArticleTypeLabelMapper _articleTypeLabelMapper;

        public MappingSetupTask(IHeroSectionMapper heroSectionMapper,
                                ISeoDataMapper seoDataMapper,
                                ISeoSchemaDataMapper seoSchemaDataMapper,
                                IPolarNativeAdsDataMapper polarNativeAdsMapper,
                                IUseDropCaseMapper useDropCaseMapper,
                                IArticleTypeLabelMapper articleTypeLabelMapper)
        {
            _heroSectionMapper = heroSectionMapper;
            _seoDataMapper = seoDataMapper;
            _seoSchemaDataMapper = seoSchemaDataMapper;
            _polarNativeAdsMapper = polarNativeAdsMapper;
            _useDropCaseMapper = useDropCaseMapper;
            _articleTypeLabelMapper = articleTypeLabelMapper;
        }
        public void Run(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ArticleDetailsDto, ArticleViewModel>()
                .ForMember(dest => dest.HeroSection, opt => opt.MapFrom(src => _heroSectionMapper.Map(src)))
                .ForMember(dest => dest.UseDropCase, opt => opt.MapFrom(src => _useDropCaseMapper.Map(src)))
                .ForMember(dest => dest.SeoData, opt => opt.MapFrom(src => _seoDataMapper.Map(src.SeoData)))
                .ForMember(dest => dest.SeoSchemaMarkup, opt => opt.MapFrom(src => _seoSchemaDataMapper.Map(src)))
                .ForMember(dest => dest.PolarNativeAdsData, opt => opt.MapFrom(src => _polarNativeAdsMapper.Map(src)))
                .ForMember(dest => dest.MoreArticleData, opt => opt.MapFrom(src => src.MoreArticleData))
                .ForMember(dest => dest.ArticleTypeLabel, opt => opt.MapFrom(src => _articleTypeLabelMapper.Map(src)))
                .ForMember(dest => dest.InsightsData, opt => opt.MapFrom(src => new CsnInsightsData{MetaData = src.InsightsData}))
                .ForMember(dest => dest.ContentSections, opt => opt.MapFrom(src => src.ContentSections.Select(c => new ContentSectionViewModel(c, src.NetworkId))));

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
        }
    }
}