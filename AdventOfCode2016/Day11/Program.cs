using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
--- Day 11: Radioisotope Thermoelectric Generators ---

You come upon a column of four floors that have been entirely sealed off from the rest of the building except for a 
small dedicated lobby. There are some radiation warnings and a big sign which reads "Radioisotope Testing Facility".

According to the project status board, this facility is currently being used to experiment with Radioisotope 
Thermoelectric Generators (RTGs, or simply "generators") that are designed to be paired with specially-constructed 
microchips. Basically, an RTG is a highly radioactive rock that generates electricity through heat.

The experimental RTGs have poor radiation containment, so they're dangerously radioactive. The chips are prototypes and
don't have normal radiation shielding, but they do have the ability to generate an elecromagnetic radiation shield when
powered. Unfortunately, they can only be powered by their corresponding RTG. An RTG powering a microchip is still 
dangerous to other microchips.

In other words, if a chip is ever left in the same area as another RTG, and it's not connected to its own RTG, the chip
will be fried. Therefore, it is assumed that you will follow procedure and keep chips connected to their corresponding 
RTG when they're in the same room, and away from other RTGs otherwise.

These microchips sound very interesting and useful to your current activities, and you'd like to try to retrieve them. 
The fourth floor of the facility has an assembling machine which can make a self-contained, shielded computer for you 
to take with you - that is, if you can bring it all of the RTGs and microchips.

Within the radiation-shielded part of the facility (in which it's safe to have these pre-assembly RTGs), there is an 
elevator that can move between the four floors. Its capacity rating means it can carry at most yourself and two RTGs or
microchips in any combination. (They're rigged to some heavy diagnostic equipment - the assembling machine will detach 
it for you.) As a security measure, the elevator will only function if it contains at least one RTG or microchip. The
elevator always stops on each floor to recharge, and this takes long enough that the items within it and the items on 
that floor can irradiate each other. (You can prevent this if a Microchip and its Generator end up on the same floor in
this way, as they can be connected while the elevator is recharging.)

You make some notes of the locations of each component of interest (your puzzle input). Before you don a hazmat suit
and start moving things around, you'd like to have an idea of what you need to do.

When you enter the containment area, you and the elevator will start on the first floor.

For example, suppose the isolated area has the following arrangement:

The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.
The second floor contains a hydrogen generator.
The third floor contains a lithium generator.
The fourth floor contains nothing relevant.
As a diagram (F# for a Floor number, E for Elevator, H for Hydrogen, L for Lithium, M for Microchip, and G for 
Generator), the initial state looks like this:

F4 .  .  .  .  .  
F3 .  .  .  LG .  
F2 .  HG .  .  .  
F1 E  .  HM .  LM 
Then, to get everything up to the assembling machine on the fourth floor, the following steps could be taken:

Bring the Hydrogen-compatible Microchip to the second floor, which is safe because it can get power from the Hydrogen
Generator:

F4 .  .  .  .  .  
F3 .  .  .  LG .  
F2 E  HG HM .  .  
F1 .  .  .  .  LM 
Bring both Hydrogen-related items to the third floor, which is safe because the Hydrogen-compatible microchip is 
getting power from its generator:

F4 .  .  .  .  .  
F3 E  HG HM LG .  
F2 .  .  .  .  .  
F1 .  .  .  .  LM 
Leave the Hydrogen Generator on floor three, but bring the Hydrogen-compatible Microchip back down with you so you can 
still use the elevator:

F4 .  .  .  .  .  
F3 .  HG .  LG .  
F2 E  .  HM .  .  
F1 .  .  .  .  LM 
At the first floor, grab the Lithium-compatible Microchip, which is safe because Microchips don't affect each other:

F4 .  .  .  .  .  
F3 .  HG .  LG .  
F2 .  .  .  .  .  
F1 E  .  HM .  LM 
Bring both Microchips up one floor, where there is nothing to fry them:

F4 .  .  .  .  .  
F3 .  HG .  LG .  
F2 E  .  HM .  LM 
F1 .  .  .  .  .  
Bring both Microchips up again to floor three, where they can be temporarily connected to their corresponding 
generators while the elevator recharges, preventing either of them from being fried:

F4 .  .  .  .  .  
F3 E  HG HM LG LM 
F2 .  .  .  .  .  
F1 .  .  .  .  .  
Bring both Microchips to the fourth floor:

F4 E  .  HM .  LM 
F3 .  HG .  LG .  
F2 .  .  .  .  .  
F1 .  .  .  .  .  
Leave the Lithium-compatible microchip on the fourth floor, but bring the Hydrogen-compatible one so you can still use
the elevator; this is safe because although the Lithium Generator is on the destination floor, you can connect 
Hydrogen-compatible microchip to the Hydrogen Generator there:

F4 .  .  .  .  LM 
F3 E  HG HM LG .  
F2 .  .  .  .  .  
F1 .  .  .  .  .  
Bring both Generators up to the fourth floor, which is safe because you can connect the Lithium-compatible Microchip to
the Lithium Generator upon arrival:

F4 E  HG .  LG LM 
F3 .  .  HM .  .  
F2 .  .  .  .  .  
F1 .  .  .  .  .  
Bring the Lithium Microchip with you to the third floor so you can use the elevator:

F4 .  HG .  LG .  
F3 E  .  HM .  LM 
F2 .  .  .  .  .  
F1 .  .  .  .  .  
Bring both Microchips to the fourth floor:

F4 E  HG HM LG LM 
F3 .  .  .  .  .  
F2 .  .  .  .  .  
F1 .  .  .  .  .  
In this arrangement, it takes 11 steps to collect all of the objects at the fourth floor for assembly. (Each elevator
stop counts as one step, even if nothing is added to or removed from it.)

In your situation, what is the minimum number of steps required to bring all of the objects to the fourth floor?

--- Part Two ---

You step into the cleanroom separating the lobby from the isolated area and put on the hazmat suit.

Upon entering the isolated containment area, however, you notice some extra parts on the first floor that weren't listed on the record outside:

An elerium generator.
An elerium-compatible microchip.
A dilithium generator.
A dilithium-compatible microchip.
These work just like the other generators and microchips. You'll have to get them up to assembly as well.

What is the minimum number of steps required to bring all of the objects, including these four new ones, to the fourth floor?

*/

namespace Day11
{
    class Program
    {
        private struct State
        {
            public State(int Elevator, int[] Items, int Steps)
            {
                elevator = Elevator;
                items = Items;
                steps = Steps;
            }

            public int elevator { get; }
            public int[] items { get; }
            public int steps { get; }
        }


        static void Main(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            List<string> input = new List<string>();
            try
            {
                string line = "";

                using (StreamReader sr = new StreamReader("input.txt"))
                {
                    while ( sr.Peek() >= 0)
                    {
                        line = sr.ReadLine();
                        input.Add(line);
                    }
                }
            }
            catch ( Exception e)
            {
                Console.Error.WriteLine("Could not read file: ");
                Console.Error.WriteLine(e.Message);
                Environment.Exit(0);
            }

            Dictionary<string, int> generators = new Dictionary<string, int>();
            Dictionary<string, int> chips = new Dictionary<string, int>();

            
            int floor = 1;

            
            foreach ( string line in input )
            {
                string[] current = line.Split(new[] { " a " }, StringSplitOptions.None);
                foreach (string s in current)
                {
                    if ( s.Contains("generator"))
                    {
                        string[] parts = s.Split(' ');
                        generators.Add(parts[0], floor);
                    }
                    else if ( s.Contains("microchip"))
                    {
                        string[] parts = s.Split('-');
                        chips.Add(parts[0], floor);
                    }
                }
                floor++;
            }

            // Part 2: Comment away for part 1.
            generators.Add("elerium", 1);
            generators.Add("dilithium", 1);
            chips.Add("elerium", 1);
            chips.Add("dilithium", 1);
           
            // Each pair of indexes represent a particular generator / microchip pair.
            int[] items = new int[generators.Count * 2];

            // Construct the initial state with given input.
            int index = 0;
            foreach( KeyValuePair<string, int> pair in generators)
            {
                items[index] = pair.Value;
                index++;
                items[index] = chips[pair.Key];
                index++;
            }

            State initialState = new State(1, items, 0);

            List<State> seenStates = new List<State>();
            seenStates.Add(initialState);

            Queue<State> moves = new Queue<State>();
            List<State> startingMoves = PossibleMoves(initialState);
            foreach ( State state in startingMoves)
            {
                moves.Enqueue(state);
            }

            bool endStateFound = false;
            int endStateStep = 0;

            while (!endStateFound && moves.Count > 0)
            {
                State nextState = moves.Dequeue();
                
                //System.Diagnostics.Debug.WriteLine(moves.Count());
                List<State> newMoves = PossibleMoves(nextState);

                // Check each new state to see if it's been seen before. If it's new, add it to list of seen states and
                // check if it's the final state.
                for (int i = 0; i < newMoves.Count(); i++)
                {
                    if (!AlreadySeen(seenStates, newMoves[i]))
                    {
                        if (IsTargetState(newMoves[i]))
                        {
                            endStateFound = true;
                            endStateStep = newMoves[i].steps;
                        }
                        else
                        {
                            moves.Enqueue(newMoves[i]);
                            seenStates.Add(newMoves[i]);
                        }
                    }
                }
            }

            if (!endStateFound)
                Console.WriteLine("No answer found!");
            else
            {
                Console.WriteLine("Target state found after " + endStateStep + " steps!");
                System.Diagnostics.Debug.WriteLine("Target state found after " + endStateStep + " steps!");
            }

            string elapsed = watch.Elapsed.ToString();
            System.Diagnostics.Debug.WriteLine(elapsed);
            Console.WriteLine(elapsed);
        }
        
        // Build up a list of all possible moves to be made from given start-state.
        private static List<State> PossibleMoves( State currentState )
        {
            List<State> newMoves = new List<State>();
            
            List<int> itemsOnFloor = Enumerable.Range(0, currentState.items.Count())
                    .Where(i => currentState.items[i] == currentState.elevator)
                    .ToList();

            if ( itemsOnFloor.Count == 0 )
            {
                Console.WriteLine("Elevator is on floor " + currentState.elevator + " and items are: ");
                foreach (int i in currentState.items)
                    Console.Write(i + " ");
                Console.WriteLine();
                Environment.Exit(0);
            }

            // As long as current floor is NOT 4th floor, make state for every legal combination of items moving up.
            if ( currentState.elevator < 4 )
            {
                // Single item moves.
                foreach ( int index in itemsOnFloor )
                {
                    int[] newItems = (int[])currentState.items.Clone();
                    newItems[index]++;
                    State newState = new State(currentState.elevator + 1, newItems, currentState.steps + 1);
                    if (IsLegalState(newState))
                        newMoves.Add(newState);
                }

                // Two item moves.
                for ( int i = 0; i < itemsOnFloor.Count-1; i++)
                {
                    for ( int j = i+1; j < itemsOnFloor.Count; j++)
                    {
                        int[] newItems = (int[])currentState.items.Clone();
                        newItems[itemsOnFloor[i]]++;
                        newItems[itemsOnFloor[j]]++;
                        State newState = new State(currentState.elevator + 1, newItems, currentState.steps + 1);
                        if (IsLegalState(newState))
                            newMoves.Add(newState);
                    }
                }
            }
            // As long as current floor is NOT 1st floor, make state for every legal combination of items moving down.
            if ( currentState.elevator > 1 )
            {
                // Single item moves.
                foreach (int index in itemsOnFloor)
                {
                    int[] newItems = (int[])currentState.items.Clone();
                    newItems[index]--;
                    State newState = new State(currentState.elevator - 1, newItems, currentState.steps + 1);
                    if (IsLegalState(newState))
                        newMoves.Add(newState);
                }
                // Two item moves.
                for (int i = 0; i < itemsOnFloor.Count - 1; i++)
                {
                    for (int j = i + 1; j < itemsOnFloor.Count; j++)
                    {
                        int[] newItems = (int[])currentState.items.Clone();
                        newItems[itemsOnFloor[i]]--;
                        newItems[itemsOnFloor[j]]--;
                        State newState = new State(currentState.elevator - 1, newItems, currentState.steps + 1);
                        if (IsLegalState(newState))
                            newMoves.Add(newState);
                    }
                }
            }
            
            return newMoves;
        }

        // Turns the current items of a State into an array that describes what kind of pairs it contains.
        // Since microchip-generator pairs are interchangeable, the exact pairs don't matter, just their numbers.
        private static int[] CombinationsList( State state )
        {
            int[] combinations = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] items = state.items;
            for ( int i = 0; i < items.Count(); i += 2)
            {
                if (items[i] == 1 && items[i + 1] == 1)
                    combinations[0]++;
                else if (items[i] == 1 && items[i + 1] == 2)
                    combinations[1]++;
                else if (items[i] == 1 && items[i + 1] == 3)
                    combinations[2]++;
                else if (items[i] == 1 && items[i + 1] == 4)
                    combinations[3]++;
                else if (items[i] == 2 && items[i + 1] == 1)
                    combinations[4]++;
                else if (items[i] == 2 && items[i + 1] == 2)
                    combinations[5]++;
                else if (items[i] == 2 && items[i + 1] == 3)
                    combinations[6]++;
                else if (items[i] == 2 && items[i + 1] == 4)
                    combinations[7]++;
                else if (items[i] == 3 && items[i + 1] == 1)
                    combinations[8]++;
                else if (items[i] == 3 && items[i + 1] == 2)
                    combinations[9]++;
                else if (items[i] == 3 && items[i + 1] == 3)
                    combinations[10]++;
                else if (items[i] == 3 && items[i + 1] == 4)
                    combinations[11]++;
                else if (items[i] == 4 && items[i + 1] == 1)
                    combinations[12]++;
                else if (items[i] == 4 && items[i + 1] == 2)
                    combinations[13]++;
                else if (items[i] == 4 && items[i + 1] == 3)
                    combinations[14]++;
                else if (items[i] == 4 && items[i + 1] == 4)
                    combinations[15]++;
            }
            return combinations;
        }

        // Check if a given state has already been seen.
        private static bool AlreadySeen( List<State> seenStates, State newState )
        {
            for (int i = 0; i < seenStates.Count(); i++)
            {
                if (newState.elevator < 1 || newState.elevator > 4)
                {
                    Console.WriteLine("Elevator off the rails!");
                    Environment.Exit(0);
                }
                if (seenStates[i].elevator == newState.elevator && seenStates[i].steps <= newState.steps &&
                    seenStates[i].items.SequenceEqual(newState.items))
                    return true;
                else if (seenStates[i].elevator == newState.elevator && CombinationsList(seenStates[i]).SequenceEqual(CombinationsList(newState)))
                {
                    if (seenStates[i].steps > newState.steps)
                    {
                        seenStates.Remove(seenStates[i]);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Check if given state is the target state.
        private static bool IsTargetState( State newState )
        {
            if (newState.elevator == 4)
            {
                for (int i = 0; i < newState.items.Count(); i++)
                    if (newState.items[i] != 4)
                        return false;
                return true;
            }
            return false;
        }

        // Check if a given state is legal.
        private static bool IsLegalState(State newState)
        {
            int[] items = newState.items;
            for ( int i = 1; i < items.Count(); i += 2)
            {
                // If chip is on the same floor as its generator, it is OK: no further checks required.
                if ( items[i] != items[i-1] )
                {
                    // Otherwise, chip is in illegal position if it shares floor with a generator of another type.
                    // If this is the case, return false.
                    for (int j = 0; j < items.Count(); j += 2)
                    {
                        if (items[j] == items[i])
                            return false;
                    }  
                }
            }

            return true;
        }
    }
}
