﻿using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public interface IArticleUrlMapper
    {
        string MapDetailsUrl(SearchResultDto source);
    }

    [AutoBind]
    public class ArticleUrlMapper : IArticleUrlMapper
    {
        private readonly IEditorialRouteSettings _settings;

        public ArticleUrlMapper(IEditorialRouteSettings settings)
        {
            _settings = settings;
        }

        public string MapDetailsUrl(SearchResultDto source)
        {
            if (!string.IsNullOrEmpty(source.DetailsPageUrlPath)) return source.DetailsPageUrlPath;

            return $"/editorial/details/{source.GetSlug()}/";
        }
    }
}