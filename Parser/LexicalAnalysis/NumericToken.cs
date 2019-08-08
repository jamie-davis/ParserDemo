namespace Parser.LexicalAnalysis
{
    internal class NumericToken : Token
    {
        public NumericToken(string number)
        {
            Text = number;
        }

        public override TokenType TokenType => TokenType.NumericLiteral;
    }
}