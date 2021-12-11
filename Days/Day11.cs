namespace AoC_2021
{
    public class Day11 : Day
    {
        int[,] octopusMapReference = new int[10, 10];
        public override string Part1()
        {
            input = GetInput().Result;
            var lines = input.Replace("\r", "").Split("\n");
            octopusMapReference = new int[lines.Length, lines[0].Length];
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for (int j = 0; j < lines.Length; j++)
                {
                    octopusMapReference[i, j] = int.Parse(line[j].ToString());
                }
            }

            return Simulate(100).ToString();
        }

        void PrintMap(int[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.ForegroundColor = (map[i, j] == 0) ? ConsoleColor.White : ConsoleColor.Gray;
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }

        long Simulate(int steps, bool stopWhenSynchronized=false, bool print = false)
        {
            int[,] octopusMap = new int[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    octopusMap[i, j] = octopusMapReference[i, j];
                }
            }
            long totalFlashes = 0;
            if (print)
            {
                Console.WriteLine("Before any steps:");
                PrintMap(octopusMap);
            }
            for (int s = 0; s < steps; s++)
            {
                var flashesInThisStep=0;
                var flashed = new List<(int x, int y)>();
                var toCheck = new List<(int x, int y)>();
                for (int y = 0; y < octopusMap.GetLength(0); y++)
                {
                    for (int x = 0; x < octopusMap.GetLength(1); x++)
                    {
                        octopusMap[y, x]++;
                        toCheck.Add((x, y));
                    }
                }
                while (toCheck.Count > 0)
                {
                    var octopus = toCheck[0];
                    toCheck.RemoveAt(0);
                    if (octopusMap[octopus.y, octopus.x] > 9)
                    {
                        totalFlashes++;
                        flashesInThisStep++;
                        octopusMap[octopus.y, octopus.x] = 0;
                        flashed.Add(octopus);
                        var neighbours = GetNeighbours(octopusMap, octopus.x, octopus.y);
                        foreach (var neighbour in neighbours)
                        {
                            if (flashed.Contains(neighbour))
                            {
                                continue;
                            }
                            octopusMap[neighbour.y, neighbour.x]++;
                            toCheck.Add(neighbour);
                        }
                    }
                }
                if (print)
                {
                    Console.WriteLine("After step {0}", s + 1);
                    PrintMap(octopusMap);
                    Console.WriteLine();
                }
                if (stopWhenSynchronized && flashesInThisStep == octopusMap.GetLength(0) * octopusMap.GetLength(1))
                {
                    if (print)
                    {
                        Console.WriteLine("Synchronized after {0} steps", s + 1);
                        PrintMap(octopusMap);
                        Console.WriteLine();
                    }
                    return s+1;
                }
            }
            if (stopWhenSynchronized)
            {
                return -1;
            }
            return totalFlashes;
        }

        List<(int x, int y)> GetNeighbours(int[,] map, int x, int y)
        {
            var neighbours = new List<(int x, int y)>();
            if (x > 0)
            {
                neighbours.Add((x - 1, y)); //left
            }
            if (x < map.GetLength(0) - 1)
            {
                neighbours.Add((x + 1, y)); //right
            }
            if (y > 0)
            {
                neighbours.Add((x, y - 1)); //down
                if (x > 0)
                {
                    neighbours.Add((x - 1, y - 1)); //left down
                }
                if (x < map.GetLength(0) - 1)
                {
                    neighbours.Add((x + 1, y - 1)); //right down
                }
            }
            if (y < map.GetLength(1) - 1)
            {
                neighbours.Add((x, y + 1)); //up
                if (x > 0)
                {
                    neighbours.Add((x - 1, y + 1)); //left up
                }
                if (x < map.GetLength(0) - 1)
                {
                    neighbours.Add((x + 1, y + 1)); //right up
                }
            }

            return neighbours;
        }

        public override string Part2()
        {
            return Simulate(10000,true, false).ToString();
        }
    }

}