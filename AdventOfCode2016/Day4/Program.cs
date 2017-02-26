using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
--- Day 4: Security Through Obscurity ---

Finally, you come across an information kiosk with a list of rooms. Of course, the list is encrypted and full of decoy
data, but the instructions to decode the list are barely hidden nearby. Better remove the decoy data first.

Each room consists of an encrypted name (lowercase letters separated by dashes) followed by a dash, a sector ID, and
a checksum in square brackets.

A room is real (not a decoy) if the checksum is the five most common letters in the encrypted name, in order, with ties
broken by alphabetization.For example:

aaaaa-bbb-z-y-x-123[abxyz] is a real room because the most common letters are a(5), b(3), and then a tie between x, y,
and z, which are listed alphabetically.

a-b-c-d-e-f-g-h-987[abcde] is a real room because although the letters are all tied (1 of each), the first five are
listed alphabetically.

not-a-real-room-404[oarel] is a real room.

totally-real-room-200[decoy] is not.

Of the real rooms from the list above, the sum of their sector IDs is 1514.


What is the sum of the sector IDs of the real rooms?

--- Part Two ---

With all the decoy data out of the way, it's time to decrypt this list and get moving.

The room names are encrypted by a state-of-the-art shift cipher, which is nearly unbreakable without the right software.
However, the information kiosk designers at Easter Bunny HQ were not expecting to deal with a master cryptographer like
yourself.

To decrypt a room name, rotate each letter forward through the alphabet a number of times equal to the room's sector ID.
A becomes B, B becomes C, Z becomes A, and so on. Dashes become spaces.

For example, the real name for qzmt-zixmtkozy-ivhz-343 is very encrypted name.

What is the sector ID of the room where North Pole objects are stored?

*/

namespace Day4
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

            int sumOfCodes = 0;

            // Part 1: Find out the list of real rooms as well as adding up the sum of their sector IDs.
            List<string[]> realRooms = new List<string[]>();
            for ( int i = 0; i < input.Count; i++)
            {
                string[] current = input[i].Split('[', ']');

                string[] roomParts = current[0].Split('-');
                string roomString = "";
                for (int j = 0; j < roomParts.Count() - 1; j++)
                    roomString += roomParts[j];
                if (code(roomString) == current[1])
                    sumOfCodes += int.Parse(roomParts[roomParts.Count() - 1]);
                realRooms.Add(roomParts);
            }

            // Part 2: With the list of real rooms done in part 1, it's time to decrypt their names.
            Dictionary<string, int> translations = new Dictionary<string, int>();
            string translation = "";
            string part = "";

            for ( int i = 0; i < realRooms.Count; i++)
            {
                translation = "";
                string[] current = realRooms[i];
                for ( int j = 0; j < current.Count()-1; j++ )
                {
                    part = "";
                    foreach (char c in current[j])
                        part += Decrypter(c, int.Parse(current[current.Count() - 1]));

                    translation += part+" ";
                }
                translations.Add(translation, int.Parse(current[current.Count() -1 ]));
            }

            // For ease of browsing, alphabetize the the list.
            IOrderedEnumerable<KeyValuePair<string, int>> alphabetized = translations
                    .OrderBy(x => x.Key);

            // Print out the list: mainly because the instructions didn't specify exact name of the room we're looking
            // for.
            foreach (KeyValuePair<string, int> kvp in alphabetized)
            {
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }

            // Part 1 answer.
            Console.WriteLine("Sum of the codes of the legit rooms: " + sumOfCodes);
        }

        // Code-generator for turning room name into a 5-letter code.
        public static string code ( string roomName)
        {
            Dictionary<char, int> letters = new Dictionary<char, int>();

            // Add up letter counts...
            for (int i = 0; i < roomName.Length; i++)
            {
                if (letters.ContainsKey(roomName.ElementAt(i)))
                {
                    letters[roomName.ElementAt(i)]++;
                }
                else
                {
                    letters.Add(roomName.ElementAt(i), 1);
                }
            }
            string code = "";

            // ...then order them by number of occurences, using alphabetic order in case of ties.
            IOrderedEnumerable<KeyValuePair<char, int>> sortedCollection = letters
                    .OrderByDescending(x => x.Value)
                    .ThenBy(x => x.Key);
            // Finally, pick the first 5 letters from the list to form the code. Since we are assuming all input is
            // legit, no additional checks for ensuring we actually have 5 different letters.
            for (int i = 0; i < 5; i++)
                code += sortedCollection.ElementAt(i).Key;
                        
            return code;
        }

        // Decyption function for room names.
        public static char Decrypter(char alphabet, int code)
        {
            if (alphabet == '-')
                return ' ';
            string alphabets = "abcdefghijklmnopqrstuvwxyz";
            int number = alphabets.IndexOf(alphabet);
            number += code;
            number = (number % 26);
            return alphabets[number];
        }
    }
}
