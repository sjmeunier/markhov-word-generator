using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace MarkovChains
{
    public class WordGenLetter
    {
        public struct RootLetter
        {
            public char cLetter;
            public int iChildCount;
            public int iOccurrences;
            public Hashtable Children;
        }
        public struct ChildLetter
        {
            public int iOccurrences;
            public char cLetter;
        }
        public Hashtable aLetters = new Hashtable(26);

        public bool Load(String[] sWordArray)
        {
            int iNumWords = sWordArray.GetLength(0);
            aLetters = new Hashtable(26);
            RootLetter oRLetter = new RootLetter();
            ChildLetter oChildLetter = new ChildLetter();
            char cCurChar;
            char cCurChildChar;

            for (int i = 0; i < iNumWords; i++)
            {
                for (int j = 0; j < sWordArray[i].Length; j++)
                {
                    cCurChar = sWordArray[i][j];
                    if (j < sWordArray[i].Length - 1)
                    {
                        cCurChildChar = sWordArray[i][j + 1];
                    }
                    else
                    {
                        cCurChildChar = '~';
                    }
                    if (aLetters.ContainsKey(cCurChar))
                    {
                        oRLetter = (RootLetter)aLetters[cCurChar];
                        oRLetter.iOccurrences++;
                        if (oRLetter.Children.ContainsKey(cCurChildChar))
                        {
                            oChildLetter = (ChildLetter)oRLetter.Children[cCurChildChar];
                            oChildLetter.iOccurrences++;
                            oRLetter.Children.Remove(cCurChildChar);
                            oRLetter.Children.Add(cCurChildChar, oChildLetter);
                        }
                        else
                        {
                            oChildLetter = new ChildLetter();
                            oChildLetter.cLetter = cCurChildChar;
                            oChildLetter.iOccurrences = 1;
                            oRLetter.Children.Add(cCurChildChar, oChildLetter);
                            oRLetter.iChildCount++;
                            
                        }
                        aLetters.Remove(cCurChar);
                        aLetters.Add(cCurChar, oRLetter);
                    }
                    else
                    {
                        oRLetter = new RootLetter();
                        oRLetter.iChildCount = 1;
                        oRLetter.cLetter = cCurChar;
                        oRLetter.iOccurrences = 1;
                        oRLetter.Children = new Hashtable();
                        oChildLetter = new ChildLetter();
                        oChildLetter.cLetter = cCurChildChar;
                        oChildLetter.iOccurrences = 1;
                        
                        oRLetter.Children.Add(cCurChildChar, oChildLetter);

                        aLetters.Add(cCurChar, oRLetter);
                    }   
                }
                cCurChar = sWordArray[i][sWordArray[i].Length - 1];


            }
            return true;
        }

        public String GenerateWord()
        {
            String sGen = "";
            bool bLoop = true;
            RootLetter oRLetter = new RootLetter();
            ChildLetter oChildLetter = new ChildLetter();
            double dRandNum = 0;
            Random oRand = new Random(Environment.TickCount);
            double dCumulativePerc = 0;
            if (aLetters.Count == 0)
            {
                return "";
            }
            int iStartItem = oRand.Next(0, 26 - 1) + 97;
            while (!aLetters.ContainsKey(Convert.ToChar(iStartItem)))
            {
                iStartItem = oRand.Next(0, 26 - 1) + 97;
            }
            oRLetter = (RootLetter)aLetters[Convert.ToChar(iStartItem)];
            sGen = oRLetter.cLetter.ToString();
            while (bLoop)
            {
                dRandNum = oRand.NextDouble();
                dCumulativePerc = 0;
                foreach (object x in oRLetter.Children)
                {
                    oChildLetter = (ChildLetter)oRLetter.Children[((System.Collections.DictionaryEntry)x).Key];
                    dCumulativePerc += (double)oChildLetter.iOccurrences / (double)oRLetter.iOccurrences;
                    if (dRandNum <= dCumulativePerc)
                    {
                        if (oChildLetter.cLetter == '~')
                        {
                            bLoop = false;
                        }
                        else
                        {
                            sGen += oChildLetter.cLetter;
                            oRLetter = (RootLetter)aLetters[oChildLetter.cLetter];
                        }
                        break;
                    }
                }
            }
            return sGen;
        }
    }
}
