using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using IMapper = Csn.Retail.Editorial.Web.Infrastructure.Mappers.IMapper;

namespace Csn.Retail.Editorial.Web.Features.Listings.Mappings
{
    [AutoBind]
    public class MappingSetupTask : IMappingSetupTask
    {
        private readonly IPolarNativeAdsDataMapper _polarNativeAdsMapper;
        private readonly IMapper _mapper;

        public MappingSetupTask(IPolarNativeAdsDataMapper polarNativeAdsMapper, IMapper mapper)
        {
            _polarNativeAdsMapper = polarNativeAdsMapper;
            _mapper = mapper;
        }
        public void Run(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap <RyvussNavResultDto, ListingsViewModel > ()
                .ForMember(dest => dest.NavResults, opt => opt.MapFrom(src => _mapper.Map<NavResult>(src)))
                .ForMember(dest => dest.PolarNativeAdsData, opt => opt.MapFrom(src => _polarNativeAdsMapper.Map(src)));
        }
    }
}