using System;
using System.Collections.Generic;
using System.IO;

/*

--- Day 21: Fractal Art ---
You find a program trying to generate some art. It uses a strange process that involves repeatedly enhancing the 
detail of an image through a set of rules.

The image consists of a two-dimensional square grid of pixels that are either on (#) or off (.). The program always 
begins with this pattern:

    .#.
    ..#
    ###

Because the pattern is both 3 pixels wide and 3 pixels tall, it is said to have a size of 3.

Then, the program repeats the following process:

    - If the size is evenly divisible by 2, break the pixels up into 2x2 squares, and convert each 2x2 square into 
    a 3x3 square by following the corresponding enhancement rule.
    - Otherwise, the size is evenly divisible by 3; break the pixels up into 3x3 squares, and convert each 3x3 square
    into a 4x4 square by following the corresponding enhancement rule.
    
Because each square of pixels is replaced by a larger one, the image gains pixels and so its size increases.

The artist's book of enhancement rules is nearby (your puzzle input); however, it seems to be missing rules. 
The artist explains that sometimes, one must rotate or flip the input pattern to find a match. (Never rotate 
or flip the output pattern, though.) Each pattern is written concisely: rows are listed as single units, 
ordered top-down, and separated by slashes. For example, the following rules correspond to the adjacent patterns:

    ../.#  =  ..
              .#

                    .#.
    .#./..#/###  =  ..#
                    ###

                            #..#
    #..#/..../#..#/.##.  =  ....
                            #..#
                            .##.

When searching for a rule to use, rotate and flip the pattern as necessary. For example, all of the following patterns 
match the same rule:

    .#.   .#.   #..   ###
    ..#   #..   #.#   ..#
    ###   ###   ##.   .#.

Suppose the book contained the following two rules:

    ../.# => ##./#../...
    .#./..#/### => #..#/..../..../#..#

As before, the program begins with this pattern:

    .#.
    ..#
    ###

The size of the grid (3) is not divisible by 2, but it is divisible by 3. It divides evenly into a single square; 
the square matches the second rule, which produces:

    #..#
    ....
    ....
    #..#

The size of this enhanced grid (4) is evenly divisible by 2, so that rule is used. It divides evenly into four squares:

    #.|.#
    ..|..
    --+--
    ..|..
    #.|.#

Each of these squares matches the same rule (../.# => ##./#../...), three of which require some flipping and rotation 
to line up with the rule. The output for the rule is the same in all four cases:

    ##.|##.
    #..|#..
    ...|...
    ---+---
    ##.|##.
    #..|#..
    ...|...

Finally, the squares are joined into a new grid:

    ##.##.
    #..#..
    ......
    ##.##.
    #..#..
    ......

Thus, after 2 iterations, the grid contains 12 pixels that are on.

How many pixels stay on after 5 iterations?

    Your puzzle answer was 162.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---
How many pixels stay on after 18 iterations?

*/

namespace Day21
{
    class Program
    {
        static string RotateRight(string target)
        {
            string result = "";
            if (target.Length == 4)
            {
                result += target[2].ToString() + target[0].ToString() + target[3].ToString() + target[1].ToString();
            }
            else if (target.Length == 9)
            {
                result += target[6].ToString() + target[3].ToString() + target[0].ToString() + target[7].ToString()
                    + target[4].ToString() + target[1].ToString() + target[8].ToString() + target[5].ToString()
                    + target[2].ToString();
            }
            else
            {
                Console.WriteLine("Rotate error: wrong string size!");
                Console.ReadKey();
                Environment.Exit(0);
            }
            return result;
        }

        static string Flip(string target)
        {
            string result = "";
            if (target.Length == 4)
            {
                result += target[1].ToString() + target[0].ToString() + target[3].ToString() + target[2].ToString();
            }
            else if (target.Length == 9)
            {
                result += target[2].ToString() + target[1].ToString() + target[0].ToString() + target[5].ToString()
                    + target[4].ToString() + target[3].ToString() + target[8].ToString() + target[7].ToString()
                    + target[6].ToString();
            }
            else
            {
                Console.WriteLine("Flip error: wrong string size!");
                Console.ReadKey();
                Environment.Exit(0);
            }
            return result;
        }

        static string FindEnhancement(string source, Dictionary<string, string> rules)
        {
            string flipped = "";
            // First, try each rotation.
            for (int i = 0; i < 4; i++)
            {
                if (rules.ContainsKey(source))
                {
                    return rules[source];
                }
                flipped = Flip(source);
                if (rules.ContainsKey(flipped))
                {
                    return rules[flipped];
                }
                source = RotateRight(source);
            }

            // No match. This shouldn't ever happen.
            Console.WriteLine("Could not find matching rule!");
            Console.ReadKey();
            Environment.Exit(0);
            return "";
        }

        static void Main(string[] args)
        {
            string[] input;
            string line;
            string[] delimiter = { " ", "\t", "/", "=", ">" };
            string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);

            Dictionary<string, string> rules = new Dictionary<string, string>();

            // Read the puzzle input.
            StreamReader file = new StreamReader(filePath + "/input.txt");
            while ((line = file.ReadLine()) != null)
            {
                input = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                if (input.Length == 5)
                {
                    rules.Add(input[0] + input[1], input[2] + input[3] + input[4]);
                }
                else if (input.Length == 7)
                {
                    rules.Add(input[0] + input[1] + input[2], input[3] + input[4] + input[5] + input[6]);
                }
                else
                {
                    Console.WriteLine("Input error!");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }

            foreach (KeyValuePair<string, string> rule in rules)
            {
                Console.WriteLine(rule.Key + " => " + rule.Value);
            }

            List<string> grid = new List<string>();
            grid.Add(".#.");
            grid.Add("..#");
            grid.Add("### ");
            string candidate = "";
            List<string> gridpieces = new List<string>();

            int iterations = 0;
            // Part 1:
            int iterationCount = 5;
            // Part 2:
            iterationCount = 18;
            while (iterations < iterationCount)
            {
                gridpieces.Clear();
                if (grid.Count % 2 == 0)
                {
                    for (int y = 0; y < grid.Count; y += 2)
                    {
                        for (int x = 0; x < grid.Count; x += 2)
                        {
                            candidate = grid[y].Substring(x, 2) + grid[y + 1].Substring(x, 2);
                            //Console.WriteLine("Candidate string is: " + candidate);
                            gridpieces.Add(FindEnhancement(candidate, rules));
                        }
                    }
                    int newSize = grid.Count / 2 * 3;
                    grid.Clear();
                    for (int y = 0; y < newSize; y += 3)
                    {
                        string line1 = "";
                        string line2 = "";
                        string line3 = "";
                        for (int x = 0; x < newSize; x += 3)
                        {
                            line1 += gridpieces[0].Substring(0, 3);
                            line2 += gridpieces[0].Substring(3, 3);
                            line3 += gridpieces[0].Substring(6, 3);
                            gridpieces.RemoveAt(0);
                        }
                        grid.Add(line1);
                        grid.Add(line2);
                        grid.Add(line3);
                    }
                    iterations++;
                }
                else if (grid.Count % 3 == 0)
                {
                    for (int y = 0; y < grid.Count; y += 3)
                    {
                        for (int x = 0; x < grid.Count; x += 3)
                        {
                            candidate = grid[y].Substring(x, 3) + grid[y + 1].Substring(x, 3) + grid[y + 2].Substring(x, 3);
                            //Console.WriteLine("Candidate string is: " + candidate);
                            gridpieces.Add(FindEnhancement(candidate, rules));
                        }
                    }
                    int newSize = grid.Count / 3 * 4;
                    grid.Clear();
                    for (int y = 0; y < newSize; y += 4)
                    {
                        string line1 = "";
                        string line2 = "";
                        string line3 = "";
                        string line4 = "";
                        for (int x = 0; x < newSize; x += 4)
                        {
                            line1 += gridpieces[0].Substring(0, 4);
                            line2 += gridpieces[0].Substring(4, 4);
                            line3 += gridpieces[0].Substring(8, 4);
                            line4 += gridpieces[0].Substring(12, 4);
                            gridpieces.RemoveAt(0);
                        }
                        grid.Add(line1);
                        grid.Add(line2);
                        grid.Add(line3);
                        grid.Add(line4);
                    }
                    iterations++;
                }
                else
                {
                    Console.WriteLine("Incorrect grid size.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }

            // Finally, count the number of # in the grid.
            int hashtags = 0;
            foreach (string s in grid)
            {
                foreach (char c in s)
                {
                    if (c.Equals('#'))
                    {
                        hashtags++;
                    }
                }
            }

            foreach (string s in grid)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine();

            Console.WriteLine("The number of pixels that stay on is " + hashtags);
            Console.WriteLine("Grid size is: " + Math.Pow(grid.Count, 2) +" pixels");
            Console.ReadKey();
        }
    }
}
