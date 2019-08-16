using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parser.LexicalAnalysis;

namespace Parser.Parsing
{
    internal sealed class TokenKeeper
    {
        private static TerminatorToken Terminator {get;} = new TerminatorToken();

        private List<Token> _tokens;
        private int _pos;

        internal bool Finished => _pos >= _tokens.Count;
        internal Token Next => Finished ? Terminator : _tokens[_pos];

        public TokenKeeper(IEnumerable<Token> tokenStream)
        {
            _tokens = tokenStream.ToList();
            _pos = 0;
        }

        public TokenKeeper(TokenKeeper other)
        {
            _tokens = other._tokens;
            _pos = other._pos;
        }

        internal bool IsNext(TokenType tokenType)
        {
            return Next.TokenType == tokenType;
        }

        internal Token Take()
        {
            var next = Next;
            if (!Finished)
                ++_pos;

            return next;
        }

        internal void Swap(TokenKeeper other)
        {
            var ourTokens = _tokens;
            var ourPos = _pos;

            _tokens = other._tokens;
            _pos = other._pos;

            other._tokens = ourTokens;
            other._pos = ourPos;
        }

        internal string RemainingData()
        {
            if (Finished) return string.Empty;

            return string.Join(" ", _tokens.Skip(_pos).Select(s => s.Text));
        }

        internal void DiscardAll()
        {
            _pos = _tokens.Count;
        }

        internal void DiscardWhile(params TokenType[] matches)
        {
            while (!Finished && matches.Contains(Next.TokenType))
                Take();
        }
    }
}