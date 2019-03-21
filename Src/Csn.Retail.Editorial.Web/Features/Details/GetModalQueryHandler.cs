using System.Net;
using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    public class GetModalQuery : IQuery
    {
        public string Id { get; set; }
        public string Source { get; set; }
    }

    [AutoBind]
    public class GetModalQueryHandler : IAsyncQueryHandler<GetModalQuery, GetArticleResponse>
    {
        private readonly IAsyncQueryHandler<GetArticleQuery, GetArticleResponse> _articleQueryHandler;

        public GetModalQueryHandler(IAsyncQueryHandler<GetArticleQuery, GetArticleResponse> articleQueryHandler)
        {
            _articleQueryHandler = articleQueryHandler;
        }

        public async Task<GetArticleResponse> HandleAsync(GetModalQuery query)
        {
            var detailsResponse = await _articleQueryHandler.HandleAsync(new GetArticleQuery{ Id = query.Id, IsPreview = false });

            if (detailsResponse.HttpStatusCode != HttpStatusCode.OK || detailsResponse.ArticleViewModel == null) return detailsResponse;

            var viewModel = detailsResponse.ArticleViewModel;

            // we need to do this to remove certain modal specific features
            viewModel.SocialMetaData = null;
            viewModel.StockListingData = null;
            viewModel.MoreArticleData = null;
            viewModel.SpecDataGetVariantsUrl = null;
            viewModel.DisqusData = null;
            viewModel.PolarNativeAdsData = null;
            viewModel.SeoSchemaData = null;
            viewModel.SeoData = null;

            foreach (var contributor in viewModel.Contributors)
            {
                contributor.LinkUrl = null;
            }

            if (viewModel.InsightsData.MetaData.ContainsKey("displayType"))
            {
                viewModel.InsightsData.MetaData["displayType"] = "modal";
            }
            else
            {
                viewModel.InsightsData.MetaData.Add("displayType", "modal");
            }

            if (viewModel.InsightsData.MetaData.ContainsKey("source"))
            {
                viewModel.InsightsData.MetaData["source"] = query.Source;
            }
            else
            {
                viewModel.InsightsData.MetaData.Add("source", query.Source);
            }

            return detailsResponse;
        }
    }
}