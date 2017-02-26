using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
--- Day 13: A Maze of Twisty Little Cubicles ---

You arrive at the first floor of this new building to discover a much less welcoming environment than the shiny atrium
of the last one. Instead, you are in a maze of twisty little cubicles, all alike.

Every location in this area is addressed by a pair of non-negative integers (x,y). Each such coordinate is either a
wall or an open space. You can't move diagonally. The cube maze starts at 0,0 and seems to extend infinitely toward
positive x and y; negative values are invalid, as they represent a location outside the building. You are in a small
waiting area at 1,1.

While it seems chaotic, a nearby morale-boosting poster explains, the layout is actually quite logical. You can
determine whether a given x,y coordinate will be a wall or an open space using a simple system:

- Find x*x + 3*x + 2*x*y + y + y*y.
- Add the office designer's favorite number (your puzzle input).
- Find the binary representation of that sum; count the number of bits that are 1.
    - If the number of bits that are 1 is even, it's an open space.
    - If the number of bits that are 1 is odd, it's a wall.

For example, if the office designer's favorite number were 10, drawing walls as # and open spaces as ., the corner of
the building containing 0,0 would look like this:

  0123456789
0 .#.####.##
1 ..#..#...#
2 #....##...
3 ###.#.###.
4 .##..#..#.
5 ..##....#.
6 #...##.###

Now, suppose you wanted to reach 7,4. The shortest route you could take is marked as O:

  0123456789
0 .#.####.##
1 .O#..#...#
2 #OOO.##...
3 ###O#.###.
4 .##OO#OO#.
5 ..##OOO.#.
6 #...##.###

Thus, reaching 7,4 would take a minimum of 11 steps (starting from your current location, 1,1).

What is the fewest number of steps required for you to reach 31,39?

--- Part Two ---

How many locations (distinct x,y coordinates, including your starting location) can you reach in at most 50 steps?

Your puzzle input is 1364.
*/

namespace Day13
{
    class Program
    {
        // Struct representing a location of a node in a 2-D map.
        public struct Point
        {
            public Point (int a, int b)
            {
                x = a;
                y = b;
            }

            public int x { get; set; }
            public int y { get; set; }
        }

        // Enum representing possible states of a node.
        public enum NodeState { Untested, Open, Closed, Path }

        // Class representing a single node on the map.
        public class Node
        {
            public Node(Point point, bool walkable, float g, float h, NodeState nodeState, Node parent)
            {
                Location = point;
                IsWalkable = walkable;
                G = g;
                H = h;
                State = nodeState;
                ParentNode = parent;
                Steps = 0;
            }

            public Point Location { get; private set; }
            public bool IsWalkable { get; set; }
            public float G { get; private set; }
            public float H { get; private set; }
            public float F { get { return this.G + this.H; } }
            public NodeState State { get; set; }
            public Node ParentNode { get; set; }
            public int Steps { get; set; }
        }

        // Class that takes parameters as input and does the required search based on them as requested.
        public class SearchParameters
        {
            public SearchParameters( Node start, Node end, Node[,] nodes, int max)
            {
                StartLocation = start;
                EndLocation = end;
                Map = nodes;
                MaxSteps = max;
            }

            public Node StartLocation { get; set; }
            public Node EndLocation { get; set; }
            public Node [,] Map { get; set; }
            public int MaxSteps { get; set; }

            // Recursively searches for fastest route from startLocation to endLocation.
            private bool Search(Node currentNode)
            {
                currentNode.State = NodeState.Closed;
                List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);
                nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
                foreach (var nextNode in nextNodes)
                {
                    if (nextNode == this.EndLocation)
                    {
                        return true;
                    }
                    else
                    {
                        if (Search(nextNode)) // Note: Recurses back into Search(Node)
                            return true;
                    }
                }
                return false;
            }

            // Helper function for BreadthFirst. Closes the node being checked and returns unclosed walkable nodes
            // around it.
            private List<Node> BreadthSearch(Node currentNode)
            {
                currentNode.State = NodeState.Closed;
                List<Node> nextNodes = GetBreadthAdjacentWalkableNodes(currentNode);
                return nextNodes;
            }

            // Helper function for BreadthFirst. Returns all unclosed, walkable nodes around given node.
            public List<Node> GetBreadthAdjacentWalkableNodes(Node fromNode)
            {
                List<Node> walkableNodes = new List<Node>();
                IEnumerable<Point> nextLocations = GetAdjacentLocations(fromNode.Location);

                foreach (var location in nextLocations)
                {
                    int x = location.x;
                    int y = location.y;

                    if (x < 0 || x >= Map.GetLength(1) || y < 0 || y >= Map.GetLength(1))
                        continue;

                    Node node = this.Map[x, y];
                    node.ParentNode = fromNode;
                    node.Steps = node.ParentNode.Steps + 1;

                    // Ignore 51st step and above.
                    if (node.Steps > 50)
                        continue;

                    // Ignore non-walkable
                    if (!node.IsWalkable)
                        continue;

                    // Ignore already closed
                    if (node.State == NodeState.Closed)
                        continue;
                    
                    walkableNodes.Add(node);
                }

                return walkableNodes;
            }

            // Helper function for 
            public List<Point> GetAdjacentLocations(Point location)
            {
                List<Point> points = new List<Point>();
                points.Add(new Point(location.x + 1, location.y));
                points.Add(new Point(location.x, location.y+1));
                points.Add(new Point(location.x - 1, location.y));
                points.Add(new Point(location.x, location.y -1));

                return points;
            }

            // Helper function for FindPath. Returns target point's traversal cost.
            public float GetTraversalCost( Point current, Point target )
            {
                return Math.Abs(target.x - current.x + target.y - target.x);
            }

            // Helper function for FindPath. Returns list of eligible nodes around given node.
            public List<Node> GetAdjacentWalkableNodes(Node fromNode)
            {
                List<Node> walkableNodes = new List<Node>();
                IEnumerable<Point> nextLocations = GetAdjacentLocations(fromNode.Location);

                foreach (var location in nextLocations)
                {
                    int x = location.x;
                    int y = location.y;

                    if (x < 0 || x >= Map.GetLength(1) || y < 0 || y >= Map.GetLength(1))
                        continue;

                    Node node = this.Map[x, y];
                    // Ignore non-walkable
                    if (!node.IsWalkable)
                        continue;

                    // Ignore already closed
                    if (node.State == NodeState.Closed)
                        continue;

                    // Already open only added to the list if their G-value is lower via this route
                    if ( node.State == NodeState.Open)
                    {
                        float traversalCost = GetTraversalCost(node.Location, node.ParentNode.Location);
                        float gTemp = fromNode.G + traversalCost;
                        if ( gTemp < node.G)
                        {
                            node.ParentNode = fromNode;
                            walkableNodes.Add(node);
                        }
                    }
                    else
                    {
                        // If untested, set the parent and flag as 'Open' for consideration.
                        node.ParentNode = fromNode;
                        node.State = NodeState.Open;
                        walkableNodes.Add(node);
                    }
                }

                return walkableNodes;
            }

            // Search for a path to target location, starting from StartLocation. If a path is successfully found,
            // returns the path as a list of points.
            public List<Point> FindPath()
            {
                List<Point> path = new List<Point>();
                bool success = Search(StartLocation);
                if (success)
                {
                    Node node = this.EndLocation;
                    while (node.ParentNode != null)
                    {
                        path.Add(node.Location);
                        node = node.ParentNode;
                    }
                    path.Reverse();
                }
                return path;
            }

            // Resets the state of the map, without wiping the initial input.
            public void Reset()
            {
                for ( int x = 0; x < Map.GetLength(0); x++ )
                {
                    for ( int y = 0; y < Map.GetLength(1); y++)
                    {
                        Map[x, y].ParentNode = null;
                        Map[x, y].State = NodeState.Untested;
                    }
                }
            }

            // Does a breadth-first search to see how many nodes can be reached in 50 steps.
            public int BreadthFirst()
            {
                List<Point> path = new List<Point>();
                
                Queue<Node> searchQueue = new Queue<Node>();
                searchQueue.Enqueue(StartLocation);

                Node node;

                while ( searchQueue.Count > 0)
                {
                    node = searchQueue.Dequeue();
                    BreadthSearch(node).ForEach(o => searchQueue.Enqueue(o));
                }

                int searchedNodes = 0;
                for (int x = 0; x < Map.GetLength(0); x++)
                {
                    for (int y = 0; y < Map.GetLength(1); y++)
                    {
                        if (Map[x, y].State == NodeState.Closed)
                            searchedNodes++;
                    }
                }

                return searchedNodes;
            }
        }

        static void Main(string[] args)
        {
            int input = 1364;
            int width = 50;
            int height = 50;

            Node[,] floorPlan = new Node[width, height];

            // Create the map as instructed.
            for ( int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int sum = x * x + 3 * x + 2 * x * y + y + y * y + input;
                    int count = 0;
                    while (sum != 0)
                    {
                        sum = sum & (sum - 1);
                        count++;
                    }
                    if ( count % 2 == 0 )
                    {
                        floorPlan[x, y] = new Node(new Point(x, y), true, -1, -1, NodeState.Untested, null);
                    }
                    else
                    {
                        floorPlan[x, y] = new Node(new Point(x, y), false, -1, -1, NodeState.Untested, null);
                    }
                } 
            }

            // Search fastest way to target location (31,  39) from start location (1, 1).
            SearchParameters search = new SearchParameters(floorPlan[1, 1], floorPlan[31, 39], floorPlan, 50);
            List<Point> path = search.FindPath();
            
            Console.WriteLine("The length of the fastest route is " + path.Count);

            // Draw map of the fastest path.

            // Adds "Path" state to the points in the map where the path goes through.
            foreach ( Point point in path )
            {
                floorPlan[point.x, point.y].State = NodeState.Path;
            }

            // Add 0-9 numbering on top of the map.
            int numbers = 0;
            Console.Write(' ');
            for ( int i = 0; i < width; i++ )
            {
                Console.Write(numbers);
                numbers++;
                if (numbers > 9)
                    numbers = 0;
            }
            Console.WriteLine();
            numbers = 0;

            // Draw map, line by line.
            for ( int y = 0; y < height; y++)
            {
                // Start each line with 0-9 to signify which row is in question. 
                Console.Write(numbers);
                floorPlan[1, 1].State = NodeState.Path;
                for ( int x = 0; x < width; x++)
                {
                    if (floorPlan[x, y].State == NodeState.Path)
                        Console.Write('X');
                    else if (floorPlan[x, y].IsWalkable)
                        Console.Write('.');
                    else
                        Console.Write('#');
                }
                Console.WriteLine();
                numbers++;
                if (numbers > 9)
                    numbers = 0;
            }

            // Add numbers on bottom of the map.
            numbers = 0;
            Console.Write(' ');
            for (int i = 0; i < width; i++)
            {
                Console.Write(numbers);
                numbers++;
                if (numbers > 9)
                    numbers = 0;
            }
            Console.WriteLine();

            // Part 2: Do the breadth-first search.

            search.Reset();
            Console.WriteLine("The amount of distinct nodes reached: " + search.BreadthFirst());
        }
    }
}
