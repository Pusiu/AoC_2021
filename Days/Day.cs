using System.Reflection;
using System.Net;
using System.Text.RegularExpressions;

namespace AoC_2021
{
    public abstract class Day
    {
        protected string input;
        public void Run()
        {
            Console.WriteLine($"Part 1: {Part1()}\n");
            Console.WriteLine($"Part 2: {Part2()}\n");
        }

        public async Task<string> GetInput()
        {
            var baseDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
            var inputDirectory = baseDirectory + @"\Inputs\";
            Console.WriteLine(inputDirectory);
            var match = Regex.Match(GetType().ToString(), @"\d+$");
            int dayNumber = int.Parse(match.Value);


            var filename = $"day{dayNumber}.txt";
            if (File.Exists(inputDirectory + filename))
            {
                input = File.ReadAllText(inputDirectory + filename);
            }
            else
            {
                var cookiePath = baseDirectory + @"\cookie.txt";
                var cookie="";
                if (File.Exists(cookiePath))
                {
                    cookie = File.ReadAllText(cookiePath);
                }
                else
                {
                    Console.WriteLine("Session cookie does not exist, please provie a session cookie: ");
                    cookie = Console.ReadLine();
                    File.WriteAllText(cookiePath, cookie);
                }
                var url = $"https://adventofcode.com/2021/day/{dayNumber.ToString()}/input";
                var client = new HttpClient();
                var resp = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url)
                {
                    Headers = { { "Cookie", cookie } }
                });
                var inp = await resp.Content.ReadAsStringAsync();
                //var resp = await client.GetStringAsync(url);
                this.input=inp;
                File.WriteAllText(inputDirectory + filename, inp);
            }

            return input;
        }

        public virtual string Part1()
        {
            return "";
        }

        public virtual string Part2()
        {
            return "";
        }
    }
}