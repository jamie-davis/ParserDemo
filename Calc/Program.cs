using System;
using Parser;
using Parser.Computation;

namespace Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            var calc = string.Join(" ", args);
            var parse = ArithmeticParser.Parse(calc);
            if (!parse.IsValid)
            {
                Console.Error.WriteLine("Invalid input:");
                Console.Error.WriteLine(parse.ErrorMessage);
                Environment.ExitCode = -1000;
                return;
            }

            var answer = Computer.Compute(parse);
            Console.WriteLine($"{calc} = {answer}");
        }
    }
}
