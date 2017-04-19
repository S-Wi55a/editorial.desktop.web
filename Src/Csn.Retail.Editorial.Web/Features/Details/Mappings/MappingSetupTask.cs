﻿using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using SocialMetaData = Csn.Retail.Editorial.Web.Features.Details.Models.SocialMetaData;
using MediaMotiveData = Csn.Retail.Editorial.Web.Features.Shared.Models.MediaMotiveData;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    [AutoBind]
    public class MappingSetupTask : IMappingSetupTask
    {
        private readonly IHeroSectionMapper _heroSectionMapper;
        private readonly ISeoDataMapper _seoDataMapper;

        public MappingSetupTask(IHeroSectionMapper heroSectionMapper,
                                ISeoDataMapper seoDataMapper)
        {
            _heroSectionMapper = heroSectionMapper;
            _seoDataMapper = seoDataMapper;
        }
        public void Run(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ArticleDetailsDto, ArticleViewModel>()
                .ForMember(dest => dest.HeroSection, opt => opt.MapFrom(src => _heroSectionMapper.Map(src)))
                .ForMember(dest => dest.UseDropCase, opt => opt.ResolveUsing<UseDropCaseResolver>())
                .ForMember(dest => dest.SeoData, opt => opt.MapFrom(src => _seoDataMapper.Map(src.SeoData)));


            // Social Meta Data
            cfg.CreateMap<Shared.Proxies.EditorialApi.SocialMetaData, SocialMetaData>();

            // MediaMotive Data
            cfg.CreateMap<Shared.Proxies.EditorialApi.MediaMotiveData, MediaMotiveData>();
            cfg.CreateMap<Shared.Proxies.EditorialApi.MediaMotiveData.MMItem, MediaMotiveData.MMItem>();
            cfg.CreateMap<Shared.Proxies.EditorialApi.MediaMotiveData.MMItem.TileUrl, MediaMotiveData.MMItem.TileUrl>();

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