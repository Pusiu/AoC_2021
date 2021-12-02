namespace AoC_2021
{
    public class Day02 : Day
    {
        int horizontal = 0;
        int depth = 0;

        int aim=0;
        string[] inputArray;
        public override string Part1()
        {
            GetInput();
            inputArray = input.Split("\n");
            foreach (var line in inputArray)
            {
                var command = line.Split(" ");
                switch (command[0])
                {
                    case "forward":
                        horizontal += int.Parse(command[1]);
                        break;
                    case "up":
                        depth -= int.Parse(command[1]);
                        break;
                    case "down":
                        depth += int.Parse(command[1]);
                        break;
                }
            }
            return(depth*horizontal).ToString();
        }

        public override string Part2()
        {
            depth=horizontal=0;
            foreach (var line in inputArray)
            {
                var command = line.Split(" ");
                switch (command[0])
                {
                    case "forward":
                        horizontal += int.Parse(command[1]);
                        depth+=aim * int.Parse(command[1]);
                        break;
                    case "up":
                        aim -= int.Parse(command[1]);
                        break;
                    case "down":
                        aim += int.Parse(command[1]);
                        break;
                }
            }

            return (depth*horizontal).ToString();
        }
    }
}