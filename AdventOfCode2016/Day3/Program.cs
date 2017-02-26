using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * --- Day 3: Squares With Three Sides ---

Now that you can think clearly, you move deeper into the labyrinth of hallways and office furniture that makes up this 
part of Easter Bunny HQ. This must be a graphic design department; the walls are covered in specifications for 
triangles.

Or are they?

The design document gives the side lengths of each triangle it describes, but... 5 10 25? Some of these aren't 
triangles. You can't help but mark the impossible ones.

In a valid triangle, the sum of any two sides must be larger than the remaining side. For example, the "triangle"
given above is impossible, because 5 + 10 is not larger than 25.

In your puzzle input, how many of the listed triangles are possible?

--- Part Two ---

Now that you've helpfully marked up their design documents, it occurs to you that triangles are specified in groups
of three vertically. Each set of three numbers in a column specifies a triangle. Rows are unrelated.

For example, given the following specification, numbers with the same hundreds digit would be part of the same
triangle:

101 301 501
102 302 502
103 303 503
201 401 601
202 402 602
203 403 603
In your puzzle input, and instead reading by columns, how many of the listed triangles are possible?

*/

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> input = new List<string>();
            string line;
            try
            {
                using (StreamReader sr = new StreamReader("input.txt"))
                {
                    while (sr.Peek() >= 0)
                    {
                        line = sr.ReadLine();
                        line.Trim();
                        input.Add(line);
                    }
                }
                Console.WriteLine("Lines: " + input.Count);
                Console.WriteLine("File read!");
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read: ");
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }

            int possibleTriangles = 0;
            int possibleVerticals = 0;

            // Part 1: Normal triangles.
            for (int i = 0; i < input.Count; i++)
            {
                int[] numbers = input[i].Split(new char[0], StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n)).ToArray(); 
                numbers = (from element in numbers orderby element ascending select element).ToArray();
                if (numbers[2] < numbers[1] + numbers[0])
                    if (numbers[1] < numbers[2] + numbers[0])
                        if (numbers[0] < numbers[2] + numbers[1])
                            possibleTriangles++;
            }

            // Part 2: Triangles from reading the input vertically.

            // Start by transforming the original input into new list of vertical inputs.
            List<int[]> verticals = new List<int[]>();

            for ( int i = 0; i < input.Count; i++)
            {
                int[] triangle1 = new int[3];
                int[] triangle2 = new int[3];
                int[] triangle3 = new int[3];
                int[] numbers = input[i].Split(new char[0], StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n)).ToArray();

                triangle1[0] = numbers[0];
                triangle2[0] = numbers[1];
                triangle3[0] = numbers[2];
                i++;
                numbers = input[i].Split(new char[0], StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n)).ToArray();
                triangle1[1] = numbers[0];
                triangle2[1] = numbers[1];
                triangle3[1] = numbers[2];
                i++;
                numbers = input[i].Split(new char[0], StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n)).ToArray();
                triangle1[2] = numbers[0];
                triangle2[2] = numbers[1];
                triangle3[2] = numbers[2];

                verticals.Add(triangle1);
                verticals.Add(triangle2);
                verticals.Add(triangle3);
            }

            // Use the new list to run the same tests as for the original.
            for ( int i = 0; i < verticals.Count; i++)
            {
                int[] numbers = verticals[i];
                if (numbers[2] < numbers[1] + numbers[0])
                    if (numbers[1] < numbers[2] + numbers[0])
                        if (numbers[0] < numbers[2] + numbers[1])
                            possibleVerticals++;
            }

            Console.WriteLine("Numbers of possible horizontal triangles is " + possibleTriangles + " out of " + input.Count + " given outputs.");
            Console.WriteLine("Numbers of possible vertical triangles is " + possibleVerticals + " out of " + verticals.Count + " given outputs.");
        }
    }
}
