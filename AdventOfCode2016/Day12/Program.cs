using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
--- Day 12: Leonardo's Monorail ---

You finally reach the top floor of this building: a garden with a slanted glass ceiling. Looks like there are no more
stars to be had.

While sitting on a nearby bench amidst some tiger lilies, you manage to decrypt some of the files you extracted from
the servers downstairs.

According to these documents, Easter Bunny HQ isn't just this building - it's a collection of buildings in the nearby
area. They're all connected by a local monorail, and there's another building not far from here! Unfortunately, being
night, the monorail is currently not operating.

You remotely connect to the monorail control systems and discover that the boot sequence expects a password. The 
password-checking logic (your puzzle input) is easy to extract, but the code it uses is strange: it's assembunny code
designed for the new computer you just assembled. You'll have to execute the code and get the password.

The assembunny code you've extracted operates on four registers (a, b, c, and d) that start at 0 and can hold any 
integer. However, it seems to make use of only a few instructions:

- cpy x y copies x (either an integer or the value of a register) into register y.
- inc x increases the value of register x by one.
- dec x decreases the value of register x by one.
- jnz x y jumps to an instruction y away (positive means forward; negative means backward), but only if x is not zero.

The jnz instruction moves relative to itself: an offset of -1 would continue at the previous instruction, while an
offset of 2 would skip over the next instruction.

For example:

cpy 41 a
inc a
inc a
dec a
jnz a 2
dec a

The above code would set register a to 41, increase its value by 2, decrease its value by 1, and then skip the last dec
a (because a is not zero, so the jnz a 2 skips it), leaving register a at 42. When you move past the last instruction,
the program halts.

After executing the assembunny code in your puzzle input, what value is left in register a?

--- Part Two ---

As you head down the fire escape to the monorail, you notice it didn't start; register c needs to be initialized to the
position of the ignition key.

If you instead initialize register c to be 1, what value is now left in register a?

*/

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> input = new List<string>();
            string line = "";
            try
            {
                using (StreamReader sr = new StreamReader("input.txt"))
                {
                    while ( sr.Peek() >= 0 )
                    {
                        line = sr.ReadLine();
                        input.Add(line);
                    }
                }
            }
            catch ( Exception e)
            {
                Console.WriteLine("File could not be read :");
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }

            Console.WriteLine("Lines in input: " + input.Count());

            // Set up registeries.
            Int32 a = 0;
            Int32 b = 0;
            Int32 c = 1; // For part 1: set to 0.
            Int32 d = 0;
            Int32 number = 0;

            // Go through each command one by one.
            for ( int i = 0; i < input.Count; i++ )
            {
                number = 0;
                string[] command = input[i].Split(' ');
                if ( command[0] == "cpy")
                {
                    if ( !int.TryParse(command[1], out number))
                    {
                        switch (command[1])
                        {
                            case "a":
                                number = a;
                                break;
                            case "b":
                                number = b;
                                break;
                            case "c":
                                number = c;
                                break;
                            case "d":
                                number = d;
                                break;
                            default:
                                System.Diagnostics.Debug.WriteLine("CPY1 - invalid register");
                                Environment.Exit(0);
                                break;
                        }
                    }
                    switch (command[2])
                    {
                        case "a":
                            a = number;
                            break;
                        case "b":
                            b = number;
                            break;
                        case "c":
                            c = number;
                            break;
                        case "d":
                            d = number;
                            break;
                        default:
                            System.Diagnostics.Debug.WriteLine("CPY2 - invalid register @ line "+i);
                            Environment.Exit(0);
                            break;
                    }
                }
                else if ( command[0] == "inc")
                {
                    switch (command[1])
                    {
                        case "a":
                            a++;
                            break;
                        case "b":
                            b++;
                            break;
                        case "c":
                            c++;
                            break;
                        case "d":
                            d++;
                            break;
                        default:
                            System.Diagnostics.Debug.WriteLine("INC - invalid register");
                            Environment.Exit(0);
                            break;
                    }
                }
                else if ( command[0] == "dec")
                {
                    switch (command[1])
                    {
                        case "a":
                            a--;
                            break;
                        case "b":
                            b--;
                            break;
                        case "c":
                            c--;
                            break;
                        case "d":
                            d--;
                            break;
                        default:
                            System.Diagnostics.Debug.WriteLine("DEC - invalid register");
                            Environment.Exit(0);
                            break;
                    }
                }
                else if ( command[0] == "jnz")
                {
                    if (!int.TryParse(command[1], out number))
                    {
                        switch (command[1])
                        {
                            case "a":
                                number = a;
                                break;
                            case "b":
                                number = b;
                                break;
                            case "c":
                                number = c;
                                break;
                            case "d":
                                number = d;
                                break;
                            default:
                                System.Diagnostics.Debug.WriteLine("JNZ - invalid register @ line " + i);
                                System.Diagnostics.Debug.WriteLine(input[i]);
                                Environment.Exit(0);
                                break;
                        }
                    }
                    if ( number != 0 )
                    {
                        number = int.Parse(command[2]);
                        i += number;
                        i--;
                    }
                }
                else
                {
                    Console.WriteLine("Unknown command");
                    Environment.Exit(0);
                }
            }

            Console.WriteLine("Values of registers are: "+a+" "+b+" "+c+" "+d);
            System.Diagnostics.Debug.WriteLine("Values of registers are: " + a + " " + b + " " + c + " " + d);
            Environment.Exit(0);

        }
    }
}
