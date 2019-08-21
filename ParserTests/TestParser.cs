using Xunit;
using Parser;
using TestConsoleLib;
using TestConsoleLib.Testing;
using TestConsole.OutputFormatting;
using System.Linq;

namespace ParserTests
{
    public class TestParser
    {
        [Fact]
        public void InputIsConverted()
        {
            //Arrange
            var testStrings = new [] {
                "1",
                "32-5",
                "(100 * 3)",
                "100 + (3 * 5)",
                "(3 * 5) + 100",
                "1 + 2 + 3",
                "1 + (3 * (5 + 6))",
                "1 +",
                "(1 + 3",
                "(",
                ")",
                "((1 + 2) * 3",
                "(1 + 2) * 3)",
                "5 + (10)"
            };

            //Act
            var results = testStrings
                .Select(s => new { String = s, Result = ArithmeticParser.Parse(s)})
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

        [Fact]
        public void ResultIsComputed()
        {
            //Arrange
            var testStrings = new [] {
                "5 + 3 * 6",
                "5 * 3 + 6",
                "5 * 2 + 3 / 4",
            };

            //Act
            var results = testStrings
                .Select(s => new { String = s, Result = ArithmeticParser.Parse(s)})
                .ToList();

            //Assert
            var output = new Output();
            output.FormatTable(results.Select(r => new { r.String,  Description = r.Result.Describe(), Result = r.Result.Compute()}));
            output.Report.Verify();
        }
    }
}