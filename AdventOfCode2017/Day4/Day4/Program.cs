using System;
using System.Collections.Generic;
using System.IO;

/*

-- Day 4: High-Entropy Passphrases ---

A new system policy has been put in place that requires all accounts to use a passphrase instead of simply a password.
A passphrase consists of a series of words (lowercase letters) separated by spaces.

To ensure security, a valid passphrase must contain no duplicate words.

For example:

    aa bb cc dd ee is valid.
    aa bb cc dd aa is not valid - the word aa appears more than once.
    aa bb cc dd aaa is valid - aa and aaa count as different words.

The system's full passphrase list is available as your puzzle input. How many passphrases are valid?

    Your puzzle answer was 451.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---

For added security, yet another system policy has been put in place. Now, a valid passphrase must contain no two words
that are anagrams of each other - that is, a passphrase is invalid if any word's letters can be rearranged to form any
other word in the passphrase.

For example:

    abcde fghij is a valid passphrase.
    abcde xyz ecdab is not valid - the letters from the third word can be rearranged to form the first word.
    a ab abc abd abf abj is a valid passphrase, because all letters need to be used when forming another word.
    iiii oiii ooii oooi oooo is valid.
    oiii ioii iioi iiio is not valid - any of these words can be rearranged to form any other word.

Under this new system policy, how many passphrases are valid?


    Your puzzle answer was 223.

*/

namespace Day4
{
    class Program
    {
        static bool HasNoDuplicates(string s)
        {
            string[] parts = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length - 1; i++)
            {
                for (int j = i + 1; j < parts.Length; j++)
                {
                    if (string.Compare(parts[i], parts[j]) == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static bool HasNoAnagrams(string s)
        {
            string[] parts = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                char[] characters = parts[i].ToCharArray();
                Array.Sort(characters);
                parts[i] = new string(characters);
            }

            for (int i = 0; i < parts.Length - 1; i++)
            {
                for (int j = i + 1; j < parts.Length; j++)
                {
                    if (string.Compare(parts[i], parts[j]) == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static void Main(string[] args)
        {
            List<string> lines = new List<string>();
            string line = "";
            string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);

            StreamReader file = new StreamReader(filePath + "/input.txt");
            while ((line = file.ReadLine()) != null)
            {
                lines.Add(line);
            }

            int validCount = 0;
            foreach (string s in lines)
            {
                if (HasNoDuplicates(s))
                {
                    validCount++;
                }
            }

            Console.WriteLine("Part 1 asnwer: " + validCount);

            validCount = 0;
            foreach (string s in lines)
            {
                if (HasNoAnagrams(s))
                {
                    validCount++;
                }
            }

            Console.WriteLine("Part 2 answer: " + validCount);

            Console.ReadKey();
        }
    }
}
