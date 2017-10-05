using System;
using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders
{
    [AutoBind]
    public class ListingsTagBuilder : IMediaMotiveTagBuilder
    {
        public IEnumerable<MediaMotiveTag> Build(MediaMotiveAdQuery query)
        {
            throw new NotImplementedException();
        }

        public bool IsMatch(MediaMotiveAdQuery query)
        {
            throw new NotImplementedException();
        }
    }
}