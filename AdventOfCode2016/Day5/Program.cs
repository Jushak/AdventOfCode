using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

/*
--- Day 5: How About a Nice Game of Chess? ---

You are faced with a security door designed by Easter Bunny engineers that seem to have acquired most of their
security knowledge by watching hacking movies.

The eight-character password for the door is generated one character at a time by finding the MD5 hash of some Door ID
(your puzzle input) and an increasing integer index (starting with 0).

A hash indicates the next character in the password if its hexadecimal representation starts with five zeroes. If it
does, the sixth character in the hash is the next character of the password.

For example, if the Door ID is abc:

- The first index which produces a hash that starts with five zeroes is 3231929, which we find by hashing abc3231929;
the sixth character of the hash, and thus the first character of the password, is 1.
- 5017308 produces the next interesting hash, which starts with 000008f82..., so the second character of the password is
8.
- The third time a hash starts with five zeroes is for abc5278568, discovering the character f.
In this example, after continuing this search a total of eight times, the password is 18f47a30.

Given the actual Door ID, what is the password?

--- Part Two ---

As the door slides open, you are presented with a second door that uses a slightly more inspired security mechanism.
Clearly unimpressed by the last version (in what movie is the password decrypted in order?!), the Easter Bunny
engineers have worked out a better solution.

Instead of simply filling in the password from left to right, the hash now also indicates the position within the
password to fill. You still look for hashes that begin with five zeroes; however, now, the sixth character represents
the position (0-7), and the seventh character is the character to put in that position.

A hash result of 000001f means that f is the second character in the password. Use only the first result for each
position, and ignore invalid positions.

For example, if the Door ID is abc:

- The first interesting hash is from abc3231929, which produces 0000015...; so, 5 goes in position 1: _5______.
- In the previous method, 5017308 produced an interesting hash; however, it is ignored, because it specifies an invalid 
position (8).
- The second interesting hash is at index 5357525, which produces 000004e...; so, e goes in position 4: _5__e___.
You almost choke on your popcorn as the final character falls into place, producing the password 05ace8e3.

Given the actual Door ID and this new method, what is the password? Be extra proud of your solution if it uses a 
cinematic "decrypting" animation.
*/

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "wtnhxymk";
            bool calculating = true;
            int index = 0;
            string hashable = "";
            string testHash = "";
            string password = "";

            // Part 1: Basic door MD5.

            Console.WriteLine("Calculating basic door MD5");
            while ( calculating )
            {
                hashable = input + index;
                testHash = CalculateMD5Hash(hashable);
                if ( testHash.Substring(0, 5) == "00000")
                {
                    password += testHash.ElementAt(5);
                }
                if (password.Length >= 8)
                    calculating = false;
                index++;
            }

            Console.WriteLine("Basic password is: " + password);

            // Part 2: Advanced door MD5.

            Console.WriteLine("Calculating advanced door MD5");
            calculating = true;
            index = 0;
            password = "        ";
            int passwordIndex = 0;
            while (calculating)
            {
                hashable = input + index;
                testHash = CalculateMD5Hash(hashable);
                // Roll hashes until one starts with "00000"
                if (testHash.Substring(0, 5) == "00000")
                {
                    char[] array = password.ToCharArray();
                    char testChar = testHash[5];
                    // Check if the 6th char in the hash is a number.
                    if (Char.IsNumber(testChar))
                    {
                        passwordIndex = (int)Char.GetNumericValue(testChar);
                        // If the number is between 0-7 and said index is still blank, add the 7th character to the
                        // password in that index.
                        if (passwordIndex <= 7 && array[passwordIndex] == ' ')
                        {
                            array[passwordIndex] = testHash[6];
                            password = new string(array);
                            Console.WriteLine("Password in progress: '" + password + "'");
                        }
                    }
                }

                // Stop when password is ready.
                if (!password.Contains(" "))
                    calculating = false;
                index++;

            }
            Console.WriteLine("Advanced password is: " + password);
        }

        // Function for calculating MD5 hash from given input.
        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
