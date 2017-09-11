using System.Linq;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Aspect;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using IMapper = Csn.Retail.Editorial.Web.Infrastructure.Mappers.IMapper;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Shared
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
            cfg.CreateMap<RyvussNavResultDto, NavResult>();
            cfg.CreateMap<RyvussNavDto, Nav.Nav>();
            cfg.CreateMap<RyvussNavNodeDto, NavNode>();

            cfg.CreateMap<RefinementsNodeDto, NavNode>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.MultiSelectMode, opt => opt.Ignore());

            cfg.CreateMap<RyvussNavNodeDto, AspectResult>()
                .ForMember(dest => dest.Count, opt => opt.Ignore());

            cfg.CreateMap<RyvussNavNodeDto, RefinementResult>()
                .ForMember(dest => dest.Count, opt => opt.Ignore())
                .ForMember(dest => dest.Refinements, opt => opt.MapFrom(src => GetRefinements(src)));

            cfg.CreateMap<FacetNodeDto, FacetNode>()
                .ForMember(dest => dest.IsRefineable, opt => opt.MapFrom(src => IsRefineable(src)))
                .ForMember(dest => dest.Refinement, opt => opt.MapFrom(src => GetRefinement(src)))
                .ForMember(dest => dest.Refinements, opt => opt.MapFrom(src => GetRefinements(src)));

            cfg.CreateMap<BreadCrumbDto, BreadCrumb>();

            cfg.CreateMap<SearchResultDto, SearchResult>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => $"https://editorial.li.csnstatic.com/carsales{src.PhotoPath}")); // TODO: remove hardcoding
        }

        private bool IsRefineable(FacetNodeDto input)
        {
            if (input.MetaData?.IsRefineable == null || !input.MetaData.IsRefineable.Any())
                return false;

            return input.MetaData.IsRefineable.First();
        }

        private Refinement GetRefinement(FacetNodeDto input)
        {
            if (input.MetaData?.Refinement == null || !input.MetaData.Refinement.Any())
                return null;

            return input.MetaData.Refinement.First();
        }

        private NavNode GetRefinements(FacetNodeDto input)
        {
            if (input.MetaData?.Refinements == null || !input.MetaData.Refinements.Any())
                return null;

            return _mapper.Map<NavNode>(input.MetaData.Refinements.First());
        }

        private NavNode GetRefinements(RyvussNavNodeDto input)
        {
            var refinementNode = input.MetaData?.Refinements?.FirstOrDefault();

            return refinementNode == null ? null : _mapper.Map<NavNode>(refinementNode);
        }
    }
}