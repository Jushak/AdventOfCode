using System;
/*
--- Day 3: Spiral Memory ---

You come across an experimental new kind of memory stored on an infinite two-dimensional grid.

Each square on the grid is allocated in a spiral pattern starting at a location marked 1 and then counting up while 
spiraling outward. For example, the first few squares are allocated like this:

17  16  15  14  13
18   5   4   3  12
19   6   1   2  11
20   7   8   9  10
21  22  23---> ...

While this is very space-efficient (no squares are skipped), requested data must be carried back to square 1 
(the location of the only access port for this memory system) by programs that can only move up, down, left, or right.
They always take the shortest path: the Manhattan Distance between the location of the data and square 1.

For example:

- Data from square 1 is carried 0 steps, since it's at the access port.
- Data from square 12 is carried 3 steps, such as: down, left, left.
- Data from square 23 is carried only 2 steps: up twice.
- Data from square 1024 must be carried 31 steps.

How many steps are required to carry the data from the square identified in your puzzle input all the way to 
the access port?

    Your puzzle answer was 552.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---

As a stress test on the system, the programs here clear the grid and then store the value 1 in square 1. Then, 
in the same allocation order as shown above, they store the sum of the values in all adjacent squares, including
diagonals.

So, the first few squares' values are chosen as follows:

Square 1 starts with the value 1.
Square 2 has only one adjacent filled square (with value 1), so it also stores 1.
Square 3 has both of the above squares as neighbors and stores the sum of their values, 2.
Square 4 has all three of the aforementioned squares as neighbors and stores the sum of their values, 4.
Square 5 only has the first and fourth squares as neighbors, so it gets the value 5.
Once a square is written, its value does not change. Therefore, the first few squares would receive the following 
values:

147  142  133  122   59
304    5    4    2   57
330   10    1    1   54
351   11   23   25   26
362  747  806--->   ...

What is the first value written that is larger than your puzzle input?

    Your puzzle answer was 330785.

Your puzzle input is 325489. 
 */
namespace Day3
{
    class Program
    {
        static int[,] ResizeArray(int[,] array)
        {
            int newDimensions = array.GetLength(0) + 2;
            int[,] newArray = new int[newDimensions, newDimensions];
            for (int i = 0; i < newDimensions; i++)
            {
                for (int j = 0; j < newDimensions; j++)
                {
                    if (i == 0 || j == 0 || i == newDimensions - 1 || j == newDimensions - 1)
                    {
                        newArray[i, j] = 0;
                    }
                    else
                    {
                        newArray[j, i] = array[j - 1, i - 1];
                    }
                }
            }
            return newArray;
        }

        public static void Print2DArray<T>(T[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            double input = 325489;
            double power = 2;
            double n = 0;
            double cap = 0;
            double circle = 0;
            while (input > cap)
            {
                n++;
                cap = Math.Pow(2 * n + 1, power);
            }
            circle = 8 * n;
            double position = input - Math.Pow(2 * (n - 1) + 1, power);
            double quarter = circle / 4;
            double middle = quarter / 2;
            while (position > quarter)
            {
                position -= quarter;
            }
            position = Math.Abs(position - middle);
            double steps = position + n;
            Console.WriteLine(steps);

            Console.ReadKey();

            int spiralW = 1;
            int spiralH = 1;
            int x = 0;
            int y = 0;
            char direction = 'E';

            int nextCell = 0;
            int[,] spiral = { { 1 } };
            while (nextCell < input)
            {
                switch (direction)
                {
                    case 'N':
                        y--;
                        break;
                    case 'E':
                        x++;
                        break;
                    case 'S':
                        y++;
                        break;
                    case 'W':
                        x--;
                        break;
                }
                if (x >= spiralW)
                {
                    spiral = ResizeArray(spiral);
                    direction = 'N';
                    spiralW += 2;
                    spiralH += 2;
                    y++;
                    x++;
                }
                else if (y == 0 && direction.Equals('N'))
                {
                    direction = 'W';
                }
                else if (x == 0 && direction.Equals('W'))
                {
                    direction = 'S';
                }
                else if (y >= spiralH)
                {
                    y--;
                    x++;
                    direction = 'E';
                }
                nextCell = 0;
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if ((x + i) >= 0 && (x + i) < spiralW && (y + j) >= 0 && (y + j) < spiralH)
                        {
                            nextCell += spiral[y + j, x + i];
                        }
                    }
                }
                spiral[y, x] = nextCell;
                Print2DArray(spiral);

                Console.WriteLine();
                Console.WriteLine("**************************************************************");
                Console.WriteLine();

            }

            Console.WriteLine(nextCell);
            Console.ReadKey();
        }
    }
}
