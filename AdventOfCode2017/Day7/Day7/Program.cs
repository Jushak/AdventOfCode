using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/*

--- Day 7: Recursive Circus ---

Wandering further through the circuits of the computer, you come upon a tower of programs that have gotten themselves
into a bit of trouble. A recursive algorithm has gotten out of hand, and now they're balanced precariously in 
a large tower.

One program at the bottom supports the entire tower. It's holding a large disc, and on the disc are balanced several 
more sub-towers. At the bottom of these sub-towers, standing on the bottom disc, are other programs, each holding 
their own disc, and so on. At the very tops of these sub-sub-sub-...-towers, many programs stand simply keeping 
the disc below them balanced but with no disc of their own.

You offer to help, but first you need to understand the structure of these towers. You ask each program to yell out
their name, their weight, and (if they're holding a disc) the names of the programs immediately above them balancing on
that disc. You write this information down (your puzzle input). Unfortunately, in their panic, they don't do this in an
orderly fashion; by the time you're done, you're not sure which program gave which information.

For example, if your list is the following:

    pbga (66)
    xhth (57)
    ebii (61)
    havc (66)
    ktlj (57)
    fwft (72) -> ktlj, cntj, xhth
    qoyq (66)
    padx (45) -> pbga, havc, qoyq
    tknk (41) -> ugml, padx, fwft
    jptl (61)
    ugml (68) -> gyxo, ebii, jptl
    gyxo (61)
    cntj (57)

    ...then you would be able to recreate the structure of the towers that looks like this:

                gyxo
              /     
         ugml - ebii
       /      \     
      |         jptl
      |        
      |         pbga
     /        /
tknk --- padx - havc
     \        \
      |         qoyq
      |             
      |         ktlj
       \      /     
         fwft - cntj
              \     
                xhth

In this example, tknk is at the bottom of the tower (the bottom program), and is holding up ugml, padx, and fwft. 
Those programs are, in turn, holding up other programs; in this example, none of those programs are holding up any 
other programs, and are all the tops of their own towers. (The actual tower balancing in front of you is much larger.)

Before you're ready to help them, you need to make sure your information is correct. What is the name of the bottom
program?

Your puzzle answer was vmpywg.

The first half of this puzzle is complete! It provides one gold star: *

--- Part Two ---

The programs explain the situation: they can't get down. Rather, they could get down, if they weren't expending all
of their energy trying to keep the tower balanced. Apparently, one program has the wrong weight, and until it's fixed,
they're stuck here.

For any program holding a disc, each program standing on that disc forms a sub-tower. Each of those sub-towers are
supposed to be the same weight, or the disc itself isn't balanced. The weight of a tower is the sum of the weights
of the programs in that tower.

In the example above, this means that for ugml's disc to be balanced, gyxo, ebii, and jptl must all have the same
weight, and they do: 61.

However, for tknk to be balanced, each of the programs standing on its disc and all programs above it must each match.
This means that the following sums must all be the same:

    ugml + (gyxo + ebii + jptl) = 68 + (61 + 61 + 61) = 251
    padx + (pbga + havc + qoyq) = 45 + (66 + 66 + 66) = 243
    fwft + (ktlj + cntj + xhth) = 72 + (57 + 57 + 57) = 243

As you can see, tknk's disc is unbalanced: ugml's stack is heavier than the other two. Even though the nodes above ugml
are balanced, ugml itself is too heavy: it needs to be 8 units lighter for its stack to weigh 243 and keep the towers
balanced. If this change were made, its weight would be 60.

Given that exactly one program is the wrong weight, what would its weight need to be to balance the entire tower?

    Your puzzle answer was 1674.

*/

namespace Day7
{
    class Unit
    {
        public string name;
        public int weight;
        public int towerWeight;
        public List<Unit> above;
        public Unit(string name, int weight)
        {
            this.name = name;
            this.weight = weight;
            this.towerWeight = 0;
            this.above = new List<Unit>();
        }
    }

    class Program
    {
        // Finds the correct weight for the imbalanced program.
        static int FindCorrectWeight(Unit root, int weight_imbalance)
        {
            int nextRoot = 0;
            // First find min and max tower-weights from above programs.
            FindMinMax(root.above, out int min, out int max);

            // If min == max we have found the imbalanced program. Return adjusted value.
            if (min == max)
            {
                return root.weight + weight_imbalance;
            }
            // Otherwise, find the root of the next program.
            else
            {
                if (weight_imbalance > 0)
                {
                    nextRoot = root.above.FindIndex(p => p.towerWeight == min);
                }
                else
                {
                    nextRoot = root.above.FindIndex(p => p.towerWeight == max);
                }
            }

            return FindCorrectWeight(root.above[nextRoot], weight_imbalance);
        }

        static void FindMinMax(List<Unit> units, out int min, out int max)
        {
            min = -1;
            max = -1;
            if (units.Count == 0)
            {
                min = max = 0;
            }
            if (units.Count == 2)
            {
                if (units[0].towerWeight == units[0].towerWeight)
                {
                    min = max = units[0].towerWeight;
                }
                else if (units[0].towerWeight < units[0].towerWeight)
                {
                    min = units[0].towerWeight;
                    max = units[0].towerWeight;
                }
            }
            else
            {
                for (int i = 0; i < units.Count - 1; i++)
                {
                    if (min < 0 || max < 0)
                    {
                        if (units[i].towerWeight < units[i + 1].towerWeight)
                        {
                            min = units[i].towerWeight;
                            max = units[i + 1].towerWeight;
                        }
                        else if (units[i].towerWeight > units[i + 1].towerWeight)
                        {
                            min = units[i + 1].towerWeight;
                            max = units[i].towerWeight;
                        }
                    }
                }
                if (min == max && min < 0)
                {
                    min = max = units[0].towerWeight;
                }
            }
        }

        // Recursively calculates the weight of a tower.
        static int CalculateWeight(Unit root)
        {
            int weight = root.weight;
            foreach (Unit held in root.above)
            {
                weight += CalculateWeight(held);
            }
            return weight;
        }

        static void Main(string[] args)
        {
            List<Unit> units = new List<Unit>();
            List<string> programs = new List<string>();
            List<string> heldPrograms = new List<string>();
            string[] input;
            string line;
            string[] delimiter = { " ", "\t", ",", "->", "(", ")" };
            string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);

            // Part 1: Create initial list of programs, note down all programs and all held programs.
            StreamReader file = new StreamReader(filePath + "/input.txt");
            while ((line = file.ReadLine()) != null)
            {
                input = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                units.Add(new Unit(input[0], int.Parse(input[1])));
                programs.Add(input[0]);
                for (int i = 2; i < input.Length; i++)
                {
                    heldPrograms.Add(input[i]);
                }
            }

            file.Dispose();

            // Programs - heldPrograms => Only one program that is not being held.
            heldPrograms = new HashSet<string>(heldPrograms).ToList();
            programs = programs.Except(heldPrograms).ToList();
            Console.WriteLine("Part 1: Unheld programs count: " + programs.Count);
            Console.Write("Programs: ");
            foreach (string s in programs)
            {
                Console.Write(s + " ");
            }
            Console.WriteLine();

            // Part 2: Add held programs to their respective roots.
            file = new StreamReader(filePath + "/input.txt");

            int index = 0;
            while ((line = file.ReadLine()) != null)
            {

                input = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 2; i < input.Length; i++)
                {
                    int unitIndex = units.FindIndex(p => string.Compare(p.name, input[i]) == 0);
                    units[index].above.Add(units[unitIndex]);
                }
                index++;
            }

            // Calculate tower weight for each program.
            foreach (Unit u in units)
            {
                u.towerWeight = CalculateWeight(u);
            }

            // Find the root program from list of programs.
            int rootProgram = units.FindIndex(p => string.Compare(p.name, programs[0]) == 0);

            // First, find the initial imbalance.
            int imbalance = 0;
            List<int> weights = new List<int>();
            int common_weight = -1;
            int unique_weight = -1;
            for (int i = 1; i < units[rootProgram].above.Count - 1; i++)
            {
                if (common_weight < 0)
                {
                    if (units[rootProgram].above[i].towerWeight == units[rootProgram].above[i + 1].towerWeight ||
                        units[rootProgram].above[i].towerWeight == units[rootProgram].above[i + -1].towerWeight)
                    {
                        common_weight = units[rootProgram].above[i].towerWeight;
                    }
                }
            }
            for (int i = 0; i < units[rootProgram].above.Count; i++)
            {
                if (unique_weight < 0)
                {
                    if (units[rootProgram].above[i].towerWeight != common_weight)
                    {
                        unique_weight = units[rootProgram].above[i].towerWeight;
                    }
                }
            }
            Console.WriteLine(common_weight + " " + unique_weight);
            imbalance = common_weight - unique_weight;

            // Starting from the root, begin recursive search for the imbalanced program.
            int weight = FindCorrectWeight(units[rootProgram], imbalance);

            Console.WriteLine("Part 2: the balanced weight would be " + weight);

            Console.ReadKey();
        }
    }
}
