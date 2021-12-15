using System.Reflection.Emit;
namespace AoC_2021
{
    public class Day15 : Day
    {
        int[,] referenceMap;
        public override string Part1()
        {
            input = GetInput().Result;
            var split = input.Split("\n");
            referenceMap = new int[split[0].Length, split.Length];
            for (int y = 0; y < split.Length; y++)
            {
                for (int x = 0; x < split[y].Length; x++)
                {
                    referenceMap[x, y] = int.Parse(split[y][x].ToString());
                }
            }

            return Dijkstra((0,0), (referenceMap.GetLength(0) - 1, referenceMap.GetLength(1) - 1), referenceMap).ToString();
        }

        public int Dijkstra((int x, int y) start, (int x,int y) end, int[,] map)
        {
            var queue = new PriorityQueue<(int x, int y), int>();
            queue.Enqueue(start,0);
            var distance = new Dictionary<(int x, int y), int>();
            distance.Add(start, 0);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current == end)
                {
                    return distance[current];
                }
                var neighbours = new List<(int x, int y)>();
                if (current.x > 0)
                {
                    neighbours.Add((current.x - 1, current.y));
                }
                if (current.x < map.GetLength(0) - 1)
                {
                    neighbours.Add((current.x + 1, current.y));
                }
                if (current.y > 0)
                {
                    neighbours.Add((current.x, current.y - 1));
                }
                if (current.y < map.GetLength(1) - 1)
                {
                    neighbours.Add((current.x, current.y + 1));
                }
                foreach (var n in neighbours)
                {
                    var newDistance = distance[current] + map[n.x, n.y];
                    if (distance.ContainsKey(n))
                    {
                        if (distance[n] > newDistance)
                        {
                            distance[n] = newDistance;
                        }
                    }
                    else
                    {
                        distance.Add(n, newDistance);
                        queue.Enqueue(n, newDistance);
                    }
                }
            }
            return -1;
        }

        public override string Part2()
        {
            var largerMap = new int[referenceMap.GetLength(0) * 5, referenceMap.GetLength(1) * 5];

            for (int y=0; y < largerMap.GetLength(1);y++)
            {
                for (int x=0; x < largerMap.GetLength(0);x++)
                {
                    var tile=(x/referenceMap.GetLength(0))+y/referenceMap.GetLength(1);
                    var value = ((referenceMap[x % referenceMap.GetLength(0), y % referenceMap.GetLength(1)]+(tile)-1)%9) + 1;
                    largerMap[x, y] =  value;
                }
            }
            var risk = Dijkstra((0,0), (largerMap.GetLength(0) - 1, largerMap.GetLength(1) - 1), largerMap);


            return risk.ToString();
        }
    }
}