using Csn.Retail.Editorial.Web.Culture;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public static class NavLabelsMapper
    {
        public static NavLabels Map()
        {
            return new NavLabels
            {
                KeywordsPlaceholderText = LanguageResourceValueProvider.GetValue(LanguageConstants.KeywordSeachPlaceholderText),
                NavShowText = LanguageResourceValueProvider.GetValue(LanguageConstants.NavShowText),
                NavCancelText = LanguageResourceValueProvider.GetValue(LanguageConstants.NavCancelText),
                NavArticleText = LanguageResourceValueProvider.GetValue(LanguageConstants.ArticleText),
                UiCulture = LanguageResourceValueProvider.GetUiCulture()
            };
        }
    }
}