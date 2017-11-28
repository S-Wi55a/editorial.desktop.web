using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements.Mapping
{
    [AutoBind]
    public class MappingSetupTask : IMappingSetupTask
    {
        public void Run(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<RyvussNavDto, RefinementNav>()
                .ForMember(dest => dest.Nodes, opt => opt.ResolveUsing<NodeRefinementsResolver>())
                .ForMember(dest => dest.KeywordsPlaceholder, opt => opt.ResolveUsing<KeywordsPlaceholderResolver<RefinementNav>>());

            cfg.CreateMap<RyvussNavNodeDto, NavNodeWithRefinements>()
                .ForMember(dest => dest.Refinements, opt => opt.ResolveUsing<RefinementsNavNodeResolver>())
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.GetDisplayName()));

            cfg.CreateMap<RefinementsNodeDto, RefinementsNavNode>()
                .ForMember(dest => dest.Refinement, opt => opt.MapFrom(src => src.GetParentExpression()))
                .ForMember(dest => dest.MultiSelectMode, opt => opt.Ignore());
        }
    }
}