using Xunit;
using FluentAssertions;
using Parser.LexicalAnalysis;

namespace ParserTests.LexicalAnalysis
{
    public class TestStringKeeper
    {
        [Fact]
        public void NextInReturnsTrueIfFirstCharInRange()
        {
            //Arrange
            var keeper = new StringKeeper("test");

            //Act
            var first = keeper.NextIn("stu");

            //Assert
            first.Should().BeTrue();
        }

        [Fact]
        public void NextInReturnsFalseIfFirstNotCharInRange()
        {
            //Arrange
            var keeper = new StringKeeper("test");

            //Act
            var first = keeper.NextIn("nope");

            //Assert
            first.Should().BeFalse();
        }

        [Fact]
        public void NextInReturnsFalseIfNoInput()
        {
            //Arrange
            var keeper = new StringKeeper("");

            //Act
            var first = keeper.NextIn("nope");

            //Assert
            first.Should().BeFalse();
        }

        [Fact]
        public void FinishedIsFalseWhenInputIsNotEmpty()
        {
            //Arrange
            var keeper = new StringKeeper("a");

            //Act/Assert
            keeper.Finished.Should().BeFalse();
        }

        [Fact]
        public void FinishedIsTrueWhenInputIsEmpty()
        {
            //Arrange
            var keeper = new StringKeeper("");

            //Act/Assert
            keeper.Finished.Should().BeTrue();
        }

        [Fact]
        public void FinishedIsTrueWhenInputIsNull()
        {
            //Arrange
            var keeper = new StringKeeper((string)null);

            //Act/Assert
            keeper.Finished.Should().BeTrue();
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r")]
        [InlineData("\f")]
        [InlineData("\n")]
        public void WhiteSpaceNextReturnsTrueWhenItShould(string whitespace)
        {
            //Arrange
            var keeper = new StringKeeper(whitespace);

            //Act/Assert
            keeper.WhiteSpaceNext().Should().BeTrue();
        }

        [Fact]
        public void WhiteSpaceNextReturnsFalseForNonWhitespace()
        {
            //Arrange
            var keeper = new StringKeeper("non whitespace"); //only 'n' will be considered

            //Act/Assert
            keeper.WhiteSpaceNext().Should().BeFalse();
        }

        [Fact]
        public void WhiteSpaceNextReturnsFalseForEmptyInput()
        {
            //Arrange
            var keeper = new StringKeeper(string.Empty);

            //Act/Assert
            keeper.WhiteSpaceNext().Should().BeFalse();
        }

        [Fact]
        public void TakeReturnsTheNextCharacter()
        {
            //Arrange
            var keeper = new StringKeeper("test");

            //Act
            var result = keeper.Take();

            //Assert
            result.Should().Be("t");
        }

        [Fact]
        public void TakeMovesThePositionAlong()
        {
            //Arrange
            var keeper = new StringKeeper("test");

            //Act
            keeper.Take();
            
            //Assert
            keeper.Next.Should().Be('e');
        }

        [Fact]
        public void TakeReturnsNulWhenNoDataRemains()
        {
            //Arrange
            var keeper = new StringKeeper("test");
            keeper.Take(); //t
            keeper.Take(); //e
            keeper.Take(); //s
            keeper.Take(); //t

            //Act
            var result = keeper.Take();
            
            //Assert
            result.Should().Be("\0");
        }

        [Fact]
        public void FinishedShouldBeTrueWhenNoDataRemains()
        {
            //Arrange
            var keeper = new StringKeeper("test");
            keeper.Take(); //t
            keeper.Take(); //e
            keeper.Take(); //s
            keeper.Take(); //t

            //Act/Assert
            keeper.Finished.Should().BeTrue();
        }

        [Fact]
        public void TakeAllReturnsFullString()
        {
            //Arrange
            var keeper = new StringKeeper("test");

            //Act
            var result = keeper.TakeAll();
            
            //Assert
            result.Should().Be("test");
        }

        [Fact]
        public void TakeAllReturnsRemainingText()
        {
            //Arrange
            var keeper = new StringKeeper("test");
            keeper.Take();

            //Act
            var result = keeper.TakeAll();
            
            //Assert
            result.Should().Be("est");
        }

        [Fact]
        public void FinishedShouldBeTrueAfterTakeAll()
        {
            //Arrange
            var keeper = new StringKeeper("test");

            //Act
            keeper.TakeAll();

            //Assert
            keeper.Finished.Should().BeTrue();
        }

        [Fact]
        public void SkipWhitespaceSetsPositionOnNextNonWhitespace()
        {
            //Arrange
            var keeper = new StringKeeper(" \t\n\r\ftest");

            //Act
            keeper.SkipWhiteSpace();
            
            //Assert
            keeper.Next.Should().Be('t');
        }

        [Fact]
        public void SkipWhitespaceCanSkipToTheEnd()
        {
            //Arrange
            var keeper = new StringKeeper("\n \t\f");

            //Act
            keeper.SkipWhiteSpace();
            
            //Assert
            keeper.Finished.Should().BeTrue();
        }

        [Fact]
        public void SkipWhitespaceWorksIfNoWhitespace()
        {
            //Arrange
            var keeper = new StringKeeper("test");

            //Act
            keeper.SkipWhiteSpace();
            
            //Assert
            keeper.Next.Should().Be('t');
        }

        [Fact]
        public void SkipWhitespaceWorksIfFinished()
        {
            //Arrange
            var keeper = new StringKeeper("test");
            keeper.TakeAll();

            //Act
            keeper.SkipWhiteSpace();
            
            //Assert
            keeper.Finished.Should().BeTrue(); //well, still true anyway
        }

        [Fact]
        public void WhitespaceSkippedDoesNotHaveToBeAtTheStart()
        {
            //Arrange
            var keeper = new StringKeeper("X \t\n\r\ftest");
            keeper.Take();

            //Act
            keeper.SkipWhiteSpace();
            
            //Assert
            keeper.Next.Should().Be('t');
        }

        [Fact]
        public void InstancesCanBeCopied()
        {
            //Arrange
            var keeper = new StringKeeper("test");

            //Act
            var keeper2 = new StringKeeper(keeper);
            
            //Assert
            keeper2.TakeAll().Should().Be(keeper.TakeAll());
        }

        [Fact]
        public void CopiedInstancesShouldHaveTheSamePositionAsTheOriginal()
        {
            //Arrange
            var keeper = new StringKeeper("test");
            keeper.Take();

            //Act
            var keeper2 = new StringKeeper(keeper);
            
            //Assert
            keeper2.TakeAll().Should().Be(keeper.TakeAll());
        }

        [Fact]
        public void CopyingAFinishedInstanceShouldProduceAFinishedInstance()
        {
            //Arrange
            var keeper = new StringKeeper("test");
            keeper.TakeAll();
            
            //Act
            var keeper2 = new StringKeeper(keeper);
            
            //Assert
            keeper2.Finished.Should().BeTrue();
        }

        [Fact]
        public void CopiedInstancesDoNotAlterTheOriginal()
        {
            //Arrange
            var keeper = new StringKeeper("test");
            var keeper2 = new StringKeeper(keeper);

            //Act
            keeper2.TakeAll();

            //Assert
            keeper.Finished.Should().BeFalse();
        }

        [Fact]
        public void CopiedInstancesDoNotFollowTheOriginal()
        {
            //Arrange
            var keeper = new StringKeeper("test");
            var keeper2 = new StringKeeper(keeper);

            //Act
            keeper.TakeAll();

            //Assert
            keeper2.Finished.Should().BeFalse();
        }

        [Fact]
        public void SwapCopiesTheCorrectPositionIntoAnotherInstance()
        {
            //Arrange
            var keeper = new StringKeeper("test");
            var keeper2 = new StringKeeper(keeper);
            keeper.TakeAll();

            //Act
            keeper.Swap(keeper2);

            //Assert
            keeper2.Finished.Should().BeTrue();
        }

        [Fact]
        public void SwapImportsTheDetailsFromTheOtherInstance()
        {
            //Arrange
            var keeper = new StringKeeper("test");
            var keeper2 = new StringKeeper(keeper);
            var takenData = keeper.TakeAll(); //keeper is now finished

            //Act
            keeper.Swap(keeper2);

            //Assert
            var result = keeper.TakeAll(); //if swap did not change keeper, this will return string.Empty
            result.Should().Be(takenData);
        }
    }
}