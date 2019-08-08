namespace Parser.LexicalAnalysis
{
    internal class OperatorToken : Token
    {
        public OperatorToken(string op)
        {
            Text = op.ToString();
        }

        public override TokenType TokenType => TokenType.Operator;
    }
}