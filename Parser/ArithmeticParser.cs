using System.Collections.Generic;
using System.Linq;
using Parser.LexicalAnalysis;
using Parser.Parsing;

namespace Parser
{
    public static class ArithmeticParser
    {
        public static ArithmeticExpression Parse(string input)
        {
            var tokens = LexicalAnalyser.ExtractTokens(input ?? string.Empty);
            var pos = new TokenKeeper(tokens);
            if (TryTakeCalc(pos, out var calculation) && pos.Finished)
            {
                return new ArithmeticExpression(calculation);
            }
            else
            {
                var errorMessage = $"Unable to interpret calculation at \"{pos.RemainingData()}\"";
                return new ArithmeticExpression(errorMessage);
            }
        }

        private static IEnumerable<ExpressionNode> AllNodes(ExpressionNode calculation)
        {
            yield return calculation;
            foreach (var node in calculation.ContainedNodes().SelectMany(n => AllNodes(n)))
            {
                yield return node;
            }
        }

        private static bool TryTakeCalc(TokenKeeper pos, out ExpressionNode calculation)
        {
            if (TryTakeOperatorExp(pos, out calculation)
             || TryTakeLiteral(pos, out calculation)
             || TryTakeParensExp(pos, out calculation))
            {
                return true;
            }

            calculation = null;
            return false;
        }

        private static bool TryTakeToken(TokenType tokenType, TokenKeeper pos)
        {
            if (pos.Next.TokenType == tokenType)
            {
                pos.Take();
                return true;
            }

            return false;
       }

        private static bool TryTakeParensExp(TokenKeeper pos, out ExpressionNode node)
        {
            var work = new TokenKeeper(pos);
            if (TryTakeToken(TokenType.OpenParens, work)
             && TryTakeCalc(work, out var calc)
             && TryTakeToken(TokenType.CloseParens, work))
            {
                pos.Swap(work);
                node = new ParensExpressionNode(calc);
                return true;
            }

            node = null;
            return false;
        }

        private static bool TryTakeOperatorExp(TokenKeeper pos, out ExpressionNode node)
        {
            var work = new TokenKeeper(pos);
            if (TryTakeFirstTerm(work, out var left)
             && TryTakeOperator(work, out var op)
             && TryTakeCalc(work, out var right))
            {
                pos.Swap(work);
                node = new OperatorExpNode(left, op, right);
                return true;
            }

            node = null;
            return false;
        }

        private static bool TryTakeFirstTerm(TokenKeeper pos, out ExpressionNode node)
        {
            if (TryTakeLiteral(pos, out node)
             || TryTakeParensExp(pos, out node))
            {
                return true;
            }

            node = null;
            return false;
        }

        private static bool TryTakeOperator(TokenKeeper pos, out OperatorToken op)
        {
            if (pos.Next.TokenType == TokenType.Operator)
            {
                op = pos.Take() as OperatorToken;
                return true;
            }

            op = null;
            return false;
        }

        private static bool TryTakeLiteral(TokenKeeper pos, out ExpressionNode node)
        {
            if (pos.Next.TokenType == TokenType.NumericLiteral)
            {
                var literal = pos.Take();
                node = new NumericLiteralNode(literal);
                return true;
            }

            node = null;
            return false;
        }
    }
}