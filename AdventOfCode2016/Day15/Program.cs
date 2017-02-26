using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
--- Day 15: Timing is Everything ---

The halls open into an interior plaza containing a large kinetic sculpture. The sculpture is in a sealed enclosure and
seems to involve a set of identical spherical capsules that are carried to the top and allowed to bounce through the
maze of spinning pieces.

Part of the sculpture is even interactive! When a button is pressed, a capsule is dropped and tries to fall through
slots in a set of rotating discs to finally go through a little hole at the bottom and come out of the sculpture. If
any of the slots aren't aligned with the capsule as it passes, the capsule bounces off the disc and soars away. You
feel compelled to get one of those capsules.

The discs pause their motion each second and come in different sizes; they seem to each have a fixed number of
positions at which they stop. You decide to call the position with the slot 0, and count up for each position it
reaches next.

Furthermore, the discs are spaced out so that after you push the button, one second elapses before the first disc is
reached, and one second elapses as the capsule passes from one disc to the one below it. So, if you push the button at
time=100, then the capsule reaches the top disc at time=101, the second disc at time=102, the third disc at time=103,
and so on.

The button will only drop a capsule at an integer time - no fractional seconds allowed.

For example, at time=0, suppose you see the following arrangement:

Disc #1 has 5 positions; at time=0, it is at position 4.
Disc #2 has 2 positions; at time=0, it is at position 1.

If you press the button exactly at time=0, the capsule would start to fall; it would reach the first disc at time=1.
Since the first disc was at position 4 at time=0, by time=1 it has ticked one position forward. As a five-position
disc, the next position is 0, and the capsule falls through the slot.

Then, at time=2, the capsule reaches the second disc. The second disc has ticked forward two positions at this point:
it started at position 1, then continued to position 0, and finally ended up at position 1 again. Because there's only
a slot at position 0, the capsule bounces away.

If, however, you wait until time=5 to push the button, then when the capsule reaches each disc, the first disc will
have ticked forward 5+1 = 6 times (to position 0), and the second disc will have ticked forward 5+2 = 7 times (also to
position 0). In this case, the capsule would fall through the discs and come out of the machine.

However, your situation has more than two discs; you've noted their positions in your puzzle input. What is the first
time you can press the button to get a capsule?

--- Part Two ---

After getting the first capsule (it contained a star! what great fortune!), the machine detects your success and begins
to rearrange itself.

When it's done, the discs are back in their original configuration as if it were time=0 again, but a new disc with 11 
positions and starting at position 0 has appeared exactly one second below the previously-bottom disc.

With this new disc, and counting again starting from time=0 with the configuration in your puzzle input, what is the
first time you can press the button to get another capsule?

*/
namespace Day15
{
    class Program
    {
        public struct Disc
        {
            public Disc( int states, int offset)
            {
                Offset = offset;
                NumberOfStates = states;
            }

            public int Offset;
            public int NumberOfStates;
        }

        static void Main(string[] args)
        {
            // Declare false for part 1.
            bool part2 = true;
            List<string> input = new List<string>();
            string line = "";
            try
            {
                using (StreamReader sr = new StreamReader("input.txt"))
                {
                    while ( sr.Peek() >= 0 )
                    {
                        line = sr.ReadLine();
                        input.Add(line);
                    }
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine("Could not read file: ");
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }

            List<Disc> discs = new List<Disc>();

            foreach ( string s in input )
            {
                string[] parts = s.Split(' ');
                int positions = int.Parse(parts[3]);
                parts = parts[11].Split('.');
                int offset = int.Parse(parts[0]);
                Disc disc = new Disc(positions, offset);
                discs.Add(disc);
            }

            if ( part2 )
            {
                discs.Add(new Disc(11, 0));
            }

            bool found = false;

            int time = 0;

            foreach ( Disc disc in discs )
            {
                Console.WriteLine("States: " + disc.NumberOfStates + " Offset: " + disc.Offset);
            }

            while ( !found )
            {
                // Test to see if
                bool test = true;
                // Check each disc in discs to see if the capsule passes through it. If any of the discs does not
                // satisfy the calculation, break the loop and continue.
                for ( int i = 0; i < discs.Count; i++)
                {
                    // For clarity:
                    // current time + drop delay (1 time) + offset of current disc + number of discs passed (1 time /
                    // disc)                     // divided by number of possible states the disc has. If the remnant 
                    // is zero, the capsule passes through. Otherwise, it bounces off and test ends up false. If all 
                    // discs are passed, the current time is the correct answer.
                    if ( (discs[i].Offset+i+1+time) % discs[i].NumberOfStates != 0 )
                    {
                        test = false;
                        break;
                    }
                }
                if (test)
                {
                    found = test;
                }
                else
                {
                    time++;
                }
            }

            Console.WriteLine("First opportunity to catch a ball at time " + time);
        }


    }
}
