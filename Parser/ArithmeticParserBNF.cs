using System.Collections.Generic;
using System.Linq;
using Parser.LexicalAnalysis;
using Parser.Parsing;

namespace Parser
{
    public static class ArithmeticParserBNF
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

        private static bool TryTakeCalc(TokenKeeper pos, out ExpressionNode node)
        {         
            if (TryTakeCalcExp(pos, out node)
                || TryTakeNumericLiteral(pos, out node))
            {
                return true;
            }

            node = null;
            return false;
        }

        private static bool TryTakeCalcExp(TokenKeeper pos, out ExpressionNode node)
        {
            if (TryTakeOperatorExp(pos, out node)
                || TryTakeParensExp(pos, out node))
            {
                return true;
            }

            node = null;
            return false;
        }

        private static bool TryTakeOperatorExp(TokenKeeper pos, out ExpressionNode node)
        {
            var work = new TokenKeeper(pos);
            if (TryTakeTerm(work, out var left)
             && TryTakeOperator(work, out var op)
             && TryTakeSecondTerm(work, out var right))
            {
                node = new OperatorExpNode(left, op, right);
                pos.Swap(work);
                return true;
            }

            node = null;
            return false;
        }

        private static bool TryTakeTerm(TokenKeeper pos, out ExpressionNode node)
        {         
            if (TryTakeNumericLiteral(pos, out node)
             || TryTakeParensExp(pos, out node))
            {
                return true;
            }

            node = null;
            return false;
        }

        private static bool TryTakeSecondTerm(TokenKeeper pos, out ExpressionNode node)
        {         
            if (TryTakeParensExp(pos, out node)
             || TryTakeOperatorExp(pos, out node)
             || TryTakeNumericLiteral(pos, out node))
            {
                return true;
            }

            node = null;
            return false;
        }

        private static bool TryTakeParensExp(TokenKeeper pos, out ExpressionNode node)
        {
            var work = new TokenKeeper(pos);
            if (TryTakeOpenParens(work)
             && TryTakeCalcExp(work, out var calc)
             && TryTakeCloseParens(work))
            {
                node = new ParensExpressionNode(calc);
                pos.Swap(work);
                return true;
            }

            node = null;
            return false;
        }

        private static bool TryTakeNumericLiteral(TokenKeeper pos, out ExpressionNode node)
        {
            if (pos.Next.TokenType == TokenType.NumericLiteral)
            {
                node = new NumericLiteralNode(pos.Take());
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

        private static bool TryTakeOpenParens(TokenKeeper pos)
        {         
            if (pos.Next.TokenType == TokenType.OpenParens)
            {
                pos.Take();
                return true;
            }

            return false;
        }

        private static bool TryTakeCloseParens(TokenKeeper pos)
        {         
            if (pos.Next.TokenType == TokenType.CloseParens)
            {
                pos.Take();
                return true;
            }

            return false;
        }
    }
}