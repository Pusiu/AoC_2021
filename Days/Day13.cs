using System.Drawing;
using System.Globalization;
using System.Security.Principal;
using System.Text.RegularExpressions;
namespace AoC_2021
{
    public class Day13 : Day
    {
        int[,] paper;
        List<(char axis, int value)> instructions = new List<(char axis, int value)>();
        public override string Part1()
        {
            input = GetInput().Result;
            var split = input.Split("\n\n");
            var dots = split[0].Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split(",")).Select(x => new { x = int.Parse(x[0]), y = int.Parse(x[1]) }).ToArray();

            paper = new int[dots.Max(x => x.x) + 1, dots.Max(x => x.y) + 1];
            foreach (var dot in dots)
            {
                paper[dot.x, dot.y] = 1;
            }
            foreach (var instruction in split[1].Split("\n"))
            {
                var match = Regex.Match(instruction, @"(x|y)=\d+");
                var axis = match.Value[0];
                var value = int.Parse(match.Value.Substring(2));
                instructions.Add((axis, value));
            }
            Fold(instructions[0].axis == 'y', instructions[0].value);
            return paper.Cast<int>().Count(x => x > 0).ToString();
        }

        void Fold(bool up, int foldPoint)
        {
            var tempPaper = new int[(!up) ? foldPoint : paper.GetLength(0), (up) ? foldPoint : paper.GetLength(1)];

            for (int y = 0; y < tempPaper.GetLength(1); y++)
            {
                for (int x = 0; x < tempPaper.GetLength(0); x++)
                {
                    tempPaper[x, y] = paper[x, y];
                }
            }
            if (up)
            {
                for (int y=foldPoint+1; y<paper.GetLength(1); y++)
                {
                    for (int x = 0; x < paper.GetLength(0); x++)
                    {
                        tempPaper[x, tempPaper.GetLength(1)-1-(y-foldPoint-1)] += paper[x, y];
                    }
                }
            }
            else
            {
                for (int y = 0; y < paper.GetLength(1); y++)
                {
                    for(int x=foldPoint+1; x<paper.GetLength(0); x++)
                    {
                        tempPaper[tempPaper.GetLength(0)-1-(x-foldPoint-1), y] += paper[x, y];
                    }
                }
            }
            
            paper=tempPaper;
            Console.WriteLine($"After fold along {(up ? "y" : "x")}={foldPoint}, {paper.Cast<int>().Count(x => x > 0).ToString()} dots are visible");
        }

        void PrintPaper(char axis = ' ', int value=0, ConsoleColor dotColor=ConsoleColor.Gray)
        {
            Console.WriteLine($"Paper size: {paper.GetLength(0)}x{paper.GetLength(1)}");
            for (int y = 0; y < paper.GetLength(1); y++)
            {
                for (int x = 0; x < paper.GetLength(0); x++)
                {
                    if (axis != ' ')
                        if (axis == 'x' && x == value)
                            Console.ForegroundColor = ConsoleColor.Red;
                        else if (axis == 'y' && y == value)
                            Console.ForegroundColor = ConsoleColor.Red;

                    if (paper[x, y] >= 1)
                    {
                        Console.ForegroundColor=dotColor;
                        System.Console.Write("#");
                    }
                    else
                    {
                        Console.ForegroundColor=ConsoleColor.Gray;
                        System.Console.Write(".");
                    }
                }
                System.Console.WriteLine();
            }
        }

        public override string Part2()
        {
            for (int i=1; i < instructions.Count;i++)
            {
                Fold(instructions[i].axis == 'y', instructions[i].value);
            }
            PrintPaper(dotColor: ConsoleColor.Green);
            return base.Part2();
        }
    }
}