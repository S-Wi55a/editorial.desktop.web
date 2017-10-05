using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders
{
    public interface IMediaMotiveTagBuilder
    {
        IEnumerable<MediaMotiveTag> Build(MediaMotiveAdQuery query);

        bool IsMatch(MediaMotiveAdQuery query);
    }
}