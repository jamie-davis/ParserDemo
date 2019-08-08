using System;

namespace Parser.LexicalAnalysis
{
    internal class StringKeeper
    {
        private const string WhiteSpaceChars = " \t\r\f\n";

        private string _input;
        private int _position;

        public bool Finished { get { return _input == null || _position >= _input.Length; } }
        public char Next { get { return _input[_position];  } }

        public StringKeeper(string input)
        {
            _input = input;
            _position = 0;
        }

        public StringKeeper(StringKeeper other)
        {
            _input = other._input;
            _position = other._position;
        }

        internal string TakeAll()
        {
            if (Finished)
            {
                return string.Empty;
            }

            var data = _input.Substring(_position);
            _position = _input.Length;
            return data;
        }

        internal void SkipWhiteSpace()
        {
            while (!Finished && WhiteSpaceNext())
                Take();
        }

        internal string Take()
        {
            if (Finished)
                return "\0";

            var result = Next.ToString();
            _position++;
            return result;
        }

        internal bool WhiteSpaceNext()
        {
            return NextIn(WhiteSpaceChars);
        }

        internal bool NextIn(string expectedCharSet)
        {
            return !Finished && expectedCharSet.IndexOf(Next) >= 0;
        }

        internal void Swap(StringKeeper other)
        {
            var otherInput = other._input;
            var otherPosition = other._position;

            other._input = _input;
            other._position = _position;

            _input = otherInput;
            _position = otherPosition;
        }
    }
}