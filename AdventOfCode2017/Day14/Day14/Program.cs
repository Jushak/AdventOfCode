using System;
using System.Collections.Generic;

/*

--- Day 14: Disk Defragmentation ---
Suddenly, a scheduled job activates the system's disk defragmenter. Were the situation different, you might sit and 
watch it for a while, but today, you just don't have that kind of time. It's soaking up valuable system resources that 
are needed elsewhere, and so the only option is to help it finish its task as soon as possible.

The disk in question consists of a 128x128 grid; each square of the grid is either free or used. On this disk, 
the state of the grid is tracked by the bits in a sequence of knot hashes.

A total of 128 knot hashes are calculated, each corresponding to a single row in the grid; each hash contains 128 bits 
which correspond to individual grid squares. Each bit of a hash indicates whether that square is free (0) or used (1).

The hash inputs are a key string (your puzzle input), a dash, and a number from 0 to 127 corresponding to the row. 
For example, if your key string were flqrgnkx, then the first row would be given by the bits of the knot hash of 
flqrgnkx-0, the second row from the bits of the knot hash of flqrgnkx-1, and so on until the last row, flqrgnkx-127.

The output of a knot hash is traditionally represented by 32 hexadecimal digits; each of these digits correspond to 
4 bits, for a total of 4 * 32 = 128 bits. To convert to bits, turn each hexadecimal digit to its equivalent binary 
value, high-bit first: 0 becomes 0000, 1 becomes 0001, e becomes 1110, f becomes 1111, and so on; a hash that begins
with a0c2017... in hexadecimal would begin with 10100000110000100000000101110000... in binary.

Continuing this process, the first 8 rows and columns for key flqrgnkx appear as follows, using # to denote used 
squares, and . to denote free ones:

    ##.#.#..-->
    .#.#.#.#   
    ....#.#.   
    #.#.##.#   
    .##.#...   
    ##..#..#   
    .#...#..   
    ##.#.##.-->
    |      |   
    V      V   

In this example, 8108 squares are used across the entire 128x128 grid.

Given your actual key string, how many squares are used?

    Your puzzle answer was 8226.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---
Now, all the defragmenter needs to know is the number of regions. A region is a group of used squares that are all 
adjacent, not including diagonals. Every used square is in exactly one region: lone used squares form their own 
isolated regions, while several adjacent squares all count as a single region.

In the example above, the following nine regions are visible, each marked with a distinct digit:

    11.2.3..-->
    .1.2.3.4   
    ....5.6.   
    7.8.55.9   
    .88.5...   
    88..5..8   
    .8...8..   
    88.8.88.-->
    |      |   
    V      V   

Of particular interest is the region marked 8; while it does not appear contiguous in this small view, all of the 
squares marked 8 are connected when considering the whole 128x128 grid. In total, in this example, 1242 regions are 
present.

How many regions are present given your key string?

    Your puzzle answer was 1128.

*/

namespace Day14
{
    class Node
    {
        public bool used;
        public bool grouped;

        public Node()
        {
            this.used = false;
            this.grouped = false;
        }

        public Node(bool used)
        {
            this.used = used;
            this.grouped = false;
        }
    }

    class Program
    {
        // Gets the "memory string"-version of the byte that matches the given hexadecimal number.
        static string GetMemoryString(char c)
        {
            // # - used ; . - free
            switch (c)
            {
                case '0':
                    return "....";
                case '1':
                    return "...#";
                case '2':
                    return "..#.";
                case '3':
                    return "..##";
                case '4':
                    return ".#..";
                case '5':
                    return ".#.#";
                case '6':
                    return ".##.";
                case '7':
                    return ".###";
                case '8':
                    return "#...";
                case '9':
                    return "#..#";
                case 'a':
                    return "#.#.";
                case 'b':
                    return "#.##";
                case 'c':
                    return "##..";
                case 'd':
                    return "##.#";
                case 'e':
                    return "###.";
                case 'f':
                    return "####";
            }
            return "";
        }

        // Get the number of 1's in the byte of the provided hexadecimal number.
        static int GetSUsedCount(char c)
        {
            switch (c)
            {
                case '0': // 0000
                    return 0;
                case '1': // 0001
                    return 1;
                case '2': // 0010
                    return 1;
                case '3': // 0011
                    return 2;
                case '4': // 0100
                    return 1;
                case '5': // 0101
                    return 2;
                case '6': // 0110
                    return 2;
                case '7': // 0111
                    return 3;
                case '8': // 1000
                    return 1;
                case '9': // 1001
                    return 2;
                case 'a': // 1010
                    return 2;
                case 'b': // 1011
                    return 3;
                case 'c': // 1100
                    return 2;
                case 'd': // 1101
                    return 3;
                case 'e': // 1110
                    return 3;
                case 'f': // 1111
                    return 4;
            }
            return 0;
        }

        // Starting from the given location, recursively marks all connected used nodes as grouped.
        static void MarkAsGrouped(int x, int y, Node[,] memory)
        {
            memory[x, y].grouped = true;
            if (x > 0 && memory[x-1,y].used && !memory[x-1,y].grouped)
            {
                MarkAsGrouped(x - 1, y, memory);
            }
            if (x < 127 && memory[x+1,y].used && !memory[x+1,y].grouped)
            {
                MarkAsGrouped(x + 1, y, memory);
            }
            if (y > 0 && memory[x,y-1].used && !memory[x,y-1].grouped)
            {
                MarkAsGrouped(x, y - 1, memory);
            }
            if (y < 127 && memory[x,y+1].used && !memory[x, y + 1].grouped)
            {
                MarkAsGrouped(x, y + 1, memory);
            }
        }

        static void Main(string[] args)
        {
            // My input + '-'
            string input = "wenycdww-";
            // Debug input:
            //input = "flqrgnkx-";
            List<string> knothashes = new List<string>();
            List<int> lengths = new List<int>();
            List<int> baseString = new List<int>();
            List<int> hashString = new List<int>();
            List<int> subList = new List<int>();
            List<string> rows = new List<string>();
            int used = 0;
            string row = "";
            
            // Create base string of numbers.
            for (int i = 0; i < 256; i++)
            {
                baseString.Add(i);
            }
            

            // Run once for each of the 128 rows.
            for (int round = 0; round < 128; round++)
            {
                // Build current row's lengths.
                lengths.Clear();
                string tmp = input+round.ToString();
                foreach (char c in tmp)
                {
                    lengths.Add(c);
                }
                lengths.Add(17);
                lengths.Add(31);
                lengths.Add(73);
                lengths.Add(47);
                lengths.Add(23);

                // Reset hashString to pristine condition before creating the row.
                hashString.Clear();
                hashString.AddRange(baseString);

                int index = 0;
                int skip = 0;
                row = "";

                // Do 64 rounds of hashing.
                int targetRounds = 64;
                for (int rounds = 0; rounds < targetRounds; rounds++)
                {
                    foreach (int length in lengths)
                    {
                        subList.Clear();
                        for (int i = length - 1; i >= 0; i--)
                        {
                            int x = index + i;
                            if (x > 255)
                            {
                                x -= 256;
                            }
                            subList.Add(hashString[x]);
                        }
                        for (int i = 0; i < length; i++)
                        {
                            int x = index + i;
                            if (x > 255)
                            {
                                x -= 256;
                            }
                            hashString[x] = subList[i];
                        }

                        index += length + skip;
                        while (index > 255)
                        {
                            index -= 256;
                        }
                        skip++;
                    }
                }

                // Build the hash from the numbers
                index = 0;
                string knothash = "";
                while (index < 256)
                {
                    int item = hashString[index];
                    for (int i = 1; i < 16; i++)
                    {
                        item ^= hashString[index + i];
                    }
                    knothash += item.ToString("x2");
                    index += 16;
                }

                // Count used bytes.
                foreach (char c in knothash)
                {
                    used += GetSUsedCount(c);
                    row += GetMemoryString(c);
                }

                rows.Add(row);
            }

            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    Console.Write(rows[j][i]);
                }
                Console.WriteLine(" ->");
            }
            Console.WriteLine();
            Console.WriteLine("|      |");
            Console.WriteLine("V      V");

            Console.WriteLine("Part 1: Total used: " + used);
            Console.ReadKey();
            Console.WriteLine();

            // Build memory grid from nodes that know whether their memory has been used and whether they've already
            // been grouped.
            Node[,] memory = new Node[128, 128];
            for (int y = 0; y < 128; y++)
            {
                for (int x = 0; x < 128; x++)
                {
                    memory[x, y] = new Node(rows[y][x] == '#');
                }
            }

            int groups = 0;

            for (int y = 0; y < 128; y++)
            {
                for (int x = 0; x< 128; x++)
                {
                    // If a node's memory has been used but it has not yet been grouped, do recurvive marking of all
                    // connected nodes.
                    if (memory[x, y].used && !memory[x,y].grouped)
                    {
                        groups++;
                        MarkAsGrouped(x, y, memory);
                    }
                }
            }

            Console.WriteLine("Part 2: Total number of groups is : " + groups);
            Console.ReadKey();
        }
    }
}
