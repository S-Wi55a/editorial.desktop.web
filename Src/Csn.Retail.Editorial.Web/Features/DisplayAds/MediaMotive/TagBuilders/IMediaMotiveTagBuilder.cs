using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive.TagBuilders
{
    public interface IMediaMotiveTagBuilder
    {
        IEnumerable<MediaMotiveTag> Build(MediaMotiveTagBuildersParams parameters);

        bool IsApplicable(MediaMotiveTagBuildersParams parameters);
    }
}