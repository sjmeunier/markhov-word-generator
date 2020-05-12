using FluentAssertions;
using System;
using WordGenerator;
using Xunit;

namespace WordGeneratorTests
{
    public class MarkhovCharacterWordGeneratorTests
    {
        [Theory]
        [InlineData(1000, 3, "italian-firstnames-character.txt", "imari")]
        [InlineData(1021, 3, "italian-firstnames-character.txt", "relvi")]
        [InlineData(1002, 3, "italian-firstnames-character.txt", "iba")]
        [InlineData(1013, 3, "italian-firstnames-character.txt", "ussi")]
        public void GenerateWordTest(int seed, int minLength, string filename, string expected)
        {
            var wordGenerator = new MarkhovCharacterWordGenerator(filename);
            var result = wordGenerator.GenerateWord(minLength, seed);

            result.Should().Be(expected);
        }
    }
}
