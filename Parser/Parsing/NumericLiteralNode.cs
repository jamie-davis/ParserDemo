using Parser.LexicalAnalysis;
using Parser.Parsing;

namespace Parser.Parsing
{
    internal class NumericLiteralNode : ExpressionNode
    {
        private decimal _value;

        public NumericLiteralNode(Token token)
        {
            _value = decimal.Parse(token.Text);
        }

        internal override string Describe()
        {
            return _value.ToString();
        }
    }
    internal class ErrorNode : ExpressionNode
    {
        private readonly string _message;
        private readonly string _atData;

        public ErrorNode(string message, string atData)
        {
            _message = message;
            _atData = atData;
        }

        internal override string Describe()
        {
            return $"Error: {_message} at {_atData}";
        }
    }
}