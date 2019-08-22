using System;
using System.Collections.Generic;
using Parser.Parsing;

namespace Parser.Parsing
{
    sealed internal class ParensExpressionNode : ExpressionNode
    {
        internal ExpressionNode Calculation {get;}

        public ParensExpressionNode(ExpressionNode calculation)
        {
            Calculation = calculation;
        }

        internal override string Describe()
        {
            return $"({Calculation.Describe()})";
        }
        
        internal override IEnumerable<ExpressionNode> ContainedNodes()
        {
            yield return Calculation;
        }
    }
}