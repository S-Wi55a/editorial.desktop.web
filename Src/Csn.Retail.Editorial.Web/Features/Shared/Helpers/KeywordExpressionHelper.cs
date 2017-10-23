using System.Linq;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Expresso.Expressions;
using Expresso.Expressions.Visitors;
using Expresso.Syntax;

namespace Csn.Retail.Editorial.Web.Features.Shared.Helpers
{
    public interface IKeywordExpressionHelper
    {
        string AppendOrUpdate(string query, string keyword);
    }

    [AutoBind]
    public class KeywordExpressionHelper : IKeywordExpressionHelper
    {
        private readonly IExpressionParser _parser;
        private readonly IExpressionFormatter _expressionFormatter;

        public KeywordExpressionHelper(IExpressionParser parser, IExpressionFormatter expressionFormatter)
        {
            _parser = parser;
            _expressionFormatter = expressionFormatter;
        }
        public string AppendOrUpdate(string query, string keyword)
        {
            var currentExpression = _parser.Parse(query);
            var newExpression = string.IsNullOrEmpty(keyword) ? Expression.Create() : new KeywordExpression("Keywords", keyword);

            var visitor = ReplacingVisitorBuilder
                .Find(e => e is KeywordExpression)
                .Substitution(e => newExpression)
                .WhenNotFound(e => e & newExpression)
                .Build();

            return _expressionFormatter.Format(visitor.ReplaceIn(currentExpression));
        }
    }
}