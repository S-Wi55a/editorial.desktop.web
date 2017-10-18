using Expresso.Expressions;
using Expresso.Syntax;

namespace Csn.Retail.Editorial.Web.Infrastructure.Utils
{

    public class RoseTreeSyntax : IExpressionSyntax
    {
        private readonly IExpressionFormatter _formatter;
        private readonly IExpressionParser _parser;

        public RoseTreeSyntax(IExpressionFormatter formatter, IExpressionParser parser)
        {
            _formatter = formatter;
            _parser = parser;
        }

        public Expression Parse(string query)
        {
            return _parser.Parse(query);
        }

        public string Format(Expression expression)
        {
            return _formatter.Format(expression);
        }
    }
}