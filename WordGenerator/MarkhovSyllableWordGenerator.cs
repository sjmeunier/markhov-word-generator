using System;
using System.Collections.Generic;
using System.IO;
using WordGenerator.Models;
using System.Linq;

namespace WordGenerator
{
    public class MarkhovSyllableWordGenerator
    {
        private Dictionary<string, ParentSyllable> _syllables = new Dictionary<string, ParentSyllable>();
        private Random _random;

        public MarkhovSyllableWordGenerator(string filename)
        {
            _random = new Random(Environment.TickCount);
            ImportWordData(filename);
        }

        public bool ImportWordData(string filename)
        {
            _syllables = new Dictionary<string, ParentSyllable>();

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
                var syllables = word.Split('/');

                for (int i = 0; i < syllables.Length; i++)
                {
                    var currentSyllable = syllables[i];
                    var currentChildSyllable = "~";

                    if (i < syllables.Length - 1)
                    {
                        currentChildSyllable = syllables[i + 1];
                    }

                    ParentSyllable parentSyllable;

                    if (_syllables.ContainsKey(currentSyllable))
                    {
                        parentSyllable = _syllables[currentSyllable];
                        parentSyllable.Occurrences++;
                    }
                    else
                    {
                        parentSyllable = new ParentSyllable()
                        {
                            Syllable = currentSyllable
                        };
                    }

                    ChildSyllable childSyllable;

                    if (i == 0)
                        parentSyllable.CanBeRoot = true;

                    parentSyllable.Occurrences++;
                    if (parentSyllable.Children.ContainsKey(currentChildSyllable))
                    {
                        childSyllable = parentSyllable.Children[currentChildSyllable];
                    }
                    else
                    {
                        childSyllable = new ChildSyllable() {
                            Syllable = currentChildSyllable
                        };
                    }

                    childSyllable.Occurrences++;
                    parentSyllable.Children[currentChildSyllable] = childSyllable;

                    _syllables[currentSyllable] = parentSyllable;
                }
            }
            return true;
        }

        public string GenerateWord(int minLength, int randomSeed = 0)
        {
            if (randomSeed != 0)
                _random = new Random(randomSeed);

            if (!_syllables.Any())
            {
                return "";
            }

            int index = _random.Next(0, _syllables.Where(x => x.Value.CanBeRoot == true).Count());

            ParentSyllable syllable = _syllables.ElementAt(index).Value;
            string generatedWord = "";
            while (generatedWord.Length < minLength)
            {
                generatedWord = GenerateNextSyllable(syllable);
            }
            return generatedWord;
        }

        private string GenerateNextSyllable(ParentSyllable syllable)
        {
            string generatedString = syllable.Syllable;

            double randomChance = _random.NextDouble();
            double cumulativePercentage = 0;
            foreach (ChildSyllable childSyllable in syllable.Children.Values)
            {
                cumulativePercentage += (double)childSyllable.Occurrences / (double)syllable.Occurrences;
                if (randomChance <= cumulativePercentage)
                {
                    if (childSyllable.Syllable != "~")
                    {
                        generatedString += GenerateNextSyllable(_syllables[childSyllable.Syllable]);
                    }
                    break;
                }
            }
            return generatedString;
        }
    }


}