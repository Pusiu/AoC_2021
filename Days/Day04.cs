namespace AoC_2021
{
    public class Day04 : Day
    {

        int[] numbers;
        int resolvedNumber = -1;
        List<int[,]> boards = new List<int[,]>();
        int[,] winnerBoard = new int[5, 5];

        public override string Part1()
        {
            input = GetInput().Result;
            var lines = input.Trim().Replace("\r", "").Split("\n");
            numbers = lines[0].Split(",").Select(int.Parse).ToArray();

            int[,] board = new int[5, 5];
            for (int i = 2; i < lines.Length; i += 6)
            {
                var boardNumbers = lines.Take(new Range(i, i + 5)).SelectMany(x => x.Split(" ")).Where(x => x != "").Select(x => int.Parse(x)).ToArray();
                for (int x = 0; x < 5; x++)
                    for (int y = 0; y < 5; y++)
                        board[x, y] = boardNumbers[x * 5 + y];

                boards.Add(board);
                board = new int[5, 5];
            }

            foreach (var num in numbers)
            {
                bool resolved = false;
                foreach (var b in boards)
                {
                    for (int x = 0; x < 5; x++)
                        for (int y = 0; y < 5; y++)
                            if (b[x, y] == num)
                                b[x, y] = -b[x, y];

                    if (IsResolved(b))
                    {
                        resolved = true;
                        resolvedNumber = num;
                        winnerBoard = b;
                    }
                }
                if (resolved)
                    break;
            }

            return (winnerBoard.Cast<int>().Where(x => x > 0).Sum() * resolvedNumber).ToString();
        }

        bool IsResolved(int[,] board)
        {
            for (int x = 0; x < 5; x++)
            {
                bool isResolved = true;
                for (int y = 0; y < 5; y++)
                {
                    if (board[x, y] > 0)
                    {
                        isResolved = false;
                        break;
                    }
                }
                if (isResolved)
                {
                    return true;
                }
            }
            for (int y = 0; y < 5; y++)
            {
                bool isResolved = true;
                for (int x = 0; x < 5; x++)
                {
                    if (board[x, y] > 0)
                    {
                        isResolved = false;
                        break;
                    }
                }
                if (isResolved)
                    return true;
            }

            return false;
        }

        public override string Part2()
        {
            var lastIndex = numbers.Cast<int>().ToList().IndexOf(resolvedNumber);
            int[,] lastToBeResolved = new int[5, 5];
            var boardsToUse = new List<int[,]>(boards);
            boardsToUse.RemoveAll(x => x == winnerBoard);

            for (int i = lastIndex + 1; i < numbers.Length; i++)
            {
                var num = numbers[i];
                var toRemove = new List<int[,]>();
                foreach (var b in boardsToUse)
                {
                    for (int x = 0; x < 5; x++)
                        for (int y = 0; y < 5; y++)
                            if (b[x, y] == num)
                                b[x, y] = -b[x, y];

                    if (IsResolved(b))
                    {
                        resolvedNumber = num;
                        lastToBeResolved = b;
                        toRemove.Add(b);
                    }
                }
                boardsToUse.RemoveAll(x => toRemove.Contains(x));
                if (boardsToUse.Count == 0)
                    break;
            }
            return (lastToBeResolved.Cast<int>().Where(x => x > 0).Sum() * resolvedNumber).ToString();
        }
    }
}