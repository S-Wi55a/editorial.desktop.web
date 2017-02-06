using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using SocialMetaData = Csn.Retail.Editorial.Web.Features.Details.Models.SocialMetaData;
//using MediaMotiveData = Csn.Retail.Editorial.Web.Features.Details.Models.MediaMotiveData;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    [AutoBind]
    public class MappingSetupTask : IMappingSetupTask
    {
        private readonly IHeroSectionMapper _heroSectionMapper;

        public MappingSetupTask(IHeroSectionMapper heroSectionMapper)
        {
            _heroSectionMapper = heroSectionMapper;
        }
        public void Run(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ArticleDetailsDto, ArticleViewModel>()
                .ForMember(dest => dest.HeroSection, opt => opt.MapFrom(src => _heroSectionMapper.Map(src)));

            // Social Meta Data
            cfg.CreateMap<Shared.Proxies.EditorialApi.SocialMetaData, SocialMetaData>();

            // MediaMotive Data
            //cfg.CreateMap<Shared.Proxies.EditorialApi.MediaMotiveData, MediaMotiveData>();

            // ProCon
            cfg.CreateMap<Shared.Proxies.EditorialApi.ProCon, Models.ProCon>();
            cfg.CreateMap<Shared.Proxies.EditorialApi.ProCon.Con, Models.ProCon.Con>();
            cfg.CreateMap<Shared.Proxies.EditorialApi.ProCon.Pro, Models.ProCon.Pro>();

            // Expert Ratings
            cfg.CreateMap<Shared.Proxies.EditorialApi.EditorialExpertRating, Models.EditorialExpertRating>();
            cfg.CreateMap<Shared.Proxies.EditorialApi.EditorialExpertRating.ExpertItem, Models.EditorialExpertRating.ExpertItem>();

            // Disqus
            cfg.CreateMap<Shared.Proxies.EditorialApi.Disqus, Models.Disqus>();

        }
    }
}