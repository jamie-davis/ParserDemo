using System.Collections.Generic;
using Parser.LexicalAnalysis;
using Parser.Parsing;
using Xunit;
using FluentAssertions;
using System.Linq;

namespace ParserTests.Parsing
{
    public class TestTokenKeeper
    {
        private const string TestExpression = "(5 * 7) + 145";
        private List<Token> _tokens;
        private TokenKeeper _keeper;

        public TestTokenKeeper()
        {
            _tokens = LexicalAnalyser.ExtractTokens(TestExpression).ToList();
            _keeper = new TokenKeeper(_tokens);
        }

        [Fact]
        public void IsNextReturnsTrueIfCorrectTokenTypeIsNext()
        {
            //Act
            var result = _keeper.IsNext(TokenType.OpenParens);
            
            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsNextReturnsFalseIfCorrectTokenTypeIsNotNext()
        {
            //Act
            var result = _keeper.IsNext(TokenType.NumericLiteral);
            
            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void TakeReturnsTheFirstToken()
        {
            //Act
            var result = _keeper.Take();
            
            //Assert
            result.TokenType.Should().Be(TokenType.OpenParens);
        }

        [Fact]
        public void TakeMovesThePositionInTheStream()
        {
            //Arrange
            _keeper.Take();

            //Assert
            _keeper.IsNext(TokenType.NumericLiteral).Should().BeTrue();
        }

        [Fact]
        public void SwapTakesPositionFromOtherTokenKeeper()
        {         
            //Arrange
            var other = new TokenKeeper(_keeper);
            other.Take();

            //Act
            _keeper.Swap(other);

            //Assert
            _keeper.IsNext(TokenType.NumericLiteral).Should().BeTrue();
        }

        [Fact]
        public void SwapExchangesPositionWithOtherTokenKeeper()
        {         
            //Arrange
            var other = new TokenKeeper(_keeper);
            other.Take();

            //Act
            _keeper.Swap(other);

            //Assert
            other.IsNext(TokenType.OpenParens).Should().BeTrue();
        }

        [Fact]
        public void RemainingDataShouldReturnTheReconstructedExpression()
        {         
            //Act
            var remaining = _keeper.RemainingData();

            //Assert
            remaining.Should().Be(string.Join(" ", _tokens.Select(t => t.Text)));
        }

        [Fact]
        public void RemainingDataShouldReturnTheUnusedExpression()
        {         
            //Arrange
            _keeper.Take();
            _keeper.Take();

            //Act
            var remaining = _keeper.RemainingData();

            //Assert
            remaining.Should().Be(string.Join(" ", _tokens.Skip(2).Select(t => t.Text)));
        }

        [Fact]
        public void RemainingDataShouldReturnEmptyStringWhenExpressionFullyUsed()
        {         
            //Arrange
            while (!_keeper.Finished)
                _keeper.Take();

            //Act
            var remaining = _keeper.RemainingData();

            //Assert
            remaining.Should().Be(string.Empty);
        }

        [Fact]
        public void DiscardAllShouldConsumeAllTokens()
        {         
            //Act
            _keeper.DiscardAll();

            //Assert
            _keeper.Finished.Should().BeTrue();
        }

        [Fact]
        public void DiscardWhileShouldConsumeAllMatchingTokens()
        {
            //Arrange
            var original = new TokenKeeper(_keeper);

            //Act
            _keeper.DiscardWhile(TokenType.OpenParens, TokenType.NumericLiteral, TokenType.Operator);

            //Assert
            while (original.Next.TokenType != TokenType.CloseParens)
                original.Take();

            var remaining = _keeper.RemainingData();
            remaining.Should().Be(original.RemainingData());
        }

        [Fact]
        public void DiscardWhileShouldConsumeToEnd()
        {
            //Act
            _keeper.DiscardWhile(TokenType.OpenParens, TokenType.NumericLiteral, TokenType.Operator, TokenType.CloseParens);

            //Assert
            _keeper.Finished.Should().BeTrue();
        }
    }
}