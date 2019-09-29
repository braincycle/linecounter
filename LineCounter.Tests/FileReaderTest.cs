using Linecounter;
using Linecounter.Logger.Abstract;
using Moq;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace LineCounter.Tests
{
    public class FileReaderTest
    {
        private Mock<IDictionary<int, string>> _defectiveStrings;
        private Mock<IDictionary<int, string>> _matchingStrings;
        private Mock<IFileLogger> _fileLogger;
        private readonly NumberStyles _style = NumberStyles.AllowDecimalPoint;
        private FileReader _fileReader;

        public FileReaderTest()
        {
            _matchingStrings = new Mock<IDictionary<int, string>>();
            _defectiveStrings = new Mock<IDictionary<int, string>>();
            _fileLogger = new Mock<IFileLogger>();
            _fileReader = new FileReader(_fileLogger.Object);
        }

        [Fact]
        public void FileReaderCreated()
        {
            Assert.NotNull(_matchingStrings);
            Assert.NotNull(_defectiveStrings);
            Assert.NotNull(_fileReader);
        }

        [Fact]
        public void MatchCollectionIsNotNull()
        {
            Assert.NotNull(_matchingStrings);
        }

        [Fact]
        public void DefectiveCollectionIsNotNull()
        {
            Assert.NotNull(_defectiveStrings);
        }

        [Theory]
        [InlineData("2.0,1,1.2")]
        public void StrConvertTest(string str)
        {
            //Arrange
            var expectedValue = 4.2;

            //Act
            var sum = _fileReader.StrConvert(str);

            //Assert
            Assert.IsType<double>(sum);
            Assert.Equal(expectedValue,sum);
        }

        [Theory]
        [InlineData("2.0",true)]
        [InlineData("abcd",false)]
        public void IsNumberTest(string word, bool expected)
        {
            //Act
            var isNumber = _fileReader.IsNumber(word);

            //Assert
            Assert.Equal(expected, isNumber);
            Assert.IsType<bool>(isNumber);
        }


        [Theory]
        [InlineData(new string[] {"1.0","2.0","100"},true)]
        [InlineData(new string[] {"1.0","abc","2","100"},false)]
        public void isMatchStr(string[] words, bool expected)
        {
            var isMatchStr = _fileReader.IsMatchStr(words);

            Assert.Equal(expected, isMatchStr);
            Assert.IsType<bool>(isMatchStr);
        }
    }
}
