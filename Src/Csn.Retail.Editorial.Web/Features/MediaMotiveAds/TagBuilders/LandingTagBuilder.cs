using System.Collections.Generic;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Constants;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.Redirects;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders
{
    [AutoBind]
    public class LandingTagBuilder : IMediaMotiveTagBuilder
    {
        private readonly IRequestContextWrapper _requestContextWrapper;

        public LandingTagBuilder(IRequestContextWrapper requestContextWrapper)
        {
            _requestContextWrapper = requestContextWrapper;
        }

        public IEnumerable<MediaMotiveTag> Build(MediaMotiveTagBuildersParams query)
        {
            var mediaMotiveTag = new List<MediaMotiveTag> { 
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.Area, !string.IsNullOrEmpty(query.Make) ? MediaMotiveAreaNames.EditorialBrandHomePage : MediaMotiveAreaNames.EditorialHomePage)
            };

            if (query.Make == null) return mediaMotiveTag;
            mediaMotiveTag.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Make, query.Make.Replace(" ", "")));
            mediaMotiveTag.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Car, query.Make.Replace(" ", ""))); // TO DO when we have a model, we need to include model as part of car tag

            return mediaMotiveTag;
        }

        public bool IsApplicable(MediaMotiveTagBuildersParams query)
        {
            var path = "/editorial/" + (query.Make != null ? query.Make.MakeUrlFriendly().ToLower() + "/" : "");

            return _requestContextWrapper.Url.AbsolutePath.IsSame(path);
        }
    }
}