using System;
using System.Collections;
using System.Collections.Generic;

namespace WordGenerator.Models
{
    public class ParentCharacter
    {
        public char Character { get; set; }
        public bool CanBeRoot { get; set; }
        public int Occurrences { get; set; }
        public Dictionary<char, ChildCharacter> Children { get; set; }

        public ParentCharacter()
        {
            Children = new Dictionary<char, ChildCharacter>();
        }
    }
}