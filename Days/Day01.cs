namespace AoC_2021
{
    public class Day01 : Day
    {
        string[] inputSplitted;
        public override string  Part1()
        {
            string input = GetInput().Result;
            inputSplitted= input.Trim().Split('\n');

            int increaseCount=0;
            int lastMeasurement = 0;
            for (int i=0; i < inputSplitted.Length; i++)
            {
                int currentMeasurement = int.Parse(inputSplitted[i]);
                if (i > 0)
                {
                    if (currentMeasurement > lastMeasurement)
                    {
                        increaseCount++;
                    }
                }

                lastMeasurement=currentMeasurement;
            }

            return increaseCount.ToString();
        }

        public override string Part2()
        {
            List<int> windows = new List<int>();
            Queue<int> q = new Queue<int>();
            for (int i=0; i < inputSplitted.Length; i++)
            {
                int currentMeasurement = int.Parse(inputSplitted[i]);
                if (q.Count == 3)
                {
                    windows.Add(q.Sum());
                    q.Dequeue();
                }
                q.Enqueue(currentMeasurement);
            }
            windows.Add(q.Sum());

            int increaseCount = 0;
            for (int i=0; i < windows.Count; i++)
            {
                //Console.WriteLine($"Window {i}: {windows[i]} {((i==0) ? "initial" : ((windows[i] > windows[i-1]) ? "increase" : (windows[i] < windows[i-1]) ? "decrease" : "no change" ))}");
                if (i > 0 && windows[i] > windows[i-1])
                {
                    increaseCount++;
                }
            }

            return increaseCount.ToString();
        }
    }
}