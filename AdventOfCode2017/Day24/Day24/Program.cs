using System;
using System.Collections.Generic;
using System.IO;

/*

--- Day 24: Electromagnetic Moat ---
The CPU itself is a large, black building surrounded by a bottomless pit. Enormous metal tubes extend outward from 
the side of the building at regular intervals and descend down into the void. There's no way to cross, but you need 
to get inside.

No way, of course, other than building a bridge out of the magnetic components strewn about nearby.

Each component has two ports, one on each end. The ports come in all different types, and only matching types can be 
connected. You take an inventory of the components by their port types (your puzzle input). Each port is identified by
the number of pins it uses; more pins mean a stronger connection for your bridge. A 3/7 component, for example, has 
a type-3 port on one side, and a type-7 port on the other.

Your side of the pit is metallic; a perfect surface to connect a magnetic, zero-pin port. Because of this, the first 
port you use must be of type 0. It doesn't matter what type of port you end with; your goal is just to make the bridge
as strong as possible.

The strength of a bridge is the sum of the port types in each component. For example, if your bridge is made of 
components 0/3, 3/7, and 7/4, your bridge has a strength of 0+3 + 3+7 + 7+4 = 24.

For example, suppose you had the following components:

    0/2
    2/2
    2/3
    3/4
    3/5
    0/1
    10/1
    9/10

With them, you could make the following valid bridges:

    0/1
    0/1--10/1
    0/1--10/1--9/10
    0/2
    0/2--2/3
    0/2--2/3--3/4
    0/2--2/3--3/5
    0/2--2/2
    0/2--2/2--2/3
    0/2--2/2--2/3--3/4
    0/2--2/2--2/3--3/5

(Note how, as shown by 10/1, order of ports within a component doesn't matter. However, you may only use each port on
a component once.)

Of these bridges, the strongest one is 0/1--10/1--9/10; it has a strength of 0+1 + 1+10 + 10+9 = 31.

What is the strength of the strongest bridge you can make with the components you have available?

    Your puzzle answer was 1868.

--- Part Two ---
The bridge you've built isn't long enough; you can't jump the rest of the way.

In the example above, there are two longest bridges:

0/2--2/2--2/3--3/4
0/2--2/2--2/3--3/5
Of them, the one which uses the 3/5 component is stronger; its strength is 0+2 + 2+2 + 2+3 + 3+5 = 19.

What is the strength of the longest bridge you can make? If you can make multiple bridges of the longest length, 
pick the strongest one.
*/

namespace Day24
{
    class Component
    {
        public int port1;
        public int port2;
        public Component(int port1, int port2)
        {
            this.port1 = port1;
            this.port2 = port2;
        }
        public bool HasPort(int port)
        {
            if (port1 == port || port2 == port)
            {
                return true;
            }
            return false;
        }
        public int GetStrength()
        {
            return port1 + port2;
        }
        public int GetOtherPort(int port)
        {
            if (port1 == port)
            {
                return port2;
            }
            else if (port2 == port)
            {
                return port1;
            }
            else
            {
                return -1;
            }
        }
    }

    class Result
    {
        public int length;
        public int strength;
        public Result(int length, int strength)
        {
            this.length = length;
            this.strength = strength;
        }
    }

    class Program
    {
        static int CalculateBridgeStrength(int currentPort, int currentStrength, List<Component> components)
        {
            int newStrength = currentStrength;
            foreach(Component c in components)
            {
                if (c.HasPort(currentPort))
                {
                    List<Component> remaining = new List<Component>();
                    remaining.AddRange(components);
                    remaining.Remove(c);
                    int tmp = CalculateBridgeStrength(c.GetOtherPort(currentPort), currentStrength + c.GetStrength(), remaining);
                    if (tmp > newStrength)
                    {
                        newStrength = tmp;
                    }
                }
            }
            return newStrength;
        }

        static Result CalculateLongestBridgeStrength(int currentPort, int currentStrength, int currentLength, List<Component> components)
        {
            Result newResult = new Result(currentLength, currentStrength);
            foreach (Component c in components)
            {
                if (c.HasPort(currentPort))
                {
                    List<Component> remaining = new List<Component>();
                    remaining.AddRange(components);
                    remaining.Remove(c);
                    Result tmp = CalculateLongestBridgeStrength(c.GetOtherPort(currentPort), currentStrength+c.GetStrength(), currentLength+1, remaining);
                    if (tmp.length > newResult.length || (tmp.length == newResult.length && tmp.strength > newResult.strength))
                    {
                        newResult= tmp;
                    }
                }
            }
            return newResult;
        }

        static void Main(string[] args)
        {
            string[] input;
            string line;
            string[] delimiter = { " ", "\t", "/" };
            string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            List<Component> components = new List<Component>();

            // Read the puzzle input.
            StreamReader file = new StreamReader(filePath + "/input.txt");
            while ((line = file.ReadLine()) != null)
            {
                input = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                components.Add(new Component(int.Parse(input[0]), int.Parse(input[1])));
            }
            file.Dispose();

            int bridgeStrength = CalculateBridgeStrength(0, 0, components);
            Console.WriteLine("Part 1: Strongest bridge has strength " + bridgeStrength);
            Console.ReadKey();

            Result longestBridge = CalculateLongestBridgeStrength(0, 0, 0, components);
            Console.WriteLine("Part 2: Longest bridge has strength " + longestBridge.strength);
            Console.ReadKey();
        }
    }
}
