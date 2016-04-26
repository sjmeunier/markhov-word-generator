using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace MarkovChains
{
    public class WordGenPhrase
    {
        public struct RootPhrase
        {
            public String sPhrase;
            public int iChildCount;
            public int iOccurrences;
            public Hashtable Children;
        }
        public struct ChildPhrase
        {
            public int iOccurrences;
            public String sPhrase;
        }
        public Hashtable aPhrases = new Hashtable(200);
        public String[] sInitialPhrases = new String[10000];
        public int iMaxPhrases;

        public bool LoadFromFile(String sFilename)
        {
            String[] sWordList = new String[10000];
            int iWordCount = 0;
            try
            {
                StreamReader oSReader = new StreamReader(sFilename);
                while (!oSReader.EndOfStream)
                {
                    sWordList[iWordCount] = oSReader.ReadLine();
                    iWordCount++;
                }
                oSReader.Close();
                Load(sWordList, iWordCount);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public bool Load(String[] sWordArray, int iWordCount)
        {
            int iNumWords = iWordCount;
            iMaxPhrases = 0;
            aPhrases = new Hashtable(200);
            RootPhrase oRPhrase = new RootPhrase();
            ChildPhrase oChildPhrase = new ChildPhrase();
            String sCurPhrase;
            String sCurChildPhrase;
            String[] sSyllables;

            for (int i = 0; i < iNumWords; i++)
            {
                sSyllables = sWordArray[i].Split('/');
                for (int j = 0; j < sSyllables.Length; j++)
                {
                    sCurPhrase = sSyllables[j];
                    if (j < sSyllables.Length - 1)
                    {
                        sCurChildPhrase = sSyllables[j + 1];
                    }
                    else
                    {
                        sCurChildPhrase = "~";
                    }
                    if (aPhrases.ContainsKey(sCurPhrase))
                    {
                        oRPhrase = (RootPhrase)aPhrases[sCurPhrase];
                        oRPhrase.iOccurrences++;
                        if (oRPhrase.Children.ContainsKey(sCurChildPhrase))
                        {
                            oChildPhrase = (ChildPhrase)oRPhrase.Children[sCurChildPhrase];
                            oChildPhrase.iOccurrences++;
                            oRPhrase.Children.Remove(sCurChildPhrase);
                            oRPhrase.Children.Add(sCurChildPhrase, oChildPhrase);
                        }
                        else
                        {
                            oChildPhrase = new ChildPhrase();
                            oChildPhrase.sPhrase = sCurChildPhrase;
                            oChildPhrase.iOccurrences = 1;
                            oRPhrase.Children.Add(sCurChildPhrase, oChildPhrase);
                            oRPhrase.iChildCount++;
                            
                        }
                        aPhrases.Remove(sCurPhrase);
                        aPhrases.Add(sCurPhrase, oRPhrase);
                    }
                    else
                    {
                        oRPhrase = new RootPhrase();
                        oRPhrase.iChildCount = 1;
                        oRPhrase.sPhrase = sCurPhrase;
                        oRPhrase.iOccurrences = 1;
                        oRPhrase.Children = new Hashtable();
                        oChildPhrase = new ChildPhrase();
                        oChildPhrase.sPhrase = sCurChildPhrase;
                        oChildPhrase.iOccurrences = 1;
                        
                        oRPhrase.Children.Add(sCurChildPhrase, oChildPhrase);

                        aPhrases.Add(sCurPhrase, oRPhrase);

//                        if (j == 0)
//                        {
                            sInitialPhrases[iMaxPhrases] = sCurPhrase;
                            iMaxPhrases++;
//                        }
                    }   
                }
    

            }
            return true;
        }

        public String GenerateWord()
        {
            String sGen = "";
            bool bLoop = true;
            RootPhrase oRPhrase = new RootPhrase();
            ChildPhrase oChildPhrase = new ChildPhrase();
            double dRandNum = 0;
            Random oRand = new Random(Environment.TickCount);
            double dCumulativePerc = 0;
            if (aPhrases.Count == 0)
            {
                return "";
            }

            int iStartItem = oRand.Next(0, iMaxPhrases);
            oRPhrase = (RootPhrase)aPhrases[sInitialPhrases[iStartItem]];
            sGen = oRPhrase.sPhrase;
            while (bLoop)
            {
                dRandNum = oRand.NextDouble();
                dCumulativePerc = 0;
                foreach (object x in oRPhrase.Children)
                {
                    oChildPhrase = (ChildPhrase)oRPhrase.Children[((System.Collections.DictionaryEntry)x).Key];
                    dCumulativePerc += (double)oChildPhrase.iOccurrences / (double)oRPhrase.iOccurrences;
                    if (dRandNum <= dCumulativePerc)
                    {
                        if (oChildPhrase.sPhrase == "~")
                        {
                            bLoop = false;
                        }
                        else
                        {
                            sGen += oChildPhrase.sPhrase;
                            oRPhrase = (RootPhrase)aPhrases[oChildPhrase.sPhrase];
                        }
                        break;
                    }
                }
            }
            return sGen;
        }
    }

    
}
