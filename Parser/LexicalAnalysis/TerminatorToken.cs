namespace Parser.LexicalAnalysis
{
    ///<summary>
    /// This is an invented token used to indicate the end of the input. This is returned by <see cref="TokenKeeper"/> 
    /// when the set of tokens has been exhausted so that the parser does not need to check for a premature end of
    /// input. The "next" token it expects will not be present and a <see cref="TerminatorToken"/> instance will be 
    /// there instead.
    ///</summary>
    internal class TerminatorToken : Token
    {
        public TerminatorToken()
        {
            Text = "Terminator";
        }

        public override TokenType TokenType => TokenType.Terminator;
    }
}