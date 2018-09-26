using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Constants;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Redirects;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive.TagBuilders
{
    [AutoBind]
    public class LandingTagBuilder : IMediaMotiveTagBuilder
    {
        private readonly IRequestContextWrapper _requestContextWrapper;
        private readonly IPageContextStore _pageContextStore;

        public LandingTagBuilder(IRequestContextWrapper requestContextWrapper, IPageContextStore pageContextStore)
        {
            _requestContextWrapper = requestContextWrapper;
            _pageContextStore = pageContextStore;
        }

        public IEnumerable<MediaMotiveTag> Build(MediaMotiveTagBuildersParams parameters)
        {
            var landingPageContext = _pageContextStore.Get() is LandingPageContext pageContext ? pageContext : null;

            var mediaMotiveTag = new List<MediaMotiveTag> { 
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.Area, !string.IsNullOrEmpty(landingPageContext?.Make) ? MediaMotiveAreaNames.EditorialBrandHomePage : MediaMotiveAreaNames.EditorialHomePage)
            };

            if (landingPageContext?.Make == null) return mediaMotiveTag;
            mediaMotiveTag.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Make, landingPageContext.Make.Replace(" ", "")));
            mediaMotiveTag.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Car, landingPageContext.Make.Replace(" ", ""))); // TO DO when we have a model, we need to include model as part of car tag

            return mediaMotiveTag;
        }

        public bool IsApplicable(MediaMotiveTagBuildersParams parameters)
        {
            return _pageContextStore.Get()?.PageContextType == PageContextTypes.Landing;
        }
    }
}