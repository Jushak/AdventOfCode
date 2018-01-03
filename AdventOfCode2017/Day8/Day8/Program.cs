using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/*

--- Day 8: I Heard You Like Registers ---

You receive a signal directly from the CPU. Because of your recent assistance with jump instructions, it would like
you to compute the result of a series of unusual register instructions.

Each instruction consists of several parts: the register to modify, whether to increase or decrease that register's
value, the amount by which to increase or decrease it, and a condition. If the condition fails, skip the instruction
without modifying the register. The registers all start at 0. The instructions look like this:

    b inc 5 if a > 1
    a inc 1 if b < 5
    c dec -10 if a >= 1
    c inc -20 if c == 10

These instructions would be processed as follows:

    Because a starts at 0, it is not greater than 1, and so b is not modified.
    a is increased by 1 (to 1) because b is less than 5 (it is 0).
    c is decreased by -10 (to 10) because a is now greater than or equal to 1 (it is 1).
    c is increased by -20 (to -10) because c is equal to 10.
    After this process, the largest value in any register is 1.

You might also encounter <= (less than or equal to) or != (not equal to). However, the CPU doesn't have the bandwidth 
to tell you what all the registers are named, and leaves that to you to determine.

What is the largest value in any register after completing the instructions in your puzzle input?

    Your puzzle answer was 4448.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---

To be safe, the CPU also needs to know the highest value held in any register during this process so that it can decide
how much memory to allocate to these operations. For example, in the above instructions, the highest value ever held was
10 (in register c after the third instruction was evaluated).

    Your puzzle answer was 6582.

*/

namespace Day8
{
    class Program
    {
        static bool IsConditionMatched(int x, int y, string condition)
        {
            if (string.Compare(condition, "==") == 0)
            {
                return (x == y);
            }
            else if (string.Compare(condition, "<") == 0)
            {
                return (x < y);
            }
            else if (string.Compare(condition, ">") == 0)
            {
                return (x > y);
            }
            else if (string.Compare(condition, "<=") == 0)
            {
                return (x <= y);
            }
            else if (string.Compare(condition, ">=") == 0)
            {
                return (x >= y);
            }
            else if (string.Compare(condition, "!=") == 0)
            {
                return (x != y);
            }
            // Should never reach this.
            return false;
        }

        static void Main(string[] args)
        {
            int highest = 0;
            string[] input;
            string line;
            string[] delimiter = { " ", "\t" };
            string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            List<string> instructions = new List<string>();
            Dictionary<string, int> registeries = new Dictionary<string, int>();

            // Read and store all instructions.
            StreamReader file = new StreamReader(filePath + "/input.txt");
            while ((line = file.ReadLine()) != null)
            {
                instructions.Add(line);
            }

            foreach (string s in instructions)
            {
                input = s.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                if (!registeries.ContainsKey(input[0]))
                {
                    registeries.Add(input[0], 0);
                }
                if (!registeries.ContainsKey(input[4]))
                {
                    registeries.Add(input[4], 0);
                }
                if (IsConditionMatched(registeries[input[4]], int.Parse(input[6]), input[5]))
                {
                    if (string.Compare(input[1], "inc") == 0)
                    {
                        registeries[input[0]] += int.Parse(input[2]);
                    }
                    else
                    {
                        registeries[input[0]] -= int.Parse(input[2]);
                    }
                    if (registeries[input[0]] > highest)
                    {
                        highest = registeries[input[0]];
                    }
                }
            }
            int result = registeries.Values.Max();
            Console.WriteLine("Part 1: Highest register value is " + result);
            Console.WriteLine("Part 2: Highest register value seen during the process: " + highest);
            Console.ReadKey();
        }
    }
}
