using System.Collections.Generic;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Features.DisplayAds;
using Csn.Retail.Editorial.Web.Features.Shared.Constants;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.Redirects;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders
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
            var pageContext = _pageContextStore.Get().PageContextType == PageContextTypes.Landing
                ? _pageContextStore.Get() as LandingPageContext
                : null;

            var mediaMotiveTag = new List<MediaMotiveTag> { 
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.Area, !string.IsNullOrEmpty(pageContext?.Make) ? MediaMotiveAreaNames.EditorialBrandHomePage : MediaMotiveAreaNames.EditorialHomePage)
            };

            if (pageContext?.Make == null) return mediaMotiveTag;
            mediaMotiveTag.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Make, pageContext.Make.Replace(" ", "")));
            mediaMotiveTag.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Car, pageContext.Make.Replace(" ", ""))); // TO DO when we have a model, we need to include model as part of car tag

            return mediaMotiveTag;
        }

        public bool IsApplicable(MediaMotiveTagBuildersParams parameters)
        {
            var pageContext = _pageContextStore.Get().PageContextType == PageContextTypes.Landing
                ? _pageContextStore.Get() as LandingPageContext
                : null;

            var path = "/editorial/" + (pageContext?.Make != null ? pageContext.Make.MakeUrlFriendly().ToLower() + "/" : "");

            return _requestContextWrapper.Url.AbsolutePath.IsSame(path);
        }
    }
}