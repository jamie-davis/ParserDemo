using System;
using System.Collections.Generic;
using Parser.LexicalAnalysis;
using Parser.Parsing;

namespace Parser.Computation
{
    public static class Computer
    {
        private static Dictionary<string, int> OperatorPrecedence = new Dictionary<string, int>
        {
            {"+", 1},
            {"-", 1},
            {"*", 0},
            {"/", 0},
        };

        #region ComputationNodes

        abstract class ComputationNode
        {
            internal abstract decimal Compute();
        }

        sealed class BinaryOperation : ComputationNode
        {
            private readonly OperatorToken _op;
            private readonly ComputationNode _left;
            private readonly ComputationNode _right;

            public BinaryOperation(OperatorToken op, ComputationNode left, ComputationNode right)
            {
                _op = op;
                _left = left;
                _right = right;
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

        sealed class Literal : ComputationNode
        {
            private readonly decimal _value;

            public Literal(decimal value)
            {
                _value = value;
            }

            internal override decimal Compute()
            {
                return _value;
            }
        }

        #endregion

        public static decimal Compute(ArithmeticExpression input)
        {
            var converted = Convert(input.Calculation);
            return converted?.Compute() ?? 0M;
        }

        private static ComputationNode Convert(ExpressionNode node)
        {
            switch (node)
            {
                case OperatorExpNode opNode:
                    return ConvertOperator(opNode);

                case ParensExpressionNode parensNode:
                    return ConvertParensNode(parensNode);

                case NumericLiteralNode numberNode:
                    return new Literal(numberNode.Value);

                default:
                    return null;
            }
        }

        private static ComputationNode ConvertParensNode(ParensExpressionNode parensNode)
        {
            return Convert(parensNode.Calculation);
        }

        private static ComputationNode ConvertOperator(OperatorExpNode opNode)
        {
            if (opNode.Right is OperatorExpNode rightOpExp)
            {
                var rightPrecedence = OperatorPrecedence[rightOpExp.Op.Text];
                var leftPrecedence = OperatorPrecedence[opNode.Op.Text];
                if (rightPrecedence > leftPrecedence)
                {
                    var myLeft = opNode.Left;
                    var yourLeft = rightOpExp.Left;
                    var yourRight = rightOpExp.Right;

                    var newRight = new OperatorExpNode(myLeft, opNode.Op, yourLeft);
                    var newOpExp = new OperatorExpNode(yourRight, rightOpExp.Op, newRight);
                    var desc = newOpExp.Describe();
                    return Convert(newOpExp);
                }
            }

            return new BinaryOperation(opNode.Op, Convert(opNode.Left), Convert(opNode.Right));
        }
    }
}