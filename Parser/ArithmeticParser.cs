using System;
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
                var error = AllNodes(calculation).FirstOrDefault(n => n is ErrorNode);
                if (error != null)
                    return new ArithmeticExpression(error.Describe());

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

        private static bool TryTakeMissingTerm(TokenKeeper pos, out ExpressionNode node)
        {
            if (pos.Finished)
            {
                node = new ErrorNode("Missing expression term", string.Empty);
                return true;
            }

            node = null;
            return false;
        }

        private static bool TryTakeCalcExp(TokenKeeper pos, out ExpressionNode node)
        {
            var work = new TokenKeeper(pos);
            if (TryTakeMissingTerm(work, out node)
             || TryTakeMisplacedOperator(work, out node)
             || TryTakeMisplacedCloseParens(work, out node)
             || TryTakeBadOperatorExp(work, out node)
             || TryTakeOperatorExp(work, out node)
             || TryTakeBadParensExp(work, out node)
             || TryTakeParensExp(work, out node)
             )
            {
                pos.Swap(work);
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
                pos.Swap(work);
                node = new OperatorExpNode(left, op, right);
                return true;
            }

            node = null;
            return false;
        }


        private static bool TryTakeBadOperatorExp(TokenKeeper pos, out ExpressionNode node)
        {
            var work = new TokenKeeper(pos);
            if (TryTakeTerm(work, out var left)
             && TryTakeOperator(work, out var op)
             && work.IsNext(TokenType.Operator))
            {
                node = new ErrorNode("Invalid operator expression", pos.RemainingData());
                pos.DiscardWhile(TokenType.Operator, TokenType.NumericLiteral);
                return true;
            }

            node = null;
            return false;
        }

        private static bool TryTakeOperator(TokenKeeper pos, out OperatorToken op)
        {
            if (pos.IsNext(TokenType.Operator))
            {
                op = pos.Take() as OperatorToken;
                return op != null;
            }

            op = null;
            return false;
        }

        private static bool TryTakeTerm(TokenKeeper pos, out ExpressionNode node)
        {
            var work = new TokenKeeper(pos);
            if (TryTakeNumericLiteral(work, out node)
             || TryTakeBadParensExp(work, out node)
             || TryTakeParensExp(work, out node)
             || TryTakeCalcExp(work, out node))
            {
                pos.Swap(work);
                return true;
            }

            node = null;
            return false;
        }

        private static bool TryTakeSecondTerm(TokenKeeper pos, out ExpressionNode node)
        {
            var work = new TokenKeeper(pos);
            if (TryTakeBadParensExp(work, out node)
             || TryTakeParensExp(work, out node)
             || TryTakeBadOperatorExp(work, out node)
             || TryTakeOperatorExp(work, out node)
             || TryTakeNumericLiteral(work, out node))
            {
                pos.Swap(work);
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
                pos.Swap(work);
                node = new ParensExpressionNode(calc);
                return true;
            }

            node = null;
            return false;
        }

        private static bool TryTakeBadParensExp(TokenKeeper pos, out ExpressionNode node)
        {
            var work = new TokenKeeper(pos);
            if (TryTakeUnclosedParensExp(work, out node))
//             || TryTakeNoCalcParensExp(work, out node))
            {
                pos.Swap(work);
                return true;
            }

            node = null;
            return false;
        }


        private static bool TryTakeUnclosedParensExp(TokenKeeper pos, out ExpressionNode node)
        {
            var work = new TokenKeeper(pos);
            if (TryTakeOpenParens(work)
             && TryTakeCalcExp(work, out var calc)
             && work.Finished)
            {
                node = new ErrorNode("Unmatched open parenthesis", pos.RemainingData());
                pos.Swap(work);
                return true;
            }

            node = null;
            return false;
        }


        private static bool TryTakeNoCalcParensExp(TokenKeeper pos, out ExpressionNode node)
        {
            var work = new TokenKeeper(pos);
            if (TryTakeOpenParens(work)
             && work.Finished)
            {
                node = new ErrorNode("Unmatched open parenthesis", pos.RemainingData());
                pos.Swap(work);
                return true;
            }

            node = null;
            return false;
        }

        private static bool TryTakeOpenParens(TokenKeeper pos)
        {
            if (pos.IsNext(TokenType.OpenParens))
            {
                pos.Take();
                return true;
            }

            return false;
        }

        private static bool TryTakeCloseParens(TokenKeeper pos)
        {
            if (pos.IsNext(TokenType.CloseParens))
            {
                pos.Take();
                return true;
            }

            return false;
        }

        private static bool TryTakeNumericLiteral(TokenKeeper pos, out ExpressionNode node)
        {
            if (pos.IsNext(TokenType.NumericLiteral))
            {
                var token = pos.Take();
                node = new NumericLiteralNode(token);
                return true;
            }

            node = null;
            return false;
        }

        private static bool TryTakeMisplacedOperator(TokenKeeper pos, out ExpressionNode node)
        {
            if (pos.IsNext(TokenType.Operator))
            {
                node = new ErrorNode("Unexpected operator", pos.RemainingData());
                pos.DiscardAll();
                return true;
            }

            node = null;
            return false;
        }

        private static bool TryTakeMisplacedCloseParens(TokenKeeper pos, out ExpressionNode node)
        {
            if (pos.IsNext(TokenType.CloseParens))
            {
                node = new ErrorNode("Unexpected close parens", pos.RemainingData());
                pos.DiscardAll();
                return true;
            }

            node = null;
            return false;
        }
    }
}