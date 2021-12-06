namespace AoC_2021
{
    public class Day06 : Day
    {
        List<ulong> fishes = new List<ulong>();
        public override string Part1()
        {
            input = "3,4,3,1,2";
            input=GetInput().Result;

            fishes = input.Split(',').Select(x => ulong.Parse(x)).ToList();

            return Simulate(80).ToString();
        }

        ulong Simulate(int days)
        {
            var fishCopy = new List<ulong>(fishes);
            Dictionary<int, ulong> fishTemplate = new Dictionary<int, ulong>()
            {
                {0,0},
                {1,0},
                {2,0},
                {3,0},
                {4,0},
                {5,0},
                {6,0},
                {7,0},
                {8,0},

            };
            var fishCount = new Dictionary<int, ulong>(fishTemplate);
            fishCopy.ForEach(x => fishCount[(int)x]++);
            /*Console.Write("Initial state: ");
            foreach ((int k,ulong v) in fishCount)
            {
                Console.Write($"{k}:{v} ");
            }*/
            for (int i = 1; i <= days; i++)
            {
                //Console.Write($"\nAfter {i} days: ");

                var newCount = new Dictionary<int,ulong>(fishCount);

                for (int j=fishCount.Count-1; j > 0; j--)
                {
                    if (j==1)
                    {
                        newCount[fishCount.Count-1] = fishCount[0]; //creating new
                        newCount[6] += fishCount[0]; //resets cycle
                        newCount[0] = fishCount[1];
                    }
                    else
                    {
                        newCount[j-1] = fishCount[j];
                    }
                }
                /*foreach ((int k,ulong v) in newCount)
                {
                    Console.Write($"\t{k}: {v} ");
                }*/
                fishCount=newCount;
            }
            Console.WriteLine();
            return fishCount.Select(x => x.Value).Aggregate((x,y) => x+y);
        }

        public override string Part2()
        {
            return Simulate(256).ToString();
        }
    }
}