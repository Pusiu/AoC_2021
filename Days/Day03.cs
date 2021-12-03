using System.Reflection;
using System;
namespace AoC_2021
{
    public class Day03 : Day
    {
        string[] inputArray;
        public override string Part1()
        {
            inputArray = GetInput().Result.Trim().Split("\n");

            int[] mask = new int[inputArray[0].Length];
            for (int i=0; i < inputArray[0].Length;i++)
            {
                for (int j=0; j < inputArray.Length;j++)
                {
                    mask[i] += inputArray[j][i] == '0' ? -1 : 1;
                }
            }

            string stringMask = string.Join("", mask.Select(x => x > 0 ? "1" : "0").ToArray());
            int gamma = BinaryToDecimal(stringMask);
            int epsilon = BinaryToDecimal(new string(stringMask.Select(x => x=='1' ? '0' : '1').ToArray()));

            return (gamma*epsilon).ToString();
        }

        int BinaryToDecimal(string binary)
        {
            int decimalNumber = 0;
            int power = 0;
            for (int i = binary.Length - 1; i >= 0; i--)
            {
                if (binary[i] == '1')
                {
                    decimalNumber += (int)Math.Pow(2, power);
                }
                power++;
            }
            return decimalNumber;
        }

        public override string Part2()
        {

            List<string> oxygenGeneratorList = new List<string>(inputArray);
            List<string> co2ScrubberList = new List<string>(inputArray);

            int currentBit=0;
            while (true)
            {
                if (oxygenGeneratorList.Count > 1)
                    oxygenGeneratorList = oxygenGeneratorList.GroupBy(x => x[currentBit])
                        .OrderByDescending(x => x.Count()).ThenByDescending(x => x.Key == '1')
                        .First().ToList();  
                if (co2ScrubberList.Count > 1)
                    co2ScrubberList = co2ScrubberList.GroupBy(x => x[currentBit])
                        .OrderBy(x => x.Count()).ThenByDescending(x => x.Key == '0')
                        .First().ToList();
                
                if (co2ScrubberList.Count == 1 && oxygenGeneratorList.Count == 1)
                    break;

                currentBit++;              
            }
            int oxygenGeneratorRating=BinaryToDecimal(oxygenGeneratorList[0]);
            int co2ScrubberRating=BinaryToDecimal(co2ScrubberList[0]);


            return (oxygenGeneratorRating*co2ScrubberRating).ToString();
        }
    }
}