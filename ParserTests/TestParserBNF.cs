using Xunit;
using TestConsoleLib;
using TestConsoleLib.Testing;
using TestConsole.OutputFormatting;
using System.Linq;
using Parser;

namespace ParserTests
{
    public class TestParserBNF
    {
        [Fact]
        public void InputIsConverted()
        {
            //Arrange
            var testStrings = new [] {
                "1",
                "32-5",
                "(3 *5)+  49",
                "1 + 2 + 3",
                "145 * ((63/2 + 5) - 16)",
                "2 * 2 * (2 * 2)",
                "1 + +",
                "(1 + 5",
                "1 2",
                "+ 1",
                "((",
                ")",
                "+",
                "",
                "(",
                "145 * ((63/2 + 5) - 16))",
                "14.5 * ((6.3/2.4 + 5.5) - 1.6)",
            };

            //Act
            var results = testStrings
                .Select(s => new { String = s, Result = ArithmeticParserBNF.Parse(s)})
                .ToList();

            //Assert
            var output = new Output();
            var report = results.AsReport(rep => rep
                .AddColumn(r => r.String, cc => cc.Heading("Input"))
                .AddColumn(r => r.Result.IsValid, cc => { })
                .AddColumn(r => r.Result.Describe(), cc => cc.Heading("Result"))
                .AddColumn(r => r.Result.ErrorMessage, cc => {})
            );

            output.FormatTable(report);
            output.Report.Verify();
        }
    }
}