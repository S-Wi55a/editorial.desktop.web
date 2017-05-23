namespace Csn.Retail.Editorial.Web.Infrastructure.Redirects
{
    public class RedirectInstruction
    {
        public string Url { get; set; }
        public bool IsPermanent { get; set; }

        public bool IsRedirectRequired => !string.IsNullOrWhiteSpace(Url);

        public static RedirectInstruction None { get; } = new RedirectInstruction();
    }
}