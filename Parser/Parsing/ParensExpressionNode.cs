using System;
using System.Collections.Generic;
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
        
        internal override IEnumerable<ExpressionNode> ContainedNodes()
        {
            yield return _calc;
        }

    }
}