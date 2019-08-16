using System;
using Parser.Parsing;

namespace Parser
{
    public class ArithmeticExpression
    {
        private ExpressionNode _calculation;

        internal ArithmeticExpression(ExpressionNode calculation)
        {
            _calculation = calculation;
            IsValid = true;
        }
        internal ArithmeticExpression(string errorMessage)
        {
            ErrorMessage = errorMessage;
            IsValid = false;
        }

        public string Describe()
        {
            return _calculation?.Describe();
        }

        public bool IsValid {get;}

        public string ErrorMessage {get;}    
    }
}