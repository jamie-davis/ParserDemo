using Parser.Parsing;

namespace Parser
{
    public class ArithmeticExpression
    {
        internal ExpressionNode Calculation {get;}

        internal ArithmeticExpression(ExpressionNode calculation)
        {
            Calculation = calculation;
            IsValid = true;
        }
        internal ArithmeticExpression(string errorMessage)
        {
            ErrorMessage = errorMessage;
            IsValid = false;
        }

        public string Describe()
        {
            return Calculation?.Describe();
        }

        public bool IsValid {get;}

        public string ErrorMessage {get;}
    }
}