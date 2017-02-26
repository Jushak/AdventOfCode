using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
--- Day 21: Scrambled Letters and Hash ---

The computer system you're breaking into uses a weird scrambling function to store its passwords. It shouldn't be much
trouble to create your own scrambled password so you can add it to the system; you just have to implement the
scrambler.

The scrambling function is a series of operations (the exact list is provided in your puzzle input). Starting with the
password to be scrambled, apply each operation in succession to the string. The individual operations behave as
follows:

- swap position X with position Y means that the letters at indexes X and Y (counting from 0) should be swapped.
- swap letter X with letter Y means that the letters X and Y should be swapped (regardless of where they appear in the
string).
- rotate left/right X steps means that the whole string should be rotated; for example, one right rotation would turn
abcd into dabc.
- rotate based on position of letter X means that the whole string should be rotated to the right based on the index of
letter X (counting from 0) as determined before this instruction does any rotations. Once the index is determined,
rotate the string to the right one time, plus a number of times equal to that index, plus one additional time if the
index was at least 4.
- reverse positions X through Y means that the span of letters at indexes X through Y (including the letters at X and
Y) should be reversed in order.
- move position X to position Y means that the letter which is at index X should be removed from the string, then
inserted such that it ends up at index Y.

For example, suppose you start with abcde and perform the following operations:

- swap position 4 with position 0 swaps the first and last letters, producing the input for the next step, ebcda.
- swap letter d with letter b swaps the positions of d and b: edcba.
- reverse positions 0 through 4 causes the entire string to be reversed, producing abcde.
rotate left 1 step shifts all letters left one position, causing the first letter to wrap to the end of the string:
bcdea.
- move position 1 to position 4 removes the letter at position 1 (c), then inserts it at position 4 (the end of the
string): bdeac.
- move position 3 to position 0 removes the letter at position 3 (a), then inserts it at position 0 (the front of
the string): abdec.
- rotate based on position of letter b finds the index of letter b (1), then rotates the string right once plus a
number of times equal to that index (2): ecabd.
- rotate based on position of letter d finds the index of letter d (4), then rotates the string right once, plus a
number of times equal to that index, plus an additional time because the index was at least 4, for a total of 6 right
rotations: decab.

After these steps, the resulting scrambled password is decab.

Now, you just need to generate a new scrambled password and you can access the system. Given the list of scrambling
operations in your puzzle input, what is the result of scrambling abcdefgh?

Your puzzle answer was baecdfgh.

--- Part Two ---

You scrambled the password correctly, but you discover that you can't actually modify the password file on the system.
You'll need to un-scramble one of the existing passwords by reversing the scrambling process.

What is the un-scrambled version of the scrambled password fbgdceah?

*/

namespace Day21
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> input = new List<string>();
            string line = "";
            try
            {
                using (StreamReader sr = new StreamReader("input.txt"))
                {
                    while ( sr.Peek() >= 0)
                    {
                        line = sr.ReadLine();
                        input.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not read file: ");
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }

            bool part2 = true; // Flip to false for part 1.

            string password = "abcdefgh";
            // If in part 2, the commands need to be executed in reverse order.
            if ( part2 )
            {
                password = "fbgdceah";
                input.Reverse();
            }
            char[] chars = password.ToArray();

            // Execute all commands in given order.
            for ( int i = 0; i < input.Count; i++ )
            {
                // Console commands included for debugging.
                //Console.WriteLine("Command line is: " + input[i]);
                //Console.WriteLine("Before: " + new string(chars));
                string[] command = input[i].Split(' ');
                if (command[0] == "swap")
                {
                    int x = 0;
                    int y = 0;
                    if (int.TryParse(command[2], out x) && int.TryParse(command[5], out y))
                    {
                        char swapped = chars[x];
                        chars[x] = chars[y];
                        chars[y] = swapped;
                    }
                    else
                    {
                        for (int j = 0; j < chars.Length; j++)
                        {
                            if (chars[j] == command[2][0])
                            {
                                chars[j] = command[5][0];
                            }
                            else if (chars[j] == command[5][0])
                            {
                                chars[j] = command[2][0];
                            }
                        }
                    }
                }
                else if (command[0] == "rotate")
                {
                    if (command[1] == "right" || command[1] == "based")
                    {
                        if (part2 && command[1] == "based")
                        {
                            int rotations = 0;
                            string indexed = "";
                            bool found = false;
                            int index = 0;
                            while (!found)
                            {
                                RotateLeft<char>(chars);
                                rotations++;
                                indexed = new string(chars);
                                index = indexed.IndexOf(command[6]);
                                if (index >= 4)
                                {
                                    index++;
                                }
                                index++;
                                // If number of rotations matches the rotation formula at current rotation, enough rotations have been made.
                                if (rotations == index)
                                {
                                    found = true;
                                }
                            }
                        }
                        else
                        {
                            int rotations = 0;
                            if (!int.TryParse(command[2], out rotations))
                            {
                                rotations = 1;
                                string indexed = new string(chars);
                                int index = indexed.IndexOf(command[6]);
                                rotations += index;
                                if (index >= 4)
                                {
                                    rotations++;
                                }
                            }
                            for (int j = 0; j < rotations; j++)
                            {
                                if (part2)
                                {
                                    RotateLeft<char>(chars);
                                }
                                else
                                {
                                    RotateRight<char>(chars);
                                }
                            }
                        }
                    }
                    else if (command[1] == "left")
                    {
                        int rotations = int.Parse(command[2]);
                        for (int j = 0; j < rotations; j++)
                        {
                            if (part2)
                            {
                                RotateRight<char>(chars);
                            }
                            else
                            {
                                RotateLeft<char>(chars);
                            }
                        }
                    }
                }
                else if (command[0] == "reverse")
                {
                    int y = int.Parse(command[4]);
                    for (int x = int.Parse(command[2]); x < y; x++, y--)
                    {
                        char tmp = chars[x];
                        chars[x] = chars[y];
                        chars[y] = tmp;
                    }
                }
                else if (command[0] == "move")
                {
                    int x = int.Parse(command[2]);
                    int y = int.Parse(command[5]);
                    if (part2)
                    {
                        int swap = x;
                        x = y;
                        y = swap;
                    }
                    string indexed = new string(chars);
                    string tmp = indexed.Substring(x, 1);
                    indexed = indexed.Remove(x, 1);
                    indexed = indexed.Insert(y, tmp);
                    chars = indexed.ToCharArray();

                }
                else
                {
                    // Mostly for debug purposes: if command is not found, close the program inconclusively.
                    Console.WriteLine("Unidentified command. Exiting program.");
                    Environment.Exit(0);
                }
                //Console.WriteLine("After:  " + new string(chars));
            }

            Console.WriteLine("The scrambled password is: " + new string(chars));


        }

        static void RotateLeft<T>(T[] source)
        {
            var temp = source[0];
            for (int i = 0; i < source.Length - 1; i++)
            {
                source[i] = source[i + 1];
            }
            source[source.Length - 1] = temp;
        }

        static void RotateRight<T>(T[] source)
        {
            var temp = source[source.Length-1];
            for ( int i = source.Length-1 ; i > 0; i-- )
            {
                source[i] = source[i - 1];
            }
            source[0] = temp;
        }
    }
}
