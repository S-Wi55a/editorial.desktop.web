namespace Csn.Retail.Editorial.Web.Features.Redirects
{
    public class RedirectRule
    {
        public string MatchUrl { get; set; }
        public string RedirectTo { get; set; }
        public bool IncludeQueryStringInRedirect { get; set; }
    }
}