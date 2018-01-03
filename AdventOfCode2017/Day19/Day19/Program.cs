using System;
using System.Collections.Generic;
using System.IO;

/*
 
--- Day 19: A Series of Tubes ---
Somehow, a network packet got lost and ended up here. It's trying to follow a routing diagram (your puzzle input), 
but it's confused about where to go.

Its starting point is just off the top of the diagram. Lines (drawn with |, -, and +) show the path it needs to take,
starting by going down onto the only line connected to the top of the diagram. It needs to follow this path until 
it reaches the end (located somewhere within the diagram) and stop there.

Sometimes, the lines cross over each other; in these cases, it needs to continue going the same direction, and only 
turn left or right when there's no other option. In addition, someone has left letters on the line; these also don't
change its direction, but it can use them to keep track of where it's been. For example:

         |          
         |  +--+    
         A  |  C    
     F---|----E|--+ 
         |  |  |  D 
         +B-+  +--+ 

Given this diagram, the packet needs to take the following path:
    
    - Starting at the only line touching the top of the diagram, it must go down, pass through A, and continue onward 
    to the first +.
    - Travel right, up, and right, passing through B in the process.
    - Continue down (collecting C), right, and up (collecting D).
    - Finally, go all the way left through E and stopping at F.
    
Following the path to the end, the letters it sees on its path are ABCDEF.

The little packet looks up at you, hoping you can help it find the way. What letters will it see (in the order it would
see them) if it follows the path? (The routing diagram is very wide; make sure you view it without line wrapping.)

    Your puzzle answer was 16064.

*/

namespace Day19
{
    class Program
    {
        static bool IsDirectionPossible(char[,] diagram, int x, int y, char direction)
        {
            bool result = false;
            switch (direction)
            {
                case 'n':
                    y--;
                    result = (y >= 0 && !diagram[x, y].Equals(' '));
                    break;
                case 'e':
                    x++;
                    result = (x < diagram.GetLength(0) && !diagram[x, y].Equals(' '));
                    break;
                case 's':
                    y++;
                    result = (y < diagram.GetLength(1) && !diagram[x, y].Equals(' '));
                    break;
                case 'w':
                    x--;
                    result = (x >= 0 && !diagram[x, y].Equals(' '));
                    break;
            }
            return result;
        }

        static char TurnLeft(char direction)
        {
            char newDirection = 'x';
            switch (direction)
            {
                case 'n':
                    newDirection = 'w';
                    break;
                case 'e':
                    newDirection = 'n';
                    break;
                case 's':
                    newDirection = 'e';
                    break;
                case 'w':
                    newDirection = 's';
                    break;
            }
            return newDirection;
        }

        static char TurnRight(char direction)
        {
            char newDirection = 'x';
            switch (direction)
            {
                case 'n':
                    newDirection = 'e';
                    break;
                case 'e':
                    newDirection = 's';
                    break;
                case 's':
                    newDirection = 'w';
                    break;
                case 'w':
                    newDirection = 'n';
                    break;
            }
            return newDirection;
        }

        static char DetermineDirection(char[,] diagram, int x, int y, char direction)
        {
            if (IsDirectionPossible(diagram, x, y, direction))
            {
                return direction;
            }
            else if (IsDirectionPossible(diagram, x, y, TurnLeft(direction)))
            {
                return TurnLeft(direction);
            }
            else if (IsDirectionPossible(diagram, x, y, TurnRight(direction)))
            {
                return TurnRight(direction);
            }
            return 'x';
        }

        static void Main(string[] args)
        {
            // Read the puzzle input.
            string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            string line;
            List<string> lines = new List<string>();
            StreamReader file = new StreamReader(filePath + "/input.txt");
            while ((line = file.ReadLine()) != null)
            {
                lines.Add(line);
            }
            int width = lines[0].Length;
            int height = lines.Count;
            char[,] diagram = new char[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    diagram[x, y] = lines[y][x];
                }
            }

            int myX = lines[0].IndexOf('|');
            int myY = 0;
            char direction = 's';
            string letters = "";
            int steps = 0;

            while (!direction.Equals('x'))
            {
                // First, move forward.
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
                }
                // Then, look at our location.
                if (char.IsLetter(diagram[myX, myY]))
                {
                    letters += diagram[myX, myY];
                    if (!IsDirectionPossible(diagram, myX, myY, direction))
                    {
                        direction = 'x';
                    }
                }
                else
                {
                    direction = DetermineDirection(diagram, myX, myY, direction);
                }
                steps++;
            }
            steps++; // Final step.
            Console.WriteLine("Part 1: The packet sees these letters in order: "+letters);
            Console.WriteLine("Part 2: Steps taken: " + steps);
            Console.ReadKey();


        }
    }
}
