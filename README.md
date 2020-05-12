# word-generator
A word generator using Markhov chaining, written in C#

The application can generate random names based on a source file. The more data in the source file, the better results. 

This is a complete rewrite of my old Markhov chaingin project

There are two versions of the generator:
- **Markhov chaining using syllables**. This splits the source file based on syllables, using a forward slash as a seperator. E.G. pe/ter, wi/li/am.
- **Markhov chaining using characters**. This generates words based on the characters themselves. This tends to generate more impossible words than the syllable version, but has a greater variation in results.

