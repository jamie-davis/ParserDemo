using System.Collections.Generic;
using Parser.LexicalAnalysis;
using Parser.Parsing;

namespace Parser.Parsing
{
    internal class OperatorExpNode : ExpressionNode
    {
        private ExpressionNode _left;
        private OperatorToken _op;
        private ExpressionNode _right;

        public OperatorExpNode(ExpressionNode left, OperatorToken op, ExpressionNode right)
        {
            _left = left;
            _op = op;
            _right = right;
        }

        internal override string Describe()
        {
            return $"{_op.Text}[{_left.Describe()}, {_right.Describe()}]";
        }

        internal override IEnumerable<ExpressionNode> ContainedNodes()
        {
            yield return _left;
            yield return _right;
        }
    }
}