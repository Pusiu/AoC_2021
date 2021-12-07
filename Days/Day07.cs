namespace AoC_2021
{
    public class Day07: Day
    {
        int[] positions;
        public override string Part1()
        {
            input = "16,1,2,0,4,2,7,1,2,14";
            input = GetInput().Result;
            positions=input.Split(',').Select(int.Parse).ToArray();

            return Solve(false).ToString();
        }

        int Solve(bool newRules, bool log=false)
        {
            var minimalCost=int.MaxValue;
            var min = positions.Min();
            var max = positions.Max();

            for (int i=min; i<max; i++)
            {
                var targetPosition = i;//positions[i];

                var totalCost=0;
                foreach (var position in positions)
                {
                    var moveCost = Math.Abs(position-targetPosition);
                    if (log)
                        Console.WriteLine($"Move from {position} to {targetPosition}: {moveCost} fuel");

                    if (newRules)
                    {
                        for (int j=1; j <= moveCost; j++)
                        {
                            totalCost+=j;
                        }
                    }
                    else
                        totalCost += moveCost;
                }
                if (log)
                    Console.WriteLine($"Total {totalCost} fuel");
                if (totalCost < minimalCost)
                {
                    minimalCost=totalCost;;
                }
            }

            return minimalCost;
        }

        public override string Part2()
        {
            return Solve(true).ToString();
        }
    }
}