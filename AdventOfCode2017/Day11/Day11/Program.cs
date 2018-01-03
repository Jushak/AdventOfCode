using System;
using System.IO;

/*

--- Day 11: Hex Ed ---
Crossing the bridge, you've barely reached the other side of the stream when a program comes up to you, clearly in 
distress. "It's my child process," she says, "he's gotten lost in an infinite grid!"

Fortunately for her, you have plenty of experience with infinite grids.

Unfortunately for you, it's a hex grid.

The hexagons ("hexes") in this grid are aligned such that adjacent hexes can be found to the north, northeast, 
southeast, south, southwest, and northwest:

  \ n  /
nw +--+ ne
  /    \
-+      +-
  \    /
sw +--+ se
  / s  \

You have the path the child process took. Starting where he started, you need to determine the fewest number of steps 
required to reach him. (A "step" means to move from the hex you are in to any adjacent hex.)

For example:

- ne,ne,ne is 3 steps away.
- ne,ne,sw,sw is 0 steps away (back where you started).
- ne,ne,s,s is 2 steps away (se,se).
- se,sw,se,sw,sw is 3 steps away (s,s,sw).

    Your puzzle answer was 743.

--- Part Two ---
How many steps away is the furthest he ever got from his starting position?

    Your puzzle answer was 1493.
*/

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input;
            string line;
            string[] delimiter = { " ", "\t", "," };
            string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            
            // Read the puzzle input.
            StreamReader file = new StreamReader(filePath + "/input.txt");
            line = file.ReadLine();
            input = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

            int r = 0;
            int g = 0;
            int b = 0;
            int maxDistance = 0;
            foreach(string s in input)
            {
                switch(s)
                {
                    case "n":
                        b++;
                        g--;
                        break;
                    case "ne":
                        r++;
                        g--;
                        break;
                    case "se":
                        r++;
                        b--;
                        break;
                    case "s":
                        g++;
                        b--;
                        break;
                    case "sw":
                        g++;
                        r--;
                        break;
                    case "nw":
                        b++;
                        r--;
                        break;
                }
                if (Math.Abs(r) > maxDistance)
                {
                    maxDistance = Math.Abs(r);
                }
                else if (Math.Abs(g) > maxDistance)
                {
                    maxDistance = Math.Abs(g);
                }
                else if (Math.Abs(b) > maxDistance)
                {
                    maxDistance = Math.Abs(b);
                }
            }
            int steps = Math.Max(Math.Abs(r), Math.Max(Math.Abs(g), Math.Abs(b)));
            // Part 1: Distance at end.
            Console.WriteLine("Part 1: Steps at the end: "+steps);
            // Part 2: Maximum distance.
            Console.WriteLine("Part 2: Maximum distance: "+maxDistance);
            Console.ReadKey();

        }
    }
}
