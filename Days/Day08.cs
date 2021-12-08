using System.Globalization;
using System.Text.RegularExpressions;

namespace AoC_2021
{
    public class Day08 : Day
    {

        List<string> outputValues = new List<string>();
        public override string Part1()
        {
            input=GetInput().Result;
            var rgx = @"\|\r?\n? ?(\w+ ?){4}";
            var matches = Regex.Matches(input, rgx, RegexOptions.Multiline);
            outputValues = new List<string>();
            for (int i = 0; i < matches.Count; i++)
            {
                matches[i].Groups[1].Captures.Select(x => x.Value).ToList().ForEach(x => outputValues.Add(x.Trim()));
            }

            var easyDigits = new Dictionary<int, int>()
            {
                {1,2}, //uses 2 segments display 1 
                {4,4}, //uses 4 segments to display 4
                {7,3},
                {8,7}
            };

            var easyDigitsCount = 0;
            foreach (var val in outputValues)
            {
                if (easyDigits.Values.Contains(val.Length))
                {
                    easyDigitsCount++;
                }
            }

            return easyDigitsCount.ToString();
        }

        public override string Part2()
        {
            input = "acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf";
            input = GetInput().Result;
            int numbers=0;
            foreach (var line in input.Split("\n"))
            {
                var inputValues = Regex.Match(line, @"(\w+ ?){10}").Groups[1].Captures.Select(x => x.Value.Trim()).ToList();
                var outputValues = Regex.Match(line, @"\| (\w+ ?){4}").Groups[1].Captures.
                Select(x => x.Value.Trim()).
                Select(x => string.Join("", x).ToCharArray().OrderBy(x => x)).Select(x => string.Join("", x)).ToList();

                var wires = inputValues.Select(x => x.OrderBy(y => y)).Select(x => string.Join("", x).ToCharArray()).ToList();


                var one = wires.Where(x => x.Length == 2).First();
                var seven = wires.Where(x => x.Length == 3).First();
                var four = wires.Where(x => x.Length == 4).First();
                var eight = wires.Where(x => x.Length == 7).First();
                var top = seven.Except(one);
                var middle_topLeft = four.Except(one);
                var bottom_bottomLeft = eight.Except(four).Except(top);
                var nine = wires.Where(x => x.Length == 6 && x.Except(four).Except(top).Count() == 1).First();
                var bottom = nine.Except(four).Except(top);
                var bottomLeft = bottom_bottomLeft.Except(bottom);
                var zero = wires.Where((x) => x.Length == 6 && x.Except(seven).Except(bottom_bottomLeft).Count() == 1).First();
                var topLeft = zero.Except(seven).Except(bottom_bottomLeft);
                var middle = middle_topLeft.Except(topLeft);
                var six = wires.Where((x) => x.Length == 6 && x.Except(top).Except(middle_topLeft).Except(bottom).Except(bottomLeft).Count() == 1).First();
                var bottomRight = six.Except(top).Except(middle_topLeft).Except(bottom).Except(bottomLeft);
                var topRight = one.Except(bottomRight);
                var two = eight.Except(bottomRight).Except(topLeft).ToArray();
                var three = nine.Except(topLeft).ToArray();
                var five = six.Except(bottomLeft).ToArray();

                Dictionary<string, int> mapping = new Dictionary<string, int>()
            {
                {new string(zero), 0},
                {new string(one), 1},
                {new string(two), 2},
                {new string(three), 3},
                {new string(four), 4},
                {new string(five), 5},
                {new string(six), 6},
                {new string(seven), 7},
                {new string(eight), 8},
                {new string(nine), 9}
            };

                foreach (var m in mapping)
                {
                    Console.WriteLine($"{m.Key} : {m.Value}");
                }
                
                string numberString = "";
                foreach (var val in outputValues)
                {
                    numberString += mapping[val];
                }
                Console.WriteLine(numberString);;
                numbers += int.Parse(numberString);
            }

            return numbers.ToString();
        }
    }
}