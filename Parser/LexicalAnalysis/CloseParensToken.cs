namespace Parser.LexicalAnalysis
{
    internal class CloseParensToken : Token
    {
        public CloseParensToken()
        {
            Text = ")";
        }

        public override TokenType TokenType => TokenType.CloseParens;
    }
}