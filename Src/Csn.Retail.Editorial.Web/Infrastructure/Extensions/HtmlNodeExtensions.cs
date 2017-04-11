using HtmlAgilityPack;

namespace Csn.Retail.Editorial.Web.Infrastructure.Extensions
{
    public static class HtmlNodeExtensions
    {
        public static string GetFirstPararaphText(this HtmlNode source)
        {
            return GetTextContentOfNode(source).Content;
        }

        private static NodeResult GetTextContentOfNode(HtmlNode node)
        {
            if (node.HasChildNodes)
            {
                var textContent = string.Empty;

                foreach (var childNode in node.ChildNodes)
                {
                    if (childNode.Name == "br")
                    {
                        return new NodeResult()
                        {
                            Continue = false,
                            Content = textContent
                        };
                    }

                    var result = GetTextContentOfNode(childNode);
                    textContent += result.Content;
                    result.Content = textContent;

                    if (!result.Continue)
                    {
                        return result;
                    }
                }

                return new NodeResult()
                {
                    Continue = true,
                    Content = textContent
                };
            }

            return new NodeResult()
            {
                Continue = true,
                Content = node.InnerText
            };
        }

        public class NodeResult
        {
            public string Content { get; set; }
            public bool Continue { get; set; }
        }
    }
}