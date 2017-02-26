using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
Something is jamming your communications with Santa. Fortunately, your signal is only partially jammed, and protocol in
situations like this is to switch to a simple repetition code to get the message through.

In this model, the same message is sent repeatedly. You've recorded the repeating message signal (your puzzle input)
, but the data seems quite corrupted - almost too badly to recover. Almost.

All you need to do is figure out which character is most frequent for each position. For example, suppose you had
recorded the following messages:

eedadn
drvtee
eandsr
raavrd
atevrs
tsrnev
sdttsa
rasrtv
nssdts
ntnada
svetve
tesnvt
vntsnd
vrdear
dvrsen
enarar

The most common character in the first column is e; in the second, a; in the third, s, and so on. Combining these
characters returns the error-corrected message, easter.

Given the recording in your puzzle input, what is the error-corrected version of the message being sent?

--- Part Two ---

Of course, that would be the message - if you hadn't agreed to use a modified repetition code instead.

In this modified code, the sender instead transmits what looks like random data, but for each character, the character
they actually want to send is slightly less likely than the others. Even after signal-jamming noise, you can look at
the letter distributions in each column and choose the least common letter to reconstruct the original message.

In the above example, the least common character in the first column is a; in the second, d, and so on. Repeating this
process for the remaining characters produces the original message, advent.

Given the recording in your puzzle input and this new decoding methodology, what is the original message that Santa
is trying to send?
 
*/

namespace Day6
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

            string message1 = "";
            string message2 = "";
            Dictionary<char, int> letter = new Dictionary<char, int>();

            // Go through each index of input lines and count number of instances of each letter. Then add the most /
            // least common letter to the messages of part 1 and part 2 respectively.
            for (int i = 0; i < input[0].Count(); i++ ) {

                letter.Clear();
                foreach (string s in input)
                {
                    if (!letter.ContainsKey(s[i]))
                        letter.Add(s[i], 1);
                    else
                        letter[s[i]]++;
                }
                message1 += (letter.Aggregate((l, r) => l.Value > r.Value ? l : r).Key);
                message2 += (letter.Aggregate((l, r) => l.Value < r.Value ? l : r).Key);
            }

            Console.WriteLine("The message of part 1 is: " + message1);
            Console.WriteLine("The message of part 2 is: " + message2);
        }
    }
}
