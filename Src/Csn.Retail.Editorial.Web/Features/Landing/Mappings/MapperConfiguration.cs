﻿using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;

namespace Csn.Retail.Editorial.Web.Features.Landing.Mappings
{
    [AutoBind]
    public class MappingSetupTask : IMappingSetupTask
    {
        public void Run(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<CarouselItem, SearchResult>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.ArticleDetailsUrl, opt => opt.MapFrom(src => src.ItemUrl))
                .ForMember(dest => dest.DateAvailable, opt => opt.Ignore())
                .ForMember(dest => dest.DisqusArticleId, opt => opt.Ignore())
                .ForMember(dest => dest.Label, opt => opt.Ignore())
                .ForMember(dest => dest.SubHeading, opt => opt.Ignore())
                .ForMember(dest => dest.Headline, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.Ignore());
        }
    }
}