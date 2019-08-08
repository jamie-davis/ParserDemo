namespace Parser.LexicalAnalysis
{
    public abstract class Token
    {
        public abstract TokenType TokenType { get; }
        public string Text {get; protected set;}
    }
}
