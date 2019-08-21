using System.Collections.Generic;
using Parser.LexicalAnalysis;
using Parser.Parsing;

namespace Parser.Parsing
{
    internal class NumericLiteralNode : ExpressionNode
    {
        private decimal _value;

        public NumericLiteralNode(Token token)
        {
            _value = decimal.Parse(token.Text);
        }

        internal override string Describe()
        {
            return _value.ToString();
        }

        internal override IEnumerable<ExpressionNode> ContainedNodes()
        {
            yield break;
        }

        internal override decimal Compute()
        {
            return _value;
        }
    }
}