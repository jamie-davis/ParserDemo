using System.Collections.Generic;

namespace Parser.Parsing
{
    internal class ErrorNode : ExpressionNode
    {
        private readonly string _message;
        private readonly string _atData;

        public ErrorNode(string message, string atData)
        {
            _message = message;
            _atData = atData;
        }

        internal override IEnumerable<ExpressionNode> ContainedNodes()
        {
            yield break;
        }

        internal override string Describe()
        {
            return $"{_message} at {_atData}";
        }
    }
}