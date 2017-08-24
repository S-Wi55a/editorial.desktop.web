using System.Linq;
using AutoMapper;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Nav
{
    [AutoBind]
    public class MappingSetupTask : IMappingSetupTask
    {
        public void Run(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<RyvussNavResultDto, NavResult>();

            cfg.CreateMap<RyvussNavDto, Nav>();
            cfg.CreateMap<RyvussNavNodeDto, NavNode>();
            cfg.CreateMap<FacetNodeDto, FacetNode>()
                .ForMember(dest => dest.IsRefineable, opt => opt.MapFrom(src => IsRefineable(src)));
            cfg.CreateMap<BreadCrumbDto, BreadCrumb>();


            cfg.CreateMap<SearchResultDto, SearchResult>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => $"https://editorial.li.csnstatic.com/carsales{src.PhotoPath}"));
        }

        private static bool IsRefineable(FacetNodeDto input)
        {
            if (input.MetaData?.IsRefineable == null || !input.MetaData.IsRefineable.Any())
                return false;

            return input.MetaData.IsRefineable.First();
        }
    }
}