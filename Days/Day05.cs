using System.Text.RegularExpressions;

namespace AoC_2021
{
    public class Day05 : Day
    {
        int[,] map;
        string[] splitted;
        public override string Part1()
        {
            input = GetInput().Result;

            splitted = input.Trim().Replace("\r", "").Split('\n');
            var max = splitted.Select(x => Regex.Match(x, @"\d+")).SelectMany(x => x.Captures).Max(x => int.Parse(x.Value));
            map = new int[max + 1, max + 1];
            Solve(false,false);
            
            return map.Cast<int>().Count(x => x > 1).ToString();
        }

        public override string Part2()
        {
            map = new int[map.GetLength(0), map.GetLength(1)];
            Solve(true, false);
            return map.Cast<int>().Count(x => x > 1).ToString();
        }

        void Solve(bool countDiagonals, bool log = false)
        {

            foreach (var line in splitted)
            {
                var match = Regex.Match(line, @"(\d+),(\d+) -> (\d+),(\d+)");
                var x1 = int.Parse(match.Groups[1].Value);
                var y1 = int.Parse(match.Groups[2].Value);
                var x2 = int.Parse(match.Groups[3].Value);
                var y2 = int.Parse(match.Groups[4].Value);

                if (countDiagonals || (!countDiagonals && (x1 == x2 || y1 == y2)))
                {
                    int xIncr = x1 < x2 ? 1 : (x1 > x2 ? -1 : 0);
                    int yIncr = y1 < y2 ? 1 : (y1 > y2 ? -1 : 0);
                    int curX = x1;
                    int curY = y1;
                    map[curX, curY]++;
                    while (curX != x2 || curY != y2)
                    {
                        if (curX != x2)
                        {
                            curX += xIncr;
                        }
                        if (curY != y2)
                        {
                            curY += yIncr;
                        }
                        if (curX >= map.GetLength(0) || curY >= map.GetLength(1) || curX < 0 || curY < 0)
                        {
                            break;
                        }
                        map[curX, curY]++;
                        if (log)
                        {
                            System.Console.WriteLine($"{line} adds to {curX},{curY}");
                        }
                    }
                }
            }

            if (log)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    for (int x = 0; x < map.GetLength(0); x++)
                    {
                        Console.Write((map[x, y] > 0) ? map[x, y] : ".");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}