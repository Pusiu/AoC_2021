using System;
namespace AoC_2021
{
    public class Day14 : Day
    {
        string template;
        Dictionary<string, string> rules = new Dictionary<string, string>();
        public override string Part1()
        {
            input = GetInput().Result;
            var split = input.Split("\n\n");
            template = split[0];
            rules = split[1].Split("\n").Select(x => x.Split(" -> ")).ToDictionary(x => x[0], x => x[1]);

            var polymer = Simulate(10).OrderBy(x => x.Value);

            var min = polymer.First();
            var max = polymer.Last();
            Console.WriteLine($"{max.Key}:{max.Value}-{min.Key}{min.Value}={max.Value - min.Value}");
            return (max.Value - min.Value).ToString();
        }

        Dictionary<char,long> Simulate(int stepCount)
        {
            var pairs = new Dictionary<string, long>();

            foreach (var r in rules)
            {
                string[] combinations = new string[]
                {
                    r.Key,
                    r.Key[0]+r.Value,
                    r.Value+r.Key[1],
                };
                foreach (var c in combinations)
                {
                    if (!pairs.ContainsKey(c))
                    {
                        pairs.Add(c, 0);
                    }
                }
            }

            var letters=new Dictionary<char, long>();
            foreach (var l in template)
            {
                if (!letters.ContainsKey(l))
                {
                    letters.Add(l, 0);
                }
                letters[l]++;
            }
            for (int i=0; i < template.Length-1;i++)
            {
                var pair = template[i].ToString() + template[i + 1].ToString();
                if (pairs.ContainsKey(pair))
                {
                    pairs[pair]++;
                }
            }
            for (int i=0; i < stepCount;i++)
            {
                var newDict = new Dictionary<string, long>();
                foreach (var p in rules)
                {
                    var thisPair = p.Key;
                    var leftPair = p.Key[0]+p.Value;
                    var rightPair = p.Value+p.Key[1];
                    if (!newDict.ContainsKey(leftPair))
                    {
                        newDict.Add(leftPair, 0);
                    }
                    if (!newDict.ContainsKey(rightPair))
                    {
                        newDict.Add(rightPair, 0);
                    }
                    if (!newDict.ContainsKey(thisPair))
                    {
                        newDict.Add(thisPair, 0);
                    }
                    newDict[leftPair] += pairs[thisPair];
                    newDict[rightPair] += pairs[thisPair];
                    if (!letters.ContainsKey(p.Value[0]))
                    {
                        letters.Add(p.Value[0],0);
                    }
                    letters[p.Value[0]]+=pairs[thisPair];
                }
                pairs = newDict;
            }
            return letters;
        }

        public override string Part2()
        {            
            var polymer = Simulate(40).OrderBy(x => x.Value);
            var min = polymer.First();
            var max = polymer.Where(x => x.Value > 0).Last();
            Console.WriteLine($"After 40 steps, the polymer is {polymer.Select(x => x.Value).Sum()} letters long. Max: {max.Key}:{max.Value}-{min.Key}{min.Value}={max.Value - min.Value}");
            return (max.Value - min.Value).ToString();
        }
    }
}