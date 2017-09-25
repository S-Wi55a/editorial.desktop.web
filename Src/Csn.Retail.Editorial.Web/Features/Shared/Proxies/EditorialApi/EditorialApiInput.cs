namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi
{
    public class EditorialApiInput
    {
        public string ServiceName { get; set; }
        public string ViewType { get; set; }
        public string Id { get; set; }
        public bool IsPreview { get; set; }
    }
}