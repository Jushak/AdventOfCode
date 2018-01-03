using System;
using System.Collections.Generic;
using System.IO;

/*

--- Day 23: Coprocessor Conflagration ---
You decide to head directly to the CPU and fix the printer from there. As you get close, you find an experimental 
coprocessor doing so much work that the local programs are afraid it will halt and catch fire. This would cause 
serious issues for the rest of the computer, so you head in and see what you can do.

The code it's running seems to be a variant of the kind you saw recently on that tablet. The general functionality 
seems very similar, but some of the instructions are different:

    - set X Y sets register X to the value of Y.
    - sub X Y decreases register X by the value of Y.
    - mul X Y sets register X to the result of multiplying the value contained in register X by the value of Y.
    - jnz X Y jumps with an offset of the value of Y, but only if the value of X is not zero. (An offset of 2 skips 
    the next instruction, an offset of -1 jumps to the previous instruction, and so on.)

Only the instructions listed above are used. The eight registers here, named a through h, all start at 0.

The coprocessor is currently set to some kind of debug mode, which allows for testing, but prevents it from doing any
meaningful work.

If you run the program (your puzzle input), how many times is the mul instruction invoked?

    Your puzzle answer was 3025.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---
Now, it's time to fix the problem.

The debug mode switch is wired directly to register a. You flip the switch, which makes register a now start at 1 when 
the program is executed.

Immediately, the coprocessor begins to overheat. Whoever wrote this program obviously didn't choose a very efficient 
implementation. You'll need to optimize the program if it has any hope of completing before Santa needs that printer 
working.

The coprocessor's ultimate goal is to determine the final value left in register h once the program completes. 
Technically, if it had that... it wouldn't even need to run the program.

After setting register a to 1, if the program were to run to completion, what value would be left in register h?

    Your puzzle answer was 915.
*/
namespace Day23
{
    class Program
    {
        public static bool IsPrime(long number)
        {
            if (number <= 1)
            {
                return false;
            }
            else if (number <= 3)
            {
                return true;
            }
            else if (number % 2 == 0 || number % 3 == 0)
            {
                return false;
            }
            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 5; i <= boundary; i += 2)
            {
                if (number % i == 0) return false;
            }

            return true;
        }

        static void Main(string[] args)
        {
            string[] input;
            string line;
            string[] delimiter = { " ", "\t" };
            string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            List<string> commands = new List<string>();

            // Read the puzzle input.
            StreamReader file = new StreamReader(filePath + "/input.txt");
            while ((line = file.ReadLine()) != null)
            {
                commands.Add(line);
            }
            file.Dispose();

            // Part 1:
            Dictionary<char, long> registeries = new Dictionary<char, long>();
            int index = 0;
            int count = commands.Count;
            long value = 0;
            long tmp = 0;
            long mulCount = 0;
            registeries.Add('a', 0);
            registeries.Add('b', 0);
            registeries.Add('c', 0);
            registeries.Add('d', 0);
            registeries.Add('e', 0);
            registeries.Add('f', 0);
            registeries.Add('g', 0);
            registeries.Add('h', 0);

            while (index >= 0 && index < count)
            {
                input = commands[index].Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                // Console.WriteLine(commands[index]);
                switch (input[0])
                {
                    case "set":
                        if (!long.TryParse(input[2], out value))
                        {
                            value = registeries[input[2][0]];
                        }
                        registeries[input[1][0]] = value;
                        index++;
                        break;
                    case "sub":
                        if (!long.TryParse(input[2], out value))
                        {
                            value = registeries[input[2][0]];
                        }
                        registeries[input[1][0]] -= value;
                        index++;
                        break;
                    case "mul":
                        if (!long.TryParse(input[2], out value))
                        {
                            value = registeries[input[2][0]];
                        }
                        registeries[input[1][0]] *= value;
                        mulCount++;
                        index++;
                        break;
                    case "jnz":
                        if (!long.TryParse(input[1], out value))
                        {
                            value = registeries[input[1][0]];
                        }
                        if (value != 0)
                        {
                            if (!long.TryParse(input[2], out value))
                            {
                                value = registeries[input[2][0]];
                            }
                            index += (int)value;
                        }
                        else
                        {
                            index++;
                        }
                        break;
                }
            }
            Console.WriteLine("Part 1: Total count of mul instructions invoked: " + mulCount);
            Console.ReadKey();

            // Part 2.
            registeries.Clear();
            long a = 1;
            long b = 105700;
            long c = 122700;
            //long d = 0;
            //long e = 0;
            long f = 0;
            long g = 2;
            long h = 0;
            
            while (b <= c)
            {
                f = 1;
                for (int d = 2; d <= b ;  d++)
                {
                    for (int e = 2; e <= b; e++)
                    {
                        if ((d*e) == b)
                        {
                            f = 0;
                            break;
                        }
                        else if ((d*e) > b)
                        {
                            break;
                        }
                    }
                    if (f == 0)
                    {
                        break;
                    }
                }
                if (f == 0)
                {
                    h++;
                }
                b += 17;
            }
            
            Console.WriteLine("Part 2: value of register h: " + h);
            Console.ReadKey();
        }
    }
}
