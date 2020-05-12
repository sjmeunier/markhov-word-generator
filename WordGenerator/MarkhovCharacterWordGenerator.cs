using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using WordGenerator.Models;
using System.IO;
using System.Linq;

namespace WordGenerator
{
    public class MarkhovCharacterWordGenerator
    {
        public Dictionary<char, ParentCharacter> _characters = new Dictionary<char, ParentCharacter>(26);
        private Random _random;

        public MarkhovCharacterWordGenerator(string filename)
        {
            _random = new Random(Environment.TickCount);
            ImportWordData(filename);
        }

        public bool ImportWordData(string filename)
        {
            _characters = new Dictionary<char, ParentCharacter>();

            List<string> words = new List<string>();

            using (StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    words.Add(reader.ReadLine().Trim().ToLower());
                }
                reader.Close();
            }

            foreach (var word in words)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    var currentCharacter = word[i];
                    var currentChildCharacter = '~';

                    if (i < word.Length - 1)
                    {
                        currentChildCharacter = word[i + 1];
                    }

                    ParentCharacter parentCharacter;

                    if (_characters.ContainsKey(currentCharacter))
                    {
                        parentCharacter = _characters[currentCharacter];
                        parentCharacter.Occurrences++;
                    }
                    else
                    {
                        parentCharacter = new ParentCharacter()
                        {
                            Character = currentCharacter
                        };
                    }

                    ChildCharacter childCharacter;

                    if (i == 0)
                        parentCharacter.CanBeRoot = true;

                    parentCharacter.Occurrences++;
                    if (parentCharacter.Children.ContainsKey(currentChildCharacter))
                    {
                        childCharacter = parentCharacter.Children[currentChildCharacter];
                    }
                    else
                    {
                        childCharacter = new ChildCharacter()
                        {
                            Character = currentChildCharacter
                        };
                    }

                    childCharacter.Occurrences++;
                    parentCharacter.Children[currentChildCharacter] = childCharacter;

                    _characters[currentCharacter] = parentCharacter;
                }
            }
            return true;
        }

        public string GenerateWord(int minLength, int randomSeed = 0)
        {
            if (randomSeed != 0)
                _random = new Random(randomSeed);

            if (!_characters.Any())
            {
                return "";
            }

            int index = _random.Next(0, _characters.Where(x => x.Value.CanBeRoot == true).Count());

            ParentCharacter character = _characters.ElementAt(index).Value;
            string generatedWord = "";
            while (generatedWord.Length < minLength)
            {
                generatedWord = GenerateNextCharacter(character);
            }
            return generatedWord;
        }

        private string GenerateNextCharacter(ParentCharacter character)
        {
            string generatedString = character.Character.ToString();

            double randomChance = _random.NextDouble();
            double cumulativePercentage = 0;
            foreach (ChildCharacter childCharacter in character.Children.Values)
            {
                cumulativePercentage += (double)childCharacter.Occurrences / (double)character.Occurrences;
                if (randomChance <= cumulativePercentage)
                {
                    if (childCharacter.Character != '~')
                    {
                        generatedString += GenerateNextCharacter(_characters[childCharacter.Character]);
                    }
                    break;
                }
            }
            return generatedString;
        }
    }
}