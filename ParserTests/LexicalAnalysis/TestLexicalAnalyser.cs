using Xunit;
using Parser.LexicalAnalysis;
using TestConsoleLib;
using TestConsoleLib.Testing;
using TestConsole.OutputFormatting;

namespace ParserTests.LexicalAnalysis
{
    public class TestLexicalAnalyser
    {
        [Fact]
        public void TokensAreExtracted()
        {
            //Arrange
            var testStrings = new [] {
                "100",
                "100+",
                "100 ",
                "100L",
                "+-*/",
                "()",
                "",
                null,
                "(3 *5)+  49"
            };

            var output = new Output();

            //Act
            foreach (var testString in testStrings)
            {
                var result = LexicalAnalyser.ExtractTokens(testString);

                output.WrapLine($@"Analysis of ""{testString ?? "NULL"}"":");
                output.FormatTable(result.AsReport(rep => rep
                                                    .AddColumn(r => r.TokenType, cc => cc.LeftAlign())
                                                    .AddColumn(r => r.Text, cc => {}))
                );

                output.WriteLine();
                output.WriteLine();
            }

            //Assert
            output.Report.Verify();
        }
    }
}