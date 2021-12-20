using System.Text.RegularExpressions;
namespace AoC_2021
{
    public class Day17 : Day
    {
        List<(int x, int y)> targetAreaTiles = new List<(int x, int y)>();
        public override string Part1()
        {
            input = "target area: x=20..30, y=-10..-5";
            input= GetInput().Result;
            var numbers = Regex.Matches(input, @"-?\d+").Cast<Match>().Select(m => int.Parse(m.Value)).ToArray();
            var minX = numbers[0];
            var maxX = numbers[1];
            var minY = numbers[2];
            var maxY = numbers[3];
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    targetAreaTiles.Add((x, y));
                }
            }

            int yPos = 0;

            for (int yVelocity = (minY * -1) - 1; yVelocity != 0; yVelocity--)
            {
                yPos += yVelocity;
            }

            return yPos.ToString();
        }

        public override string Part2()
        {
            var minY=targetAreaTiles.Min(t => t.y);
            var maxX = targetAreaTiles.Max(t => t.x);
            int maxY = -minY-1;
            int minX=0;

            HashSet<(int x, int y)> velocities = new HashSet<(int x, int y)>();
            for (int y=minY; y<=maxY; y++)
            {
                for (int x=minX; x<=maxX; x++)
                {
                    var stepcount = Simulate(x, y);
                    if (stepcount >= 0)
                    {
                        //Console.Write($"({x},{y}),");
                        velocities.Add((x, y));
                    }
                }
            }
            Console.WriteLine();
            
            
            return velocities.Count().ToString();
        }

        int Simulate(int xVelocity, int yVelocity, bool log=false)
        {
            var maxSteps = 500;
            var x = 0;
            var y = 0;
            List<(int x, int y)> probePositions = new List<(int x, int y)>();
            var deadZoneY = targetAreaTiles.Select(t => t.y).Min();
            var deadZoneX = targetAreaTiles.Select(t => t.x).Max();
            for (int i = 0; i < maxSteps; i++)
            {
                x += xVelocity;
                y += yVelocity;
                if (xVelocity > 0)
                    xVelocity--;
                else if (xVelocity < 0)
                    xVelocity++;

                yVelocity--;

                probePositions.Add((x, y));
                if (targetAreaTiles.Contains((x, y)))
                {
                    if (log)
                        Console.WriteLine($"{i + 1} steps to reach target area");
                    return i+1;
                }
                if (x > deadZoneX || y < deadZoneY)
                {
                    break;
                }
            }
            return -1;
            //DrawTrajectory(probePositions);
        }

        void DrawTrajectory(List<(int x, int y)> probePositions)
        {
            var S = (x: 0, y: 0);

            var minX = Math.Min(Math.Min(probePositions.Min(p => p.x), S.x), targetAreaTiles.Min(p => p.x));
            var maxX = Math.Max(Math.Max(probePositions.Max(p => p.x), S.x), targetAreaTiles.Max(p => p.x));
            var minY = Math.Min(Math.Min(probePositions.Min(p => p.y), S.y), targetAreaTiles.Min(p => p.y));
            var maxY = Math.Max(Math.Max(probePositions.Max(p => p.y), S.y), targetAreaTiles.Max(p => p.y));

            for (int y = maxY; y > minY; y--)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (x == 0 && y == 0)
                    {
                        Console.Write("S");
                    }
                    else if (probePositions.Contains((x, y)))
                    {
                        Console.Write("#");
                    }
                    else if (targetAreaTiles.Contains((x, y)))
                    {
                        Console.Write("T");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}