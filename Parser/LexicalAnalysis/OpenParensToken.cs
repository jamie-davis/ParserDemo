namespace Parser.LexicalAnalysis
{
    internal class OpenParensToken : Token
    {
        public OpenParensToken()
        {
            Text = "(";
        }

        public override TokenType TokenType => TokenType.OpenParens;
    }   
}