namespace AoC_2021
{
    public class Day09 : Day
    {

        int[,] map;
        List<(int x, int y)> lowestPoints;
        bool log=false;

        public override string Part1()
        {
            input=GetInput().Result;

            var lines = input.Replace("\r","").Split("\n");
            map = new int[lines[0].Length, lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    map[j, i] = int.Parse(lines[i][j].ToString());
                }
            }

            lowestPoints= new List<(int x , int y)>();
            for (int y=0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    bool isLowest=true;
                    var neighbours = GetNeighbours(x, y);
                    foreach (var n in neighbours)
                    {
                        if (map[n.x, n.y] < map[x, y])
                        {
                            isLowest = false;
                            break;
                        }
                    }
                    if (isLowest) lowestPoints.Add((x,y));

                    if (log)
                    {
                        Console.ForegroundColor = (isLowest) ? ConsoleColor.Red : ConsoleColor.White;
                        Console.Write(map[x, y]);
                    }
                }
                if (log) Console.WriteLine();
            }

            var riskLevel=0;
            foreach (var p in lowestPoints) riskLevel += 1+map[p.x,p.y];

            return riskLevel.ToString();
        }

        List<(int x, int y)> GetNeighbours(int x, int y)
        {
            List<(int x, int y)> neighbours = new List<(int x, int y)>();
            if (x > 0)
            {
                neighbours.Add((x - 1, y));
            }
            if (x < map.GetLength(0) - 1)
            {
                neighbours.Add((x + 1, y));
            }
            if (y > 0)
            {
                neighbours.Add((x, y - 1));
            }
            if (y < map.GetLength(1) - 1)
            {
                neighbours.Add((x, y + 1));
            }
            return neighbours;
        }

        public override string Part2()
        {
            var basins = new List<List<(int x,int y)>>();

            foreach (var lp in lowestPoints)
            {
                Queue<(int x, int y)> pointsToVisit = new Queue<(int x, int y)>();
                pointsToVisit.Enqueue(lp);
                var basin = new List<(int x, int y)>();
                while(pointsToVisit.Count>0)
                {
                    var p = pointsToVisit.Dequeue();
                    if (map[p.x, p.y] == 9) continue;
                    
                    var neighbours = GetNeighbours(p.x, p.y);
                    if (basin.Contains(p)) continue;

                    basin.Add(p);
                    foreach (var n in neighbours) pointsToVisit.Enqueue(n);
                }
                basins.Add(basin);
            }
            var colors = new []{ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.Magenta, ConsoleColor.Cyan};
            for (int y=0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    var basin = basins.FirstOrDefault(b => b.Contains((x, y)));
                    if (basin != null) Console.ForegroundColor = colors[basins.IndexOf(basin) % colors.Length];
                    else Console.ForegroundColor = ConsoleColor.White;

                    if (log) Console.Write(map[x, y]);
                }
                if (log) Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
            return basins.OrderByDescending(x => x.Count).Take(3).Select(x => x.Count).Aggregate((a,b) => a*b).ToString();
        }
    }
}