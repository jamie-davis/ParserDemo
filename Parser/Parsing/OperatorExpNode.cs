using System.Collections.Generic;
using Parser.LexicalAnalysis;
using Parser.Parsing;

namespace Parser.Parsing
{
    internal class OperatorExpNode : ExpressionNode
    {
        private static Dictionary<string, int> OperatorPrecedence = new Dictionary<string, int>
        {
            {"+", 1},
            {"-", 1},
            {"*", 0},
            {"/", 0},
        };

        private ExpressionNode _left;
        private OperatorToken _op;
        private ExpressionNode _right;

        public OperatorExpNode(ExpressionNode left, OperatorToken op, ExpressionNode right)
        {
            _left = left;
            _op = op;
            _right = right;

            if (_right is OperatorExpNode rightOpExp)
            {
                var rightPrecedence = OperatorPrecedence[rightOpExp._op.Text];
                var leftPrecedence = OperatorPrecedence[_op.Text];
                if (rightPrecedence > leftPrecedence)
                {
                    var myLeft = left;
                    var yourLeft = rightOpExp._left;
                    var yourRight = rightOpExp._right;

                    _right = yourRight;
                    _left = rightOpExp;

                    _op = rightOpExp._op;
                    rightOpExp._op = op;

                    rightOpExp._left = myLeft;
                    rightOpExp._right = yourLeft;
                }
            }
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

        internal override decimal Compute()
        {
            var left = _left.Compute();
            var right = _right.Compute();
            switch (_op.Text)
            {
                case "+":
                    return left + right;

                case "-":
                    return left - right;

                case "*":
                    return left * right;

                case "/":
                    return left / right;

                default:
                    return 0M;
            }
        }
    }
}