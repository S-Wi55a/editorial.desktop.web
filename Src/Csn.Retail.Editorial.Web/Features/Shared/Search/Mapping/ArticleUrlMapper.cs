using Bolt.Common.Extensions;
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
        private readonly IEditorialSettings _settings;

        public ArticleUrlMapper(IEditorialSettings settings)
        {
            _settings = settings;
        }
        public string MapDetailsUrl(SearchResultDto source)
        {
            
            var urlFormat = !string.IsNullOrEmpty(_settings.DetailsUrlFormat) ? _settings.DetailsUrlFormat : "/editorial/details/{0}";

            return $"{urlFormat.FormatWith(source.GetSlug())}/";
        }
    }
}