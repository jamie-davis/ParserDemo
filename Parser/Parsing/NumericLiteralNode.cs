using System.Collections.Generic;
using Parser.LexicalAnalysis;
using Parser.Parsing;

namespace Parser.Parsing
{
    internal sealed class NumericLiteralNode : ExpressionNode
    {
        internal decimal Value {get;}

        public NumericLiteralNode(Token token)
        {
            Value = decimal.Parse(token.Text);
        }

        internal override string Describe()
        {
            return Value.ToString();
        }

        internal override IEnumerable<ExpressionNode> ContainedNodes()
        {
            yield break;
        }
    }
}