using System;
using System.Collections;
using System.Collections.Generic;

namespace WordGenerator.Models
{
    public class ParentSyllable
    {
        public string Syllable { get; set; }
        public bool CanBeRoot { get; set; }
        public int Occurrences { get; set; }
        public Dictionary<string, ChildSyllable> Children { get; set; }

        public ParentSyllable()
        {
            Children = new Dictionary<string, ChildSyllable>();
        }
    }
}