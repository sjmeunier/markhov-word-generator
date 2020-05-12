using FluentAssertions;
using System;
using WordGenerator;
using Xunit;

namespace WordGeneratorTests
{
    public class MarkhovSyllableWordGeneratorTests
    {
        [Theory]
        [InlineData(1000, 3, "italian-firstnames-syllable.txt", "medi")]
        [InlineData(1001, 3, "italian-firstnames-syllable.txt", "baldassalomone")]
        [InlineData(1002, 3, "italian-firstnames-syllable.txt", "galice")]
        [InlineData(1003, 3, "italian-firstnames-syllable.txt", "sare")]
        public void GenerateWordTest(int seed, int minLength, string filename, string expected)
        {
            var wordGenerator = new MarkhovSyllableWordGenerator(filename);
            var result = wordGenerator.GenerateWord(minLength, seed);

            result.Should().Be(expected);
        }
    }
}
