namespace AoC_2021
{
    public class Day10 : Day
    {
        List<char> openingChars = new List<char> { '(', '[', '{', '<' };
        List<char> closingChars = new List<char> { ')', ']', '}', '>' };
        List<int> syntaxErrorPoints = new List<int> { 3, 57, 1197, 25137 };
        List<int> autocompletePoints = new List<int> { 1, 2, 3, 4 };

        List<string> incomplete = new List<string>();
        public override string Part1()
        {
            input = GetInput().Result;

            var splitted = input.Replace("\r", "").Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            long score = 0;
            foreach (var s in splitted)
            {
                var x = Validate(s);
                score += x.syntaxScore;
                if (x.valid) incomplete.Add(s);
            }
            return score.ToString();
        }

        public (bool valid, long syntaxScore) Validate(string line)
        {
            var stack = new Stack<char>();
            var syntaxScore = 0;
            foreach (var c in line)
            {
                if (openingChars.Contains(c))
                {
                    stack.Push(closingChars[openingChars.IndexOf(c)]);
                }
                else if (closingChars.Contains(c))
                {
                    var closingChar = stack.Pop();
                    if (closingChar != c)
                    {
                        syntaxScore += syntaxErrorPoints[closingChars.IndexOf(c)];
                        Console.WriteLine($"{line} - Expected {closingChar} but found {c} instead; score: {syntaxScore}");
                        return (false, syntaxScore);
                    }
                }
            }
            return (true, 0);
        }

        public override string Part2()
        {
            var completionList = new List<long>();
            foreach (var s in incomplete)
            {
                Stack<char> stack = new Stack<char>();
                var closingSequence = "";
                long totalScore = 0;
                foreach (var c in s)
                {
                    if (openingChars.Contains(c))
                    {
                        stack.Push(closingChars[openingChars.IndexOf(c)]);
                    }
                    else if (closingChars.Contains(c))
                    {
                        var closingChar = stack.Pop();
                    }
                }
                while (stack.Count > 0)
                {
                    var x = stack.Pop();
                    totalScore=totalScore*5 + autocompletePoints[closingChars.IndexOf(x)];
                    closingSequence += x;
                }
                completionList.Add(totalScore);
                Console.WriteLine($"{s} - Complete by adding {closingSequence}, score: {totalScore}");
            }

            return completionList.OrderBy(x => x).ElementAt(completionList.Count/2).ToString();
        }
    }
}
