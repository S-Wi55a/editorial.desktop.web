using System.Collections.Generic;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Features.DisplayAds;
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

        public IEnumerable<MediaMotiveTag> Build(MediaMotiveTagBuildersParams parameters)
        {
            var mediaMotiveTag = new List<MediaMotiveTag> { 
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.Area, !string.IsNullOrEmpty(parameters.Make) ? MediaMotiveAreaNames.EditorialBrandHomePage : MediaMotiveAreaNames.EditorialHomePage)
            };

            if (parameters.Make == null) return mediaMotiveTag;
            mediaMotiveTag.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Make, parameters.Make.Replace(" ", "")));
            mediaMotiveTag.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Car, parameters.Make.Replace(" ", ""))); // TO DO when we have a model, we need to include model as part of car tag

            return mediaMotiveTag;
        }

        public bool IsApplicable(MediaMotiveTagBuildersParams parameters)
        {
            var path = "/editorial/" + (parameters.Make != null ? parameters.Make.MakeUrlFriendly().ToLower() + "/" : "");

            return _requestContextWrapper.Url.AbsolutePath.IsSame(path);
        }
    }
}