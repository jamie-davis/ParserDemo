using Xunit;
using Parser;
using TestConsoleLib;
using TestConsoleLib.Testing;
using System.Linq;
using Parser.Computation;

namespace ParserTests.Computation
{
    public class TestComputer
    {
        [Fact]
        public void ResultIsComputed()
        {
            //Arrange
            var testStrings = new [] {
                "5 + 3 * 6",
                "5 * 3 + 6",
                "5 * 2 + 3 / 4",
                "5 * (2 + 3) / 4",
            };

            //Act
            var results = testStrings
                .Select(s => new { String = s, Result = ArithmeticParser.Parse(s)})
                .ToList();

            //Assert
            var output = new Output();
            output.FormatTable(results.Select(r => new { r.String,  Description = r.Result.Describe(), Result = Computer.Compute(r.Result)}));
            output.Report.Verify();
        }

    }
}