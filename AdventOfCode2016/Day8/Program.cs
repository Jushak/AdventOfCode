using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
--- Day 8: Two-Factor Authentication ---

You come across a door implementing what you can only assume is an implementation of two-factor authentication after a
long game of requirements telephone.

To get past the door, you first swipe a keycard (no problem; there was one on a nearby desk). Then, it displays a code
on a little screen, and you type that code on a keypad. Then, presumably, the door unlocks.

Unfortunately, the screen has been smashed. After a few minutes, you've taken everything apart and figured out how it
works. Now you just have to work out what the screen would have displayed.

The magnetic strip on the card you swiped encodes a series of instructions for the screen; these instructions are your
puzzle input. The screen is 50 pixels wide and 6 pixels tall, all of which start off, and is capable of three somewhat
peculiar operations:

- rect AxB turns on all of the pixels in a rectangle at the top-left of the screen which is A wide and B tall.
- rotate row y=A by B shifts all of the pixels in row A (0 is the top row) right by B pixels. Pixels that would fall
off the right end appear at the left end of the row.
- rotate column x=A by B shifts all of the pixels in column A (0 is the left column) down by B pixels. Pixels that 
would fall off the bottom appear at the top of the column.

For example, here is a simple sequence on a smaller screen:

rect 3x2 creates a small rectangle in the top-left corner:

###....
###....
.......
rotate column x=1 by 1 rotates the second column down by one pixel:

#.#....
###....
.#.....
rotate row y=0 by 4 rotates the top row right by four pixels:

....#.#
###....
.#.....
rotate column x=1 by 1 again rotates the second column down by one pixel, causing the bottom pixel to wrap back to the 
top:

.#..#.#
#.#....
.#.....
As you can see, this display technology is extremely powerful, and will soon dominate the tiny-code-displaying-screen 
market. That's what the advertisement on the back of the display tries to convince you, anyway.

There seems to be an intermediate check of the voltage used by the display: after you swipe your card, if the screen
did work, how many pixels should be lit?

--- Part Two ---

You notice that the screen is only capable of displaying capital letters; in the font it uses, each letter is 5 pixels wide and 6 tall.

After you swipe your card, what code is the screen trying to display?

*/

namespace Day8
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
                    while (sr.Peek() >= 0)
                    {
                        line = sr.ReadLine();
                        input.Add(line);
                    }
                }
            }
            catch( Exception e )
            {
                Console.WriteLine("Could not read file: ");
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }

            bool[,] lights = new bool[50, 6];

            // Input handling.
            foreach ( string s in input )
            {
                string[] current = s.Split(' ');
                // rect AxB
                if ( current[0] == "rect")
                {
                    string[] values = current[1].Split('x');
                    int x = int.Parse(values[0]);
                    int y = int.Parse(values[1]);
                    for ( int i = 0; i < x; i++)
                    {
                        for (int j = 0; j < y; j++)
                        {
                            lights[i, j] = true;
                        }
                    }
                }
                // rotate row y=A by B
                else if ( current[0] == "rotate" && current[1] == "row")
                {
                    string[] values = current[2].Split('=');
                    int y = int.Parse(values[1]);
                    int repeats = int.Parse(current[4]);
                    for ( int i = 0; i < repeats; i++ )
                    {
                        bool last = lights[49, y];
                        for ( int j = 48; j >= 0; j-- )
                        {
                            lights[j+1,y] = lights[j,y];
                        }
                        lights[0, y] = last;
                    }
                }
                // rotate column y=A by B
                else if (current[0] == "rotate" && current[1] == "column")
                {
                    string[] values = current[2].Split('=');
                    int x = int.Parse(values[1]);
                    int repeats = int.Parse(current[4]);
                    for (int i = 0; i < repeats; i++)
                    {
                        bool last = lights[x, 5];
                        for (int j = 4; j >= 0; j--)
                        {
                            lights[x, j+1] = lights[x, j];
                        }
                        lights[x, 0] = last;
                    }
                }
                else
                {
                    Console.WriteLine("Unidentified command: '"+s+"' - closing program.");
                    Environment.Exit(0);
                }
            }

            // Count the number of lights that are on.
            int lightsOn = 0;
            for (int y = 0; y < 6; y++)
            {
                line = "";
                for (int x = 0; x < 50; x++)
                {
                    if (lights[x, y])
                    {
                        lightsOn++;
                        line += "*";
                    }
                    else
                        line += " ";
                }
                // Print the display for part 2 while counting lights.
                Console.WriteLine(line);
            }
            Console.WriteLine("Total of " + lightsOn + " lights are on.");
        }
    }
}
