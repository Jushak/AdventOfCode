using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
--- Day 7: Internet Protocol Version 7 ---

While snooping around the local network of EBHQ, you compile a list of IP addresses (they're IPv7, of course; IPv6 is 
much too limited). You'd like to figure out which IPs support TLS (transport-layer snooping).

An IP supports TLS if it has an Autonomous Bridge Bypass Annotation, or ABBA. An ABBA is any four-character sequence
which consists of a pair of two different characters followed by the reverse of that pair, such as xyyx or abba. 
However, the IP also must not have an ABBA within any hypernet sequences, which are contained by square brackets.

For example:

- abba[mnop]qrst supports TLS (abba outside square brackets).
- abcd[bddb]xyyx does not support TLS (bddb is within square brackets, even though xyyx is outside square brackets).
- aaaa[qwer]tyui does not support TLS (aaaa is invalid; the interior characters must be different).
- ioxxoj[asdfgh]zxcvbn supports TLS (oxxo is outside square brackets, even though it's within a larger string).

How many IPs in your puzzle input support TLS?

--- Part Two ---

You would also like to know which IPs support SSL (super-secret listening).

An IP supports SSL if it has an Area-Broadcast Accessor, or ABA, anywhere in the supernet sequences (outside any square
bracketed sections), and a corresponding Byte Allocation Block, or BAB, anywhere in the hypernet sequences. An ABA is 
any three-character sequence which consists of the same character twice with a different character between them, such 
as xyx or aba. A corresponding BAB is the same characters but in reversed positions: yxy and bab, respectively.

For example:

- aba[bab]xyz supports SSL (aba outside square brackets with corresponding bab within square brackets).
- xyx[xyx]xyx does not support SSL (xyx, but no corresponding yxy).
- aaa[kek]eke supports SSL (eke in supernet with corresponding kek in hypernet; the aaa sequence is not related, because 
the interior character must be different).
- zazbz[bzb]cdb supports SSL (zaz has no corresponding aza, but zbz has a corresponding bzb, even though zaz and zbz 
overlap).

How many IPs in your puzzle input support SSL?
 
*/

namespace Day7
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
                        input.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("File could not be read: ");
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }

            int supportingTLS = 0;
            int supportingSSL = 0;

            // Check for part 1 and part 2 at the same time.
            foreach ( string s in input )
            {
                string[] current = s.Split('[', ']');
                if ( supportsTLS( current ))
                    supportingTLS++;
                if (supportsSSL(current))
                    supportingSSL++;
            }

            Console.WriteLine("Total of " + supportingTLS + " out of " + input.Count + " IPs support ABBA.");
            Console.WriteLine("Total of " + supportingSSL + " out of " + input.Count + " support SSL");
        }

        #region TLS
        // Function for TLS.
        public static bool supportsTLS(string[] lineParts)
        {
            bool unbracketedABBA = false;
            for (int i = 0; i < lineParts.Count(); i++)
            {
                // Odd-numbered indexes are within brackets: if one contains ABBA, the entire line fails.
                if (i % 2 != 0 && containsABBA(lineParts[i]))
                    return false;
                // Even-numbered indexes are outside bracket: if one contains ABBA, the line may support TLS, but all
                // odd indexes still need to be checked. If unbracketed ABBA has been found, skip futher checks on even
                // indexes as unnecessary.
                else if (!unbracketedABBA && i % 2 == 0 && containsABBA(lineParts[i]))
                    unbracketedABBA = true;
            }
            return unbracketedABBA;
        }

        // TLS helper function.
        public static bool containsABBA(string line)
        {
            if (line.Length < 4)
            {
                return false;
            }

            for (int i = 0 ; i < (line.Length - 3); i++) 
            {
                string test = line.Substring(i, 4);
                if (test[0] == test[3] && test[1] == test[2] && test[0] != test[1])
                    return true;
            }
            return false;
        }
        #endregion

        #region SSL
        // Function for checking if SSL is supported.
        public static bool supportsSSL( string[] lineParts )
        {
            List<string> ABAs = new List<string>();
            for ( int i = 0; i < lineParts.Count(); i+=2)
            {
                ABAs.AddRange(containedABAs(lineParts[i]));
            }
            
            List<string> BABs = new List<string>();
            foreach ( string s in ABAs)
            {
                char outer = s.ElementAt(1);
                char inner = s.ElementAt(0);
                string BAB = (outer.ToString() + inner.ToString() + outer.ToString());
                BABs.Add(BAB);
            }
            
            for (int i = 1; i < lineParts.Count(); i+=2)
            { 
                for ( int j = 0; j < BABs.Count; j++)
                {
                    if (lineParts[i].Contains(BABs[j]))
                        return true;
                }
            }
            return false;
        }

        // SSL helper function.
        public static List<string> containedABAs(string line)
        {
            List<string> ABAs = new List<string>();
            if (line.Length < 3)
            {
                return ABAs;
            }

            for (int i = 0; i < (line.Length - 2); i++)
            {
                string test = line.Substring(i, 3);
                if (test[0] == test[2] && test[0] != test[1])
                {
                    ABAs.Add(test);
                }
            }
            return ABAs;
        }
        #endregion
    }
}
