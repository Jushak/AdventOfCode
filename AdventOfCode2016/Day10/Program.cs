using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
--- Day 10: Balance Bots ---

You come upon a factory in which many robots are zooming around handing small microchips to each other.

Upon closer examination, you notice that each bot only proceeds when it has two microchips, and once it does, it gives
each one to a different bot or puts it in a marked "output" bin. Sometimes, bots take microchips from "input" bins,
too.

Inspecting one of the microchips, it seems like they each contain a single number; the bots must use some logic to 
decide what to do with each chip. You access the local control computer and download the bots' instructions (your 
puzzle input).

Some of the instructions specify that a specific-valued microchip should be given to a specific bot; the rest of the
instructions indicate what a given bot should do with its lower-value or higher-value chip.

For example, consider the following instructions:

value 5 goes to bot 2
bot 2 gives low to bot 1 and high to bot 0
value 3 goes to bot 1
bot 1 gives low to output 1 and high to bot 0
bot 0 gives low to output 2 and high to output 0
value 2 goes to bot 2

- Initially, bot 1 starts with a value-3 chip, and bot 2 starts with a value-2 chip and a value-5 chip.
- Because bot 2 has two microchips, it gives its lower one (2) to bot 1 and its higher one (5) to bot 0.
- Then, bot 1 has two microchips; it puts the value-2 chip in output 1 and gives the value-3 chip to bot 0.
- Finally, bot 0 has two microchips; it puts the 3 in output 2 and the 5 in output 0.
In the end, output bin 0 contains a value-5 microchip, output bin 1 contains a value-2 microchip, and output bin 2
contains a value-3 microchip. In this configuration, bot number 2 is responsible for comparing value-5 microchips
with value-2 microchips.

Based on your instructions, what is the number of the bot that is responsible for comparing value-61 microchips 
with value-17 microchips?

--- Part Two ---

What do you get if you multiply together the values of one chip in each of outputs 0, 1, and 2?

*/

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string[]> input = new List<string[]>();
            List<string[]> bots = new List<string[]>();
            string[] current;

            string line = "";
            try
            {
                using (StreamReader sr = new StreamReader("input.txt"))
                {
                    while (sr.Peek() >= 0)
                    {
                        line = sr.ReadLine();
                        current = line.Split(' ');
                        if (current[0] == "bot")
                            bots.Add(current);
                        else
                            input.Add(current);
                    }
                    Console.WriteLine("Read " + (input.Count + bots.Count) + " lines.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not read file: ");
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }

            Dictionary<int, Bot> botList = new Dictionary<int, Bot>();
            Dictionary<int, int> outputBins = new Dictionary<int, int>();

            // Bot command syntax:
            // 0: bot
            // 1: #
            // 2: gives
            // 3: low
            // 4: to
            // 5: bot / output
            // 6: target #
            // 7: and
            // 8: high
            // 9: to
            // 10: bot / output
            // 11: target #
            for ( int i = 0; i < bots.Count; i++ )
            {
                current = bots[i];
                int id = int.Parse(current[1]);
                bool outputLow = (current[5] == "output");
                int targetLow = int.Parse(current[6]);
                bool outputHigh = (current[10] == "output");
                int targetHigh = int.Parse(current[11]);
                botList.Add(id, new Bot(id, targetLow, targetHigh, outputLow, outputHigh));
            }

            // Value command syntax:
            // 0: value
            // 1: #
            // 2: goes
            // 3: to
            // 4: bot
            // 5: target #
            List<int> botsWithUnpassedValue = new List<int>();

            for ( int i = 0; i < input.Count ; i++)
            {
                current = input[i];
                if (botList[int.Parse(current[5])].GiveValue(int.Parse(current[1])))
                    botsWithUnpassedValue.Add(int.Parse(current[5]));

                while (botsWithUnpassedValue.Count > 0)
                {
                    Bot bot = botList[botsWithUnpassedValue[0]];

                    // Check whether or not the target bots can accept values.
                    if ((!bot.HighOutput() && botList[bot.HighTarget()].botIsFull()) ||
                        (!bot.LowOutput() && botList[bot.LowTarget()].botIsFull()))
                    {
                        // If not, add this bot to the end of the list.
                        botsWithUnpassedValue.Add(bot.GetId());
                    }
                    // If targets can accept values, proceed to pass values.
                    else
                    {
                        if (!bot.HighOutput() && botList[bot.HighTarget()].GiveValue(bot.HighValue()))
                            botsWithUnpassedValue.Add(bot.HighTarget());
                        else
                        {
                            if (!outputBins.ContainsKey(bot.HighTarget()))
                            {
                                outputBins.Add(bot.HighTarget(), bot.HighValue());
                            }
                            else
                            {
                                Console.WriteLine("Output bin " + bot.HighTarget() + " value " + outputBins[bot.HighTarget()] + " replaced by high value " + bot.HighValue());
                                outputBins[bot.HighTarget()] = bot.HighValue();
                            }
                        }
                        if (!bot.LowOutput() && botList[bot.LowTarget()].GiveValue(bot.LowValue()))
                            botsWithUnpassedValue.Add(bot.LowTarget());
                        else
                        {
                            if (!outputBins.ContainsKey(bot.LowTarget()))
                            {
                                outputBins.Add(bot.LowTarget(), bot.LowValue());
                            }
                            else
                            {
                                Console.WriteLine("Output bin " + bot.LowTarget() + " value " + outputBins[bot.LowTarget()] + " replaced by low value " + bot.LowValue());
                                outputBins[bot.LowTarget()] = bot.LowValue();
                            }
                        }
                        bot.Clear();
                    }

                    // Remove this bot from the beginning of the list.
                    botsWithUnpassedValue.RemoveAt(0);
                }
            }

            Console.WriteLine("Values in output bins 0, 1 and 2 are: " + outputBins[0] + " " + outputBins[1] + " " + outputBins[2]);
            Console.WriteLine("Total multiplication value of output bins 0, 1 and 2 is: " + outputBins[0] * outputBins[1] * outputBins[2]);

        }
    }

    // Class reprenting a single Bot described in the instructions.
    class Bot
    {
        int id;
        int high;
        int low;
        bool lowOutput;
        bool highOutput;

        List<int> values;
        public Bot(int Id, int Low, int High, bool LowOutput, bool HighOutput)
        {
            id = Id;
            high = High;
            low = Low;
            lowOutput = LowOutput;
            highOutput = HighOutput;
            values = new List<int>();
        }

        // Takes given value. If bot now has two values, return true, otherwise false.
        public bool GiveValue(int value)
        {
            values.Add(value);
            if (values.Count >= 2)
            {
                if (values.Max() == 61 && values.Min() == 17)
                    Console.WriteLine("Wanted value comparison done by bot " + id);

                return true;
            }
            return false;
        }

        public int GetId()
        {
            return id;
        }
        public int HighTarget()
        {
            return high;
        }

        public int LowTarget()
        {
            return low;
        }

        public int HighValue()
        {
            return values.Max();
        }

        public int LowValue()
        {
            return values.Min();
        }

        public bool LowOutput()
        {
            return lowOutput;
        }

        public bool HighOutput()
        {
            return highOutput;
        }

        public void Clear()
        {
            values.Clear();
        }

        public bool HasValues()
        {
            return (values.Count > 0);
        }

        public bool botIsFull()
        {
            return (values.Count >= 2);
        }
    }
}
