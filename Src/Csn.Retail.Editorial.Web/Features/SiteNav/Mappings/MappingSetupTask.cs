using AutoMapper;
using Csn.Retail.Editorial.Web.Features.SiteNav.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;

namespace Csn.Retail.Editorial.Web.Features.SiteNav.Mappings
{
    [AutoBind]
    public class MappingSetupTask : IMappingSetupTask
    {
        public void Run(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SiteNavData, SiteNavViewModel>()
                .ForMember(dest => dest.TopNav, opt => opt.MapFrom(src => src.TopNav))
                .ForMember(dest => dest.Footer, opt => opt.MapFrom(src => src.Footer))
                .ForMember(dest => dest.Script, opt => opt.MapFrom(src => src.Script))
                .ForMember(dest => dest.Style, opt => opt.MapFrom(src => src.Style));
        }
    }
}