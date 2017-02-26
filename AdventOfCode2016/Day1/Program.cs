using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * --- Day 1: No Time for a Taxicab ---

Santa's sleigh uses a very high-precision clock to guide its movements, and the clock's oscillator is regulated by
stars. Unfortunately, the stars have been stolen... by the Easter Bunny. To save Christmas, Santa needs you to
retrieve all fifty stars by December 25th.

Collect stars by solving puzzles. Two puzzles will be made available on each day in the advent calendar; the second
puzzle is unlocked when you complete the first. Each puzzle grants one star. Good luck!

You're airdropped near Easter Bunny Headquarters in a city somewhere. "Near", unfortunately, is as close as you can get
- the instructions on the Easter Bunny Recruiting Document the Elves intercepted start here, and nobody had time to
work them out further.

The Document indicates that you should start at the given coordinates (where you just landed) and face North. Then,
follow the provided sequence: either turn left (L) or right (R) 90 degrees, then walk forward the given number of
blocks, ending at a new intersection.

There's no time to follow such ridiculous instructions on foot, though, so you take a moment and work out the
destination. Given that you can only walk on the street grid of the city, how far is the shortest path to the
destination?

For example:

Following R2, L3 leaves you 2 blocks East and 3 blocks North, or 5 blocks away.
R2, R2, R2 leaves you 2 blocks due South of your starting position, which is 2 blocks away.
R5, L5, R5, R3 leaves you 12 blocks away.
How many blocks away is Easter Bunny HQ?

--- Part Two ---

Then, you notice the instructions continue on the back of the Recruiting Document. Easter Bunny HQ is actually at the 
first location you visit twice.

For example, if your instructions are R8, R4, R4, R8, the first location you visit twice is 4 blocks away, due East.

How many blocks away is the first location you visit twice?


*/

namespace Day1
{
    class Program
    {
        public struct Point
        {
            public int x;
            public int y;

            public Point ( int p1, int p2)
            {
                x = p1;
                y = p2;
            }
        }


        static void Main(string[] args)
        {
            String input = "";
            try
            {
                using (StreamReader sr = new StreamReader("input.txt"))
                {
                    input = sr.ReadToEnd();
                    Console.WriteLine("File read!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read: ");
                Console.WriteLine(e.Message);
            }

            List<Point> locations = new List<Point>();
            locations.Add(new Point(0, 0));
            int firstVisitedTwice = -1;

            int x = 0;
            int y = 0;
            int distance = 0;
            string[] inputs = input.Split(',');
            string direction = "NORTH";

            // Simplistic input handling due to assumption that all input is legit.
            for (int i = 0; i < inputs.Count(); i++)
            {   
                inputs[i] = inputs[i].Trim();
                if (inputs[i].StartsWith("R"))
                {
                    direction = turnRight(direction);
                }
                else if (inputs[i].StartsWith("L"))
                {
                    direction = turnLeft(direction);
                }
                distance = int.Parse(inputs[i].Substring(1));
                for (int s = 0; s < distance; s++)
                {
                    if (direction == "NORTH")
                    {
                        y++;
                    }
                    else if (direction == "SOUTH")
                    {
                        y--;
                    }
                    else if (direction == "EAST")
                    {
                        x++;
                    }
                    else if (direction == "WEST")
                    {
                        x--;
                    }

                    // Check if the new location has been visited.
                    if (firstVisitedTwice < 0)
                    {
                        Point currentLocation = new Point(x, y);
                        for (int index = 0; index < locations.Count; index++)
                        {
                            if (firstVisitedTwice < 0 && locations[index].x == x && locations[index].y == y)
                            {
                                firstVisitedTwice = index;
                            }
                        }
                        locations.Add(currentLocation);
                    }
                }
            }
            x = Math.Abs(x);
            y = Math.Abs(y);

            // Part 1: Distance traveled by going through the entire input.
            Console.WriteLine("Distance is " + x + " blocks horizontally and " + y + " blocks vertically for total of " + (x + y) + " blocks of distance");
            // Part 2: Distance to first visited twice.
            if ( firstVisitedTwice < 0 )
            {
                Console.WriteLine("No location visited twice!");
            }
            else
            {
                x = Math.Abs(locations[firstVisitedTwice].x);
                y = Math.Abs(locations[firstVisitedTwice].y);
                Console.WriteLine("Distance to first location visited twice is " + x + " blocks horizontally and " + y + " blocks vertically for total of " + (x + y) + " blocks of distance");
            }
        }

        // Simplistic helper function for turning left.
        public static string turnLeft( string from )
        {
            if (from == "NORTH" )
            {
                return "WEST";
            }
            else if (from == "EAST")
            {
                return "NORTH";
            }
            else if (from == "SOUTH")
            {
                return "EAST";
            }
            else
            {
                return "SOUTH";
            }
        }

        // Simplistic helper function for turning left.
        public static string turnRight( string from )
        {
            if (from == "NORTH")
            {
                return "EAST";
            }
            if (from == "EAST")
            {
                return "SOUTH";
            }
            if (from == "SOUTH")
            {
                return "WEST";
            }
            else
            {
                return "NORTH";
            }
        }
    }
}
