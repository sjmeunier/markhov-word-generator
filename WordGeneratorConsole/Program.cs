using System;
using WordGenerator;

namespace WordGeneratorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MarkhovSyllableWordGenerator syllableWordGenerator = new MarkhovSyllableWordGenerator("italian-firstnames-syllable.txt");

            Console.WriteLine(syllableWordGenerator.GenerateWord(3));
            Console.WriteLine(syllableWordGenerator.GenerateWord(3));
            Console.WriteLine(syllableWordGenerator.GenerateWord(3));
            Console.WriteLine(syllableWordGenerator.GenerateWord(3));
            Console.WriteLine(syllableWordGenerator.GenerateWord(3));

            MarkhovCharacterWordGenerator characterWordGenerator = new MarkhovCharacterWordGenerator("italian-firstnames-character.txt");
            Console.WriteLine();
            Console.WriteLine(characterWordGenerator.GenerateWord(3));
            Console.WriteLine(characterWordGenerator.GenerateWord(3));
            Console.WriteLine(characterWordGenerator.GenerateWord(3));
            Console.WriteLine(characterWordGenerator.GenerateWord(3));
            Console.WriteLine(characterWordGenerator.GenerateWord(3));
        }
    }
}
