using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders
{
    public interface IMediaMotiveTagBuilder
    {
        IEnumerable<MediaMotiveTag> Build(MediaMotiveTagBuildersParams query);

        bool IsApplicable(MediaMotiveTagBuildersParams query);
    }
}