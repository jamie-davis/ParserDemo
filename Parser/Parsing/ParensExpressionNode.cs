using System;
using Parser.Parsing;

namespace Parser.Parsing
{
    internal class ParensExpressionNode : ExpressionNode
    {
        private ExpressionNode _calc;

        public ParensExpressionNode(ExpressionNode calc)
        {
            _calc = calc;
        }

        internal override string Describe()
        {
            return $"({_calc.Describe()})";
        }
    }
}