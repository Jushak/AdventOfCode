using System;
using System.Collections.Generic;
using System.IO;

/*

--- Day 16: Permutation Promenade ---
You come upon a very unusual sight; a group of programs here appear to be dancing.

There are sixteen programs in total, named a through p. They start by standing in a line: a stands in position 0, 
b stands in position 1, and so on until p, which stands in position 15.

The programs' dance consists of a sequence of dance moves:

    - Spin, written sX, makes X programs move from the end to the front, but maintain their order otherwise. 
    (For example, s3 on abcde produces cdeab).
    - Exchange, written xA/B, makes the programs at positions A and B swap places.
    - Partner, written pA/B, makes the programs named A and B swap places.

For example, with only five programs standing in a line (abcde), they could do the following dance:

    - s1, a spin of size 1: eabcd.
    - x3/4, swapping the last two programs: eabdc.
    - pe/b, swapping programs e and b: baedc.

After finishing their dance, the programs end up in order baedc.

You watch the dance for a while and record their dance moves (your puzzle input). In what order are the programs 
standing after their dance?

    Your puzzle answer was ceijbfoamgkdnlph.

--- Part Two ---
Now that you're starting to get a feel for the dance moves, you turn your attention to the dance as a whole.

Keeping the positions they ended up in from their previous dance, the programs perform it again and again: 
including the first dance, a total of one billion (1000000000) times.

In the example above, their second dance would begin with the order baedc, and use the same dance moves:

s1, a spin of size 1: cbaed.
x3/4, swapping the last two programs: cbade.
pe/b, swapping programs e and b: ceadb.
In what order are the programs standing after their billion dances?

    Your puzzle answer was pnhajoekigcbflmd.

*/


namespace Day16
{
    class Program
    {
        static char[] DoSpin(char[] input, int spin)
        {
            spin = spin % input.Length;
            if (spin == 0)
            {
                return input;
            }
            char[] output = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                int position = (i + input.Length - spin) % input.Length;
                output[i] = input[position];
            }
            return output;
        }

        static long FindCycleLength(char[] programs, string[] dance)
        {
            string initial = new string(programs);
            string current = "";
            long cycleLength = 0;
            while (string.Compare(initial, current) != 0)
            {
                string line = "";
                string[] numbers;
                string[] partners;
                int n1 = 0;
                int n2 = 0;
                char tmp = 'x';
                foreach (string s in dance)
                {
                    {
                        if (s[0].Equals('s'))
                        {
                            // Do spin
                            line = s.Remove(0, 1);
                            programs = DoSpin(programs, int.Parse(line));
                        }
                        else if (s[0].Equals('x'))
                        {
                            // Do exchange
                            line = s.Remove(0, 1);
                            numbers = line.Split('/', StringSplitOptions.RemoveEmptyEntries);
                            n1 = int.Parse(numbers[0]);
                            n2 = int.Parse(numbers[1]);
                            tmp = programs[n1];
                            programs[n1] = programs[n2];
                            programs[n2] = tmp;
                        }
                        else if (s[0].Equals('p'))
                        {
                            // Do partner
                            line = s.Remove(0, 1);
                            partners = line.Split('/', StringSplitOptions.RemoveEmptyEntries);
                            n1 = Array.IndexOf(programs, partners[0][0]);
                            n2 = Array.IndexOf(programs, partners[1][0]);
                            tmp = programs[n1];
                            programs[n1] = programs[n2];
                            programs[n2] = tmp;
                        }
                        else
                        {
                            // Abandon all hope ye who enter
                            Console.WriteLine("Input handling error with input: " + s);
                            Console.ReadKey();
                            Environment.Exit(0);
                        }
                    }
                }
                current = new string(programs);
                cycleLength++;
            }
            return cycleLength;
        }

        static char[] DoDance(char[] programs, long dances, string[] dance)
        {
            string line = "";
            string[] numbers;
            string[] partners;
            int n1 = 0;
            int n2 = 0;
            char tmp = 'x';
            for (long i = 0; i < dances; i++)
            {
                foreach (string s in dance)
                {
                    {
                        if (s[0].Equals('s'))
                        {
                            // Do spin
                            line = s.Remove(0, 1);
                            programs = DoSpin(programs, int.Parse(line));
                        }
                        else if (s[0].Equals('x'))
                        {
                            // Do exchange
                            line = s.Remove(0, 1);
                            numbers = line.Split('/', StringSplitOptions.RemoveEmptyEntries);
                            n1 = int.Parse(numbers[0]);
                            n2 = int.Parse(numbers[1]);
                            tmp = programs[n1];
                            programs[n1] = programs[n2];
                            programs[n2] = tmp;
                        }
                        else if (s[0].Equals('p'))
                        {
                            // Do partner
                            line = s.Remove(0, 1);
                            partners = line.Split('/', StringSplitOptions.RemoveEmptyEntries);
                            n1 = Array.IndexOf(programs, partners[0][0]);
                            n2 = Array.IndexOf(programs, partners[1][0]);
                            tmp = programs[n1];
                            programs[n1] = programs[n2];
                            programs[n2] = tmp;
                        }
                        else
                        {
                            // Abandon all hope ye who enter
                            Console.WriteLine("Input handling error with input: " + s);
                            Console.ReadKey();
                            Environment.Exit(0);
                        }
                    }
                }
            }
            return programs;
        }

        static void Main(string[] args)
        {
            char[] programs = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p' };
            string[] input;
            string line;
            string[] delimiter = { " ", "\t", "," };
            string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);

            // Read the puzzle input.
            StreamReader file = new StreamReader(filePath + "/input.txt");
            line = file.ReadLine();
            input = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

            // Debug:
            // string[] input = { "s1", "x3/4", "pe/b" };
            // char[] programs = { 'a', 'b', 'c', 'd', 'e' };

            programs = DoDance(programs, 1, input);
            string result = "";
            for (int i = 0; i < programs.Length; i++)
            {
                result += programs[i];
            }
            Console.WriteLine("Part 1: The order is: " + result);
            Console.ReadKey();
            Console.WriteLine();

            long cycleLength = FindCycleLength(programs, input);
            long repetitions = 1000000000 % cycleLength;
            programs = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p' };

            Console.WriteLine("Cycle length found! It is: " + cycleLength);

            programs = DoDance(programs, repetitions, input);
            result = "";
            for (int i = 0; i < programs.Length; i++)
            {
                result += programs[i];
            }
            Console.WriteLine("Part 2: The order is: " + result);
            Console.ReadKey();
        }
    }
}
