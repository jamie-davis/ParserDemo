using System.Collections.Generic;
using Parser.LexicalAnalysis;
using Parser.Parsing;

namespace Parser.Parsing
{
    internal sealed class OperatorExpNode : ExpressionNode
    {
        internal ExpressionNode Left {get;}
        internal OperatorToken Op {get;}
        internal ExpressionNode Right {get;}

        public OperatorExpNode(ExpressionNode left, OperatorToken op, ExpressionNode right)
        {
            Left = left;
            Op = op;
            Right = right;
        }

        internal override string Describe()
        {
            return $"{Op.Text}[{Left.Describe()}, {Right.Describe()}]";
        }

        internal override IEnumerable<ExpressionNode> ContainedNodes()
        {
            yield return Left;
            yield return Right;
        }
    }
}