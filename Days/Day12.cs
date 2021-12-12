namespace AoC_2021
{
    class Cave {
        public string name;
        public bool isBig;
        public List<Cave> connections = new List<Cave>();
        public int maxOccurancesInPath=1;

        public Cave(string name, bool isBig) {
            this.name = name;
            this.isBig = isBig;
        }

        public void AddConnection(Cave cave) {
            if (connections.Contains(cave)) return;
            connections.Add(cave);
        }
    }
    public class Day12 : Day
    {
        Dictionary<string,Cave> caves = new Dictionary<string, Cave>();
        public override string Part1()
        {
            LoadCaves(GetInput().Result);

            var paths = Traverse(new List<Cave>(), caves["start"]);
            //paths.ForEach(x => Console.WriteLine(string.Join("->", x.Select(y=> y.name).ToList())));
            return paths.Count.ToString();
        }

        void LoadCaves(string input)
        {
            var lines = input.Replace("\r","").Split("\n");
            caves.Clear();
            foreach (var line in lines)
            {
                var parts = line.Split("-");
                Cave[] curCaves = new Cave[parts.Length];
                for (int i=0; i < parts.Length;i++)
                {
                    Cave c;
                    if (caves.ContainsKey(parts[i])) 
                        c= caves[parts[i]];
                    else
                    {
                        c= new Cave(parts[i], parts[i].ToUpper() == parts[i]);
                        if (parts[i].ToLower() == "start" || parts[i].ToLower() == "end")
                        {
                            c.isBig=false;
                            c.maxOccurancesInPath=1;
                        }
                        caves.Add(c.name, c);
                    }
                    curCaves[i]=c;
                }
                curCaves[0].AddConnection(curCaves[1]);
                curCaves[1].AddConnection(curCaves[0]);
            }
        }
        List<List<Cave>> Traverse(List<Cave> visited, Cave currentCave, bool useVisitCount=false)
        {
            visited.Add(currentCave);
            if (currentCave.name == "end") return new List<List<Cave>>() { visited };
            var paths = new List<List<Cave>>();

            foreach (var conn in currentCave.connections)
            {
                if (visited.Contains(conn) && !conn.isBig)
                {
                    if (useVisitCount)
                    {
                        var occurancesInPath = visited.Count(x => x.name == conn.name);
                        if (occurancesInPath >= conn.maxOccurancesInPath) continue;
                    }
                    else
                        continue;
                }
                var connPaths = Traverse(new List<Cave>(visited), conn, useVisitCount);
                foreach (var path in connPaths)
                {
                    paths.Add(path);
                }
            }

            return paths;
        }

        public override string Part2()
        {
            List<Cave> cavesToVisitTwice = new List<Cave>(caves.Values.Where(x => x.name != "start" && x.name != "end" && !x.isBig).ToList());
            List<string> totalPaths = new List<string>();
            foreach (var cave in cavesToVisitTwice)
            {
                LoadCaves(input);
                caves[cave.name].maxOccurancesInPath=2;
                var paths = Traverse(new List<Cave>(), caves["start"], true);
                foreach (var path in paths)
                {
                    var p = string.Join(",", path.Select(x=> x.name).ToList());
                    totalPaths.Add(p);
                }
            }
            totalPaths = totalPaths.Distinct().ToList();
            //totalPaths.ForEach(x=> Console.WriteLine(x));
            return totalPaths.Count.ToString();
        }
    }
}