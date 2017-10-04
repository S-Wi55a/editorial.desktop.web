using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Aspect;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping;
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
        private readonly IImageMapper _imageMapper;
        private readonly IBreadCrumbMapper _breadCrumbMapper;
        private readonly IResultsMessageMapper _resultsMessageMapper;
        private readonly IArticleUrlMapper _articleUrlMapper;


        public MappingSetupTask(IMapper mapper, IImageMapper imageMapper, IBreadCrumbMapper breadCrumbMapper,
            IResultsMessageMapper resultsMessageMapper, IArticleUrlMapper articleUrlMapper)
        {
            _mapper = mapper;
            _imageMapper = imageMapper;
            _breadCrumbMapper = breadCrumbMapper;
            _resultsMessageMapper = resultsMessageMapper;
            _articleUrlMapper = articleUrlMapper;
        }
        public void Run(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<RyvussNavResultDto, NavResult>()
                .ForMember(dest => dest.NoResultsMessage, opt => opt.MapFrom(src => _resultsMessageMapper.MapResultMessage(src.Count)))
                .ForMember(dest => dest.NoResultsInstructionMessage, opt => opt.MapFrom(src => _resultsMessageMapper.MapResultInstructionMessage(src.Count)));

            cfg.CreateMap<BreadCrumbDto, BreadCrumb>();

            cfg.CreateMap<RyvussNavDto, Nav.Nav>()
                .ForMember(dest => dest.BreadCrumbs, opt => opt.MapFrom(src => _breadCrumbMapper.GetAggregatedBreadCrumbs(src.BreadCrumbs)));

            cfg.CreateMap<RyvussNavNodeDto, NavNode>();

            cfg.CreateMap<RefinementsNodeDto, NavNode>()
                .ForMember(dest => dest.MultiSelectMode, opt => opt.Ignore());

            cfg.CreateMap<RefinementsNodeDto, RefinementNavNode>()
                .ForMember(dest => dest.MultiSelectMode, opt => opt.Ignore())
                .ForMember(dest => dest.Refinement, opt => opt.MapFrom(src => src.GetParentExpression()));

            cfg.CreateMap<RyvussNavNodeDto, AspectResult>()
                .ForMember(dest => dest.Count, opt => opt.Ignore());

            cfg.CreateMap<RyvussNavNodeDto, RefinementResult>()
                .ForMember(dest => dest.Count, opt => opt.Ignore())
                .ForMember(dest => dest.Refinements, opt => opt.MapFrom(src => _mapper.Map<RefinementNavNode>(src.GetRefinements())));

            cfg.CreateMap<FacetNodeDto, FacetNode>()
                .ForMember(dest => dest.IsRefineable, opt => opt.MapFrom(src => src.IsRefineable()))
                .ForMember(dest => dest.Refinement, opt => opt.MapFrom(src => src.GetRefinement()))
                .ForMember(dest => dest.Refinements, opt => opt.MapFrom(src => _mapper.Map<NavNode>(src.GetRefinements())));

            cfg.CreateMap<SearchResultDto, SearchResult>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => _imageMapper.MapImageUrl(src)))
                .ForMember(dest => dest.DateAvailable, opt => opt.MapFrom(src => src.MapDateAvailable()))
                .ForMember(dest => dest.ArticleDetailsUrl, opt => opt.MapFrom(src => _articleUrlMapper.MapDetailsUrl(src)));
        }        
    }
}