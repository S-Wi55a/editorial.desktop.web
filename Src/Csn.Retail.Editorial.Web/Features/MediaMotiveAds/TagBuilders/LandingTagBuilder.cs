using System.Collections.Generic;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
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

        public IEnumerable<MediaMotiveTag> Build(MediaMotiveAdQuery query)
        {
            return new List<MediaMotiveTag>()
            {
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.Area, "homepage")
            };
        }

        public bool IsApplicable(MediaMotiveAdQuery query)
        {
            return _requestContextWrapper.Url.AbsolutePath.IsSame("/editorial/");
        }
    }
}