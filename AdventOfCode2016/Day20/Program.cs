using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
--- Day 20: Firewall Rules ---

You'd like to set up a small hidden computer here so you can use it to get back into the network later. However, the
corporate firewall only allows communication with certain external IP addresses.

You've retrieved the list of blocked IPs from the firewall, but the list seems to be messy and poorly maintained, and
it's not clear which IPs are allowed. Also, rather than being written in dot-decimal notation, they are written as
plain 32-bit integers, which can have any value from 0 through 4294967295, inclusive.

For example, suppose only the values 0 through 9 were valid, and that you retrieved the following blacklist:

5-8
0-2
4-7

The blacklist specifies ranges of IPs (inclusive of both the start and end value) that are not allowed. Then, the only
IPs that this firewall allows are 3 and 9, since those are the only numbers not in any range.

Given the list of blocked IPs you retrieved from the firewall (your puzzle input), what is the lowest-valued IP that is
not blocked?

--- Part Two ---

How many IPs are allowed by the blacklist?

*/

namespace Day20
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> input = new List<string>();
            string line = "";
            try
            {
                using ( StreamReader sr = new StreamReader("input.txt"))
                {
                    while ( sr.Peek() >= 0)
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

            // Create list of blacklists ranges from the input.
            Dictionary<UInt64, UInt64> blacklists = new Dictionary<UInt64, UInt64>();
            foreach ( string s in input )
            {
                string[] pair = s.Split('-');
                blacklists.Add(UInt64.Parse(pair[0]), UInt64.Parse(pair[1]));
            }

            bool part2 = true; // Flip to false for part 1.
            bool found = false;
            UInt64 firstFree = 0;
            UInt64 freeCount = 0;

            // Keep going until the blacklist is either found, or 
            while ( !found && blacklists.Count > 0 )
            {
                // Take the lowest starting value of a blacklist range.
                UInt64 start = blacklists.Keys.Min();

                // If the starting address is smaller or equivalent to our assumed first free address, adjust our
                // assumptions.
                if (start <= firstFree )
                {
                    // If the end of the blacklist range is larger than our current assumed first free address, the
                    // current candidate for first free address becomes the IP after the end of the blacklist range.
                    if (blacklists[start] > firstFree)
                    {
                        firstFree = blacklists[start] + 1;
                    }
                    blacklists.Remove(start);
                }
                // Part 1: First free address found.
                else if ( !part2 )
                {
                    found = true;
                }
                // Part 2: Add the range of IPs between firstFree and start of the blacklist to the count of
                // unblacklisted addresses.
                else
                {
                    freeCount += start - firstFree;
                    firstFree = blacklists[start] + 1;
                    blacklists.Remove(start);
                    // If the list of blacklists has been exhausted, finish up the count.
                    if ( blacklists.Count == 0 )
                    {
                        // Decrement to take into account that we are comparing last closed vs. full range.
                        firstFree--;
                        // Add the rest of the IP range - if any - to the free count.
                        freeCount += 4294967295 - firstFree;
                        found = true;
                    }
                }
            }
            // Report whichever value is relevant to the part currently enabled.
            if (part2)
            {
                Console.WriteLine("Amount of free IPs: " + freeCount);
            }
            else
            {
                Console.WriteLine("First free IP at: " + firstFree);
            }
        }
    }
}
