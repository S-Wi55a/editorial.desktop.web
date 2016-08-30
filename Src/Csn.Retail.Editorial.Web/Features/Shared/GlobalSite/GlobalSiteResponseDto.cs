namespace Csn.Retail.Editorial.Web.Features.Shared.GlobalSite
{
    public class GlobalSiteResponseDto
    {
        public GlobalSiteDataDto Data { get; set; }
    }

    public class GlobalSiteDataDto
    {
        public static GlobalSiteDataDto Empty = new GlobalSiteDataDto
        {
            Footer = string.Empty,
            Script = string.Empty,
            Style = string.Empty,
            TopNav = string.Empty
        };

        public string Script { get; set; }
        public string Style { get; set; }
        public string TopNav { get; set; }
        public string Footer { get; set; }
    }
}