using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    [AutoBind]
    public class MappingSetupTask : IMappingSetupTask
    {
        private readonly IContributorMapper _contributorMapper;
        private readonly IHeroSectionMapper _heroSectionMapper;

        public MappingSetupTask(IContributorMapper contributorMapper, IHeroSectionMapper heroSectionMapper)
        {
            _contributorMapper = contributorMapper;
            _heroSectionMapper = heroSectionMapper;
        }
        public void Run(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ArticleDetailsDto, ArticleViewModel>()
                .ForMember(dest => dest.Contributors, opt => opt.MapFrom(src => _contributorMapper.Map(src)))
                .ForMember(dest => dest.HeroSection, opt => opt.MapFrom(src => _heroSectionMapper.Map(src)));
        }
    }
}