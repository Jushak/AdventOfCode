using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

/*

--- Day 17: Two Steps Forward ---

You're trying to access a secure vault protected by a 4x4 grid of small rooms connected by doors. You start in the
top-left room (marked S), and you can access the vault (marked V) once you reach the bottom-right room:

#########
#S| | | #
#-#-#-#-#
# | | | #
#-#-#-#-#
# | | | #
#-#-#-#-#
# | | |  
####### V

Fixed walls are marked with #, and doors are marked with - or |.

The doors in your current room are either open or closed (and locked) based on the hexadecimal MD5 hash of a passcode
(your puzzle input) followed by a sequence of uppercase characters representing the path you have taken so far (U for
up, D for down, L for left, and R for right).

Only the first four characters of the hash are used; they represent, respectively, the doors up, down, left, and right
from your current position. Any b, c, d, e, or f means that the corresponding door is open; any other character (any
number or a) means that the corresponding door is closed and locked.

To access the vault, all you need to do is reach the bottom-right room; reaching this room opens the vault and all
doors in the maze.

For example, suppose the passcode is hijkl. Initially, you have taken no steps, and so your path is empty: you simply
find the MD5 hash of hijkl alone. The first four characters of this hash are ced9, which indicate that up is open (c),
down is open (e), left is open (d), and right is closed and locked (9). Because you start in the top-left corner,
there are no "up" or "left" doors to be open, so your only choice is down.

Next, having gone only one step (down, or D), you find the hash of hijklD. This produces f2bc, which indicates that you
can go back up, left (but that's a wall), or right. Going right means hashing hijklDR to get 5745 - all doors closed
and locked. However, going up instead is worthwhile: even though it returns you to the room you started in, your path
would then be DU, opening a different set of doors.

After going DU (and then hashing hijklDU to get 528e), only the right door is open; after going DUR, all doors lock.
(Fortunately, your actual passcode is not hijkl).

Passcodes actually used by Easter Bunny Vault Security do allow access to the vault if you know the right path. For
example:

- If your passcode were ihgpwlah, the shortest path would be DDRRRD.
- With kglvqrro, the shortest path would be DDUDRLRRUDRD.
- With ulqzkmiv, the shortest would be DRURDRUDDLLDLUURRDULRLDUUDDDRR.

Given your vault's passcode, what is the shortest path (the actual path, not just the length) to reach the vault?

--- Part Two ---

You're curious how robust this security solution really is, and so you decide to find longer and longer paths which
still provide access to the vault. You remember that paths always end the first time they reach the bottom-right room
(that is, they can never pass through it, only end in it).

For example:

If your passcode were ihgpwlah, the longest path would take 370 steps.
With kglvqrro, the longest path would be 492 steps long.
With ulqzkmiv, the longest path would be 830 steps long.
What is the length of the longest path that reaches the vault?

Your puzzle input is pgflpeqp.

*/

namespace Day17
{
    class Program
    {
        public struct Path
        {
            public Path(int startX, int startY, string startRoute)
            {
                x = startX;
                y = startY;
                route = startRoute;
            }
            public int x;
            public int y;
            public string route;
        }

        static void Main(string[] args)
        {
            string passcode = "pgflpeqp";
            // passcode = "ihgpwlah"; // Debug 1.
            // passcode = "kglvqrro"; // Debug 2.

            bool part2 = true; // Flip to false for part 1

            Queue<Path> unsearchedPaths = new Queue<Path>();
            unsearchedPaths.Enqueue(new Path(0, 0, ""));

            Path chosenPath = new Path(0, 0, "");

            using (MD5 md5Hash = MD5.Create())
            {
                while (unsearchedPaths.Count > 0)
                {
                    Path current = unsearchedPaths.Dequeue();
                    string hash = GetMd5Hash(md5Hash, passcode + current.route);
                    // Moving up: 
                    if ( current.y > 0  && IsOpen(hash[0]))
                    {
                        Path newPath = new Path(current.x, current.y - 1, current.route + "U");
                        unsearchedPaths.Enqueue(newPath);
                    }
                    // Moving down:
                    if ( current.y < 3 && IsOpen(hash[1]))
                    {
                        Path newPath = new Path(current.x, current.y + 1, current.route + "D");
                        // Since vault can only be reached moving DOWN or RIGHT, check if vault has been reached here.
                        if (newPath.x == 3 && newPath.y == 3)
                        {
                            chosenPath = newPath;
                            if (!part2)
                            {
                                unsearchedPaths.Clear();
                            }
                        }
                        else
                        {
                            unsearchedPaths.Enqueue(newPath);
                        }
                    }
                    // Moving left:
                    if ( current.x > 0 && IsOpen(hash[2]))
                    {
                        Path newPath = new Path(current.x - 1, current.y, current.route + "L");
                        unsearchedPaths.Enqueue(newPath);
                    }
                    // Moving right:
                    if ( current.x < 3 && IsOpen(hash[3]))
                    {
                        Path newPath = new Path(current.x + 1, current.y, current.route + "R");
                        // Since vault can only be reached moving DOWN or RIGHT, check if vault has been reached here.
                        if ( newPath.x == 3 && newPath.y == 3 )
                        {
                            chosenPath = newPath;
                            if (!part2)
                            {
                                unsearchedPaths.Clear();
                            }
                        }
                        else
                        {
                            unsearchedPaths.Enqueue(newPath);
                        }
                    }
                }
            }

            if ( chosenPath.route == "")
            {
                Console.WriteLine("No path could be found.");
                Environment.Exit(0);
            }
            if (!part2)
            {
                Console.WriteLine("Shortest path to the vault with given passcode is: " + chosenPath.route);
            }
            else
            {
                Console.WriteLine("Longest path to the vault with given passcode is: " + chosenPath.route.Length +" steps long");
            }
        }

        static bool IsOpen( char c )
        {
            if ( c >= 'b' && c <= 'f')
            {
                return true;
            }
            return false;
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
