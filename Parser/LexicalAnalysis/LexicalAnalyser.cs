using System;
using System.Collections.Generic;

namespace Parser.LexicalAnalysis
{
    public static class LexicalAnalyser
    {
        private const string Operators = @"+-*/";
        
        private const string Terminators = "()" + Operators;
        private static Token OpenParens = new OpenParensToken();
        private static Token CloseParens = new CloseParensToken();

        public static IEnumerable<Token> ExtractTokens(string input)
        {
            var pos = new StringKeeper(input);
            pos.SkipWhiteSpace();

            while (!pos.Finished)
            {
                Token nextToken = null;
                if (TryTakeOperator(pos, out nextToken)
                || TryTakeNumeric(pos, out nextToken)
                || TryTakeParens(pos, out nextToken))
                {
                    yield return nextToken;
                    pos.SkipWhiteSpace();
                    continue;
                }
                
                yield return new ErrorToken(pos, "Invalid token");
                break;
            }
        }

        private static bool TryTakeNumeric(StringKeeper pos, out Token nextToken)
        {
            var work = new StringKeeper(pos);
            var number = string.Empty;
            while (!work.Finished && work.NextIn("0123456789."))
            {
                number += work.Take();
            }

            if (number == string.Empty)
            {
                nextToken = null;
                return false;
            }

            var errorData = string.Empty;
            while (!work.Finished && !work.WhiteSpaceNext() && !work.NextIn(Terminators))
            {
                errorData += work.Take();
            }

            if (!decimal.TryParse(number, out _) || errorData != string.Empty)
            {
                //we know that this is a badly formed number, so we should return an error token
                nextToken = new ErrorToken(number + errorData, "Invalid number");
                work.Swap(pos);
                return true;
            }

            if (number == string.Empty)
            {
                //if we didn't extract anything, this isn't a number at all.
                nextToken = null;
                return false;
            }

            nextToken = new NumericToken(number);
            work.Swap(pos);
            return true;
        }

        private static bool TryTakeOperator(StringKeeper pos, out Token nextToken)
        {
            if (pos.NextIn(Operators))
            {
                var op = pos.Take();
                nextToken = new OperatorToken(op);
                return true;
            }

            nextToken = null;
            return false;
        }
        
        private static bool TryTakeParens(StringKeeper pos, out Token nextToken)
        {
            if (pos.Next == '(')
            {
                var op = pos.Take();
                nextToken = OpenParens;
                return true;
            }
            else if (pos.Next == ')')
            {
                var op = pos.Take();
                nextToken = CloseParens;
                return true;
            }

            nextToken = null;
            return false;
        }
    }
}
