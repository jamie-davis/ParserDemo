namespace Parser.LexicalAnalysis
{
    internal class ErrorToken : Token
    {
        public override TokenType TokenType => TokenType.Error;

        public string ErrorMessage { get; }

        public ErrorToken(StringKeeper pos, string errorMessage)
        {
            Text = pos.TakeAll();
            ErrorMessage = errorMessage;
        }

        public ErrorToken(string atData, string errorMessage)
        {
            Text = atData;
            ErrorMessage = errorMessage;
        }
    }
}