using System;
using System.Collections.Generic;
using System.IO;

/*

--- Day 22: Sporifica Virus ---
Diagnostics indicate that the local grid computing cluster has been contaminated with the Sporifica Virus. The grid 
computing cluster is a seemingly-infinite two-dimensional grid of compute nodes. Each node is either clean or infected
by the virus.

To prevent overloading the nodes (which would render them useless to the virus) or detection by system administrators,
exactly one virus carrier moves through the network, infecting or cleaning nodes as it moves. The virus carrier is 
always located on a single node in the network (the current node) and keeps track of the direction it is facing.

To avoid detection, the virus carrier works in bursts; in each burst, it wakes up, does some work, and goes back to 
sleep. The following steps are all executed in order one time each burst:

    - If the current node is infected, it turns to its right. Otherwise, it turns to its left. (Turning is done 
    in-place; the current node does not change.)
    - If the current node is clean, it becomes infected. Otherwise, it becomes cleaned. (This is done after the node is
    considered for the purposes of changing direction.)
    - The virus carrier moves forward one node in the direction it is facing.

Diagnostics have also provided a map of the node infection status (your puzzle input). Clean nodes are shown as .; 
infected nodes are shown as #. This map only shows the center of the grid; there are many more nodes beyond those 
shown, but none of them are currently infected.

The virus carrier begins in the middle of the map facing up.

For example, suppose you are given a map like this:

    ..#
    #..
    ...

Then, the middle of the infinite grid looks like this, with the virus carrier's position marked with [ ]:

    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . . . . # . . .
    . . . #[.]. . . .
    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .

The virus carrier is on a clean node, so it turns left, infects the node, and moves left:

    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . . . . # . . .
    . . .[#]# . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .

The virus carrier is on an infected node, so it turns right, cleans the node, and moves up:

    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . .[.]. # . . .
    . . . . # . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .

Four times in a row, the virus carrier finds a clean, infects it, turns left, and moves forward, ending in the same 
place and still facing up:

    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . #[#]. # . . .
    . . # # # . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .

Now on the same node as before, it sees an infection, which causes it to turn right, clean the node, and move forward:

    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . # .[.]# . . .
    . . # # # . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .

After the above actions, a total of 7 bursts of activity had taken place. Of them, 5 bursts of activity caused an 
infection.

After a total of 70, the grid looks like this, with the virus carrier facing up:

    . . . . . # # . .
    . . . . # . . # .
    . . . # . . . . #
    . . # . #[.]. . #
    . . # . # . . # .
    . . . . . # # . .
    . . . . . . . . .
    . . . . . . . . .

By this time, 41 bursts of activity caused an infection (though most of those nodes have since been cleaned).

After a total of 10000 bursts of activity, 5587 bursts will have caused an infection.

Given your actual map, after 10000 bursts of activity, how many bursts cause a node to become infected? (Do not count 
nodes that begin infected.)

    Your puzzle answer was 5404.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---
As you go to remove the virus from the infected nodes, it evolves to resist your attempt.

Now, before it infects a clean node, it will weaken it to disable your defenses. If it encounters an infected node, 
it will instead flag the node to be cleaned in the future. So:

    - Clean nodes become weakened.
    - Weakened nodes become infected.
    - Infected nodes become flagged.
    - Flagged nodes become clean.

Every node is always in exactly one of the above states.

The virus carrier still functions in a similar way, but now uses the following logic during its bursts of action:

    - Decide which way to turn based on the current node:
        - If it is clean, it turns left.
        - If it is weakened, it does not turn, and will continue moving in the same direction.
        - If it is infected, it turns right.
        - If it is flagged, it reverses direction, and will go back the way it came.

    - Modify the state of the current node, as described above.
    - The virus carrier moves forward one node in the direction it is facing.

Start with the same map (still using . for clean and # for infected) and still with the virus carrier starting in 
the middle and facing up.

Using the same initial state as the previous example, and drawing weakened as W and flagged as F, the middle of the 
infinite grid looks like this, with the virus carrier's position again marked with [ ]:

    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . . . . # . . .
    . . . #[.]. . . .
    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .

This is the same as before, since no initial nodes are weakened or flagged. The virus carrier is on a clean node, 
so it still turns left, instead weakens the node, and moves left:

    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . . . . # . . .
    . . .[#]W . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .

The virus carrier is on an infected node, so it still turns right, instead flags the node, and moves up:

    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . .[.]. # . . .
    . . . F W . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .

This process repeats three more times, ending on the previously-flagged node and facing right:

    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . W W . # . . .
    . . W[F]W . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .

Finding a flagged node, it reverses direction and cleans the node:

    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . W W . # . . .
    . .[W]. W . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .

The weakened node becomes infected, and it continues in the same direction:

    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . W W . # . . .
    .[.]# . W . . . .
    . . . . . . . . .
    . . . . . . . . .
    . . . . . . . . .

Of the first 100 bursts, 26 will result in infection. Unfortunately, another feature of this evolved virus is speed; 
of the first 10000000 bursts, 2511944 will result in infection.

Given your actual map, after 10000000 bursts of activity, how many bursts cause a node to become infected? 
(Do not count nodes that begin infected.)

    Your puzzle answer was 2511672.
*/

namespace Day22
{
    class Program
    {
        static void ExpandMap(List<string> map)
        {
            string emptyLine = new string('.', map.Count + 2);
            for (int i = 0; i < map.Count; i++)
            {
                map[i] = map[i].Insert(0, ".");
                map[i] += ".";
            }
            map.Insert(0, emptyLine);
            map.Add(emptyLine);
        }

        static char[,] Expand2dMap(char[,] map)
        {
            int size = map.GetLength(0) + 10;
            char[,] newMap = new char[size, size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    newMap[y, x] = '.';
                }
            }
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    newMap[y + 5, x + 5] = map[y, x];
                }
            }
            Console.WriteLine("Created new map of size " + newMap.GetLength(0));
            return newMap;
        }

        static char TurnLeft(char direction)
        {
            switch (direction)
            {
                case 'n':
                    return 'w';
                case 'e':
                    return 'n';
                case 's':
                    return 'e';
                case 'w':
                    return 's';
            }
            return 'x';
        }

        static char TurnRight(char direction)
        {
            switch (direction)
            {
                case 'n':
                    return 'e';
                case 'e':
                    return 's';
                case 's':
                    return 'w';
                case 'w':
                    return 'n';
            }
            return 'x';
        }

        static char UTurn(char direction)
        {
            switch (direction)
            {
                case 'n':
                    return 's';
                case 'e':
                    return 'w';
                case 's':
                    return 'n';
                case 'w':
                    return 'e';
            }
            return 'x';
        }

        static void SpreadInfection(int x, int y, List<string> map)
        {
            string insertion = map[y][x].Equals('#') ? "." : "#";
            string tmp = map[y].Remove(x, 1);
            tmp = tmp.Insert(x, insertion);
            map[y] = tmp;
        }

        static bool EvolvedInfection(int x, int y, char[,] map)
        {
            bool infected = false;
            char insertion = 'x';
            switch (map[y, x])
            {
                case '.':
                    insertion = 'W';
                    break;
                case 'W':
                    insertion = '#';
                    infected = true;
                    break;
                case '#':
                    insertion = 'F';
                    break;
                case 'F':
                    insertion = '.';
                    break;
            }
            map[y, x] = insertion;
            return infected;
        }

        static void Main(string[] args)
        {
            string line;
            string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            List<string> map = new List<string>();

            // Read the puzzle input.
            StreamReader file = new StreamReader(filePath + "/input.txt");
            while ((line = file.ReadLine()) != null)
            {
                map.Add(line);
            }
            file.Dispose();

            char direction = 'n';
            int myX = map.Count / 2;
            int myY = map.Count / 2;

            int bursts = 0;
            int infections = 0;
            int burstTarget = 10000;

            while (bursts < burstTarget)
            {
                // First, turn.
                if (map[myY][myX].Equals('#'))
                {
                    direction = TurnRight(direction);
                }
                else
                {
                    direction = TurnLeft(direction);
                }

                // Infect.
                SpreadInfection(myX, myY, map);
                if (map[myY][myX].Equals('#'))
                {
                    infections++;
                }

                // Finally, move.
                switch (direction)
                {
                    case 'n':
                        myY--;
                        break;
                    case 'e':
                        myX++;
                        break;
                    case 's':
                        myY++;
                        break;
                    case 'w':
                        myX--;
                        break;
                    case 'x':
                        Console.WriteLine("Something went wrong!");
                        Console.ReadKey();
                        Environment.Exit(0);
                        break;
                }
                if (myX < 0)
                {
                    ExpandMap(map);
                    myX = 0;
                    myY++;
                }
                else if (myY < 0)
                {
                    ExpandMap(map);
                    myY = 0;
                    myX++;
                }
                else if (myX >= map.Count)
                {
                    ExpandMap(map);
                    myX++;
                    myY++;
                }
                else if (myY >= map.Count)
                {
                    ExpandMap(map);
                    myY++;
                    myX++;
                }
                bursts++;
            }

            Console.WriteLine("Part 1: Number of infections: " + infections);
            Console.ReadKey();

            // Part 2.
            map.Clear();

            // Refresh map back to start situation.
            file = new StreamReader(filePath + "/input.txt");
            while ((line = file.ReadLine()) != null)
            {
                map.Add(line);
            }
            file.Dispose();

            // Reset variables to starting values.
            direction = 'n';
            bursts = 0;
            infections = 0;
            burstTarget = 10000000;
            myX = map.Count / 2;
            myY = map.Count / 2;


            // For performance reasons, turn to 2dArray rather than list of strings.
            char[,] map2d = new char[map.Count, map.Count];
            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map.Count; x++)
                {
                    map2d[y, x] = map[y][x];
                }
            }

            while (bursts < burstTarget)
            {
                // Determine direction
                if (map2d[myY,myX].Equals('#'))
                {
                    direction = TurnRight(direction);
                }
                else if (map2d[myY, myX].Equals('.'))
                {
                    direction = TurnLeft(direction);
                }
                else if (map2d[myY, myX].Equals('F'))
                {
                    direction = UTurn(direction);
                }

                // Infect
                if (EvolvedInfection(myX, myY, map2d))
                {
                    infections++;
                }

                // Finally, move.
                switch (direction)
                {
                    case 'n':
                        myY--;
                        break;
                    case 'e':
                        myX++;
                        break;
                    case 's':
                        myY++;
                        break;
                    case 'w':
                        myX--;
                        break;
                    case 'x':
                        Console.WriteLine("Something went wrong!");
                        Console.ReadKey();
                        Environment.Exit(0);
                        break;
                }
                if (myX < 0)
                {
                    map2d = Expand2dMap(map2d);
                    myX = 4;
                    myY += 5;
                }
                else if (myY < 0)
                {
                    map2d = Expand2dMap(map2d);
                    myY = 4;
                    myX += 5;
                }
                else if (myX >= map2d.GetLength(0))
                {
                    map2d = Expand2dMap(map2d);
                    myX += 5;
                    myY += 5;
                }
                else if (myY >= map2d.GetLength(0))
                {
                    map2d = Expand2dMap(map2d);
                    myY += 5;
                    myX += 5;
                }
                bursts++;
            }

            Console.WriteLine("Part 2: Infections " + infections);
            Console.ReadKey();
        }
    }
}
