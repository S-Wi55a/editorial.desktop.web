﻿using System.Linq;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using IMapper = Csn.Retail.Editorial.Web.Infrastructure.Mappers.IMapper;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements.Mapping
{
    [AutoBind]
    public class MappingSetupTask : IMappingSetupTask
    {
        private readonly IMapper _mapper;

        public MappingSetupTask(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void Run(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<RyvussNavDto, RefinementNav>()
                .ForMember(dest => dest.Nodes, opt => opt.MapFrom(src => src.Nodes.Where(n => n.Type == "Aspect").Select(n => _mapper.Map<RefinementNavNode>(n))))
                .ForMember(dest => dest.KeywordsPlaceholder, opt => opt.ResolveUsing<KeywordsPlaceholderResolver<RefinementNav>>());

            cfg.CreateMap<RyvussNavNodeDto, RefinementNavNode>()
                .ForMember(dest => dest.Refinements, opt => opt.ResolveUsing<RefinementsNavNodeResolver>())
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.GetDisplayName()));
        }
    }
}