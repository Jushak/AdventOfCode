using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
--- Day 19: An Elephant Named Joseph ---

The Elves contact you over a highly secure emergency channel. Back at the North Pole, the Elves are busy
misunderstanding White Elephant parties.

Each Elf brings a present. They all sit in a circle, numbered starting with position 1. Then, starting with the first
Elf, they take turns stealing all the presents from the Elf to their left. An Elf with no presents is removed from the
circle and does not take turns.

For example, with five Elves (numbered 1 to 5):

  1
5   2
 4 3

- Elf 1 takes Elf 2's present.
- Elf 2 has no presents and is skipped.
- Elf 3 takes Elf 4's present.
- Elf 4 has no presents and is also skipped.
- Elf 5 takes Elf 1's two presents.
- Neither Elf 1 nor Elf 2 have any presents, so both are skipped.
- Elf 3 takes Elf 5's three presents.

So, with five Elves, the Elf that sits starting in position 3 gets all the presents.

With the number of Elves given in your puzzle input, which Elf gets all the presents?

--- Part Two ---

Realizing the folly of their present-exchange rules, the Elves agree to instead steal presents from the Elf directly
across the circle. If two Elves are across the circle, the one on the left (from the perspective of the stealer) is
stolen from. The other rules remain unchanged: Elves with no presents are removed from the circle entirely, and the
other elves move in slightly to keep the circle evenly spaced.

For example, with five Elves (again numbered 1 to 5):

The Elves sit in a circle; Elf 1 goes first:
  1
5   2
 4 3

Elves 3 and 4 are across the circle; Elf 3's present is stolen, being the one to the left. Elf 3 leaves the circle,
and the rest of the Elves move in:
  1           1
5   2  -->  5   2
 4 -          4

Elf 2 steals from the Elf directly across the circle, Elf 5:
  1         1 
-   2  -->     2
  4         4 

Next is Elf 4 who, choosing between Elves 1 and 2, steals from Elf 1:
 -          2  
    2  -->
 4          4

Finally, Elf 2 steals from Elf 4:
 2
    -->  2  
 -

So, with five Elves, the Elf that sits starting in position 2 gets all the presents.

With the number of Elves given in your puzzle input, which Elf now gets all the presents?

Your puzzle input is 3005290.

*/

namespace Day19
{
    class Program
    {
        static void Main(string[] args)
        {
            int puzzleInput = 3005290;
            // puzzleInput = 5; // Debug.
            // puzzleInput = 10; // Debug.
            // puzzleInput = 100 // Debug.
            List<int> elves = new List<int>();
            for ( int i = 1; i <= puzzleInput; i++ )
            {
                elves.Add(i);
            }

            bool part2 = true; // Flip to false for part 1.

            if (!part2)
            {
                int[] list = elves.ToArray();
                bool startingOdd = false;
                while (list.Count() > 1)
                {
                    int[] tmp;
                    if (startingOdd)
                    {
                        tmp = list.Where((value, index) => index % 2 == 1).ToArray();
                    }
                    else
                    {
                        tmp = list.Where((value, index) => index % 2 == 0).ToArray();
                    }
                    if (list.Count() % 2 == 1)
                    {
                        startingOdd = !startingOdd;
                    }
                    list = tmp;
                }

                Console.WriteLine("Last elf is " + list[0]);
            }

            // Brute-force since can't figure out a faster way right now.
            else
            {
                int index = 0;
                int target = 0;
                while ( elves.Count > 1 )
                {
                    target = (index + elves.Count / 2) % elves.Count;
                    elves.RemoveAt(target);
                    
                    // If target was before the current index, do not increment index.
                    if (target > index)
                    {
                        index++;
                    }
                    if ( index >= elves.Count )
                    {
                        index = 0;
                    }
                }

                Console.WriteLine("Last elf is " + elves[0]);
            }
        }
    }
}
