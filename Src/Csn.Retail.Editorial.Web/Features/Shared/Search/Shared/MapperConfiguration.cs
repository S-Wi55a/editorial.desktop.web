using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Aspect;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
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
        private readonly IResultsMessageMapper _resultsMessageMapper;
        private readonly IArticleUrlMapper _articleUrlMapper;

        public MappingSetupTask(IMapper mapper, IImageMapper imageMapper,
            IResultsMessageMapper resultsMessageMapper, IArticleUrlMapper articleUrlMapper)
        {
            _mapper = mapper;
            _imageMapper = imageMapper;
            _resultsMessageMapper = resultsMessageMapper;
            _articleUrlMapper = articleUrlMapper;
        }

        public void Run(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<RyvussNavResultDto, NavResult>()
                .ForMember(dest => dest.NoResultsInstructionMessage, opt => opt.MapFrom(src => _resultsMessageMapper.MapNoResultInstructionMessage(src.Count)))
                .ForMember(dest => dest.ResultsMessage, opt => opt.MapFrom(src => _resultsMessageMapper.MapResultMessage(src)));
                
            cfg.CreateMap<BreadCrumbDto, BreadCrumb>()
                .ForMember(dest => dest.Term, opt => opt.Ignore())
                .ForMember(dest => dest.RemoveAction, opt => opt.ResolveUsing<BreadCrumbRemoveActoinResolver>());

            cfg.CreateMap<RyvussNavDto, Nav.Nav>()
                .ForMember(dest => dest.BreadCrumbs, opt => opt.ResolveUsing<BreadCrumbMapperResolver>())
                .ForMember(dest => dest.Nodes, opt => opt.ResolveUsing<NavNodeResolver>())
                .ForMember(dest => dest.KeywordsPlaceholder, opt => opt.ResolveUsing<KeywordsPlaceholderResolver<Nav.Nav>>())
                .ForMember(dest => dest.CurrentAction, opt => opt.Ignore());

            cfg.CreateMap<RyvussNavNodeDto, NavNode>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.GetDisplayName()));

            cfg.CreateMap<RefinementsNodeDto, NavNode>()
                .ForMember(dest => dest.MultiSelectMode, opt => opt.Ignore());
            
            cfg.CreateMap<RyvussNavNodeDto, AspectResult>()
                .ForMember(dest => dest.Count, opt => opt.Ignore())
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.GetDisplayName()));

            cfg.CreateMap<FacetNodeDto, FacetNode>()
                .ForMember(dest => dest.IsRefineable, opt => opt.MapFrom(src => src.IsRefineable()))
                .ForMember(dest => dest.Refinement, opt => opt.MapFrom(src => src.GetRefinement()))
                .ForMember(dest => dest.Url, opt => opt.ResolveUsing<FacetNodeUrlResolver>())
                .ForMember(dest => dest.Action, opt => opt.ResolveUsing<FacetNodeActionResolver>())
                .ForMember(dest => dest.Refinements, opt => opt.MapFrom(src => _mapper.Map<NavNode>(src.GetRefinements())));

            cfg.CreateMap<SearchResultDto, SearchResult>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => _imageMapper.MapImageUrl(src)))
                .ForMember(dest => dest.DateAvailable, opt => opt.MapFrom(src => src.MapDateAvailable()))
                .ForMember(dest => dest.ArticleDetailsUrl, opt => opt.MapFrom(src => _articleUrlMapper.MapDetailsUrl(src)))     
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.GetSponsoredLabel()))
                .ForMember(dest => dest.DisqusArticleId, opt => opt.MapFrom(src => src.GetDisqusArticleId()));
        }
    }
}