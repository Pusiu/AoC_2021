using System.Text;
namespace AoC_2021
{
    class Packet
    {
        public string version = "";
        public string type = "";
        public string data = "";

        public string metadata = "";

        public int PacketLength => version.Length + type.Length + data.Length + metadata.Length;

        public List<Packet> subpackets = new List<Packet>();

        public override string ToString()
        {
            return $"{version}{type}{metadata}{data}";
        }

        public long GetValue()
        {
            long val = 0;
            switch (Day16.BinaryToDecimal(type))
            {
                case 0:
                    foreach (var p in subpackets)
                    {
                        val += p.GetValue();
                    }
                    break;
                case 1:
                    var k = subpackets[0].GetValue();
                    for (int i = 1; i < subpackets.Count; i++)
                    {
                        k *= subpackets[i].GetValue();
                    }
                    val = k;
                    break;
                case 2:
                    val = subpackets.Select(x => x.GetValue()).Min();
                    break;
                case 3:
                    val = subpackets.Select(x => x.GetValue()).Max();
                    break;
                case 4:
                    val = Day16.BinaryToDecimal(data);
                    break;
                case 5:
                    val = subpackets[0].GetValue() > subpackets[1].GetValue() ? 1 : 0;
                    break;
                case 6:
                    val = subpackets[0].GetValue() < subpackets[1].GetValue() ? 1 : 0;
                    break;
                case 7:
                    val = subpackets[0].GetValue() == subpackets[1].GetValue() ? 1 : 0;
                    break;
            }

            return val;
        }
    }

    public class Day16 : Day
    {
        long versionSum = 0;
        public override string Part1()
        {
            //input = "C200B40A82";
            input = GetInput().Result.Trim();
            var binary = HexToBinary(input);
            var p = ProcessPacket(binary);

            return versionSum.ToString();
        }

        public override string Part2()
        {
            input = GetInput().Result.Trim();
            //input="9C005AC2F8F0";
            var binary = HexToBinary(input);
            var p = ProcessPacket(binary);

            return p.GetValue().ToString();
        }

        Packet ProcessPacket(string binary, bool log = false)
        {
            Packet p = new Packet();
            p.version = binary.Substring(0, 3);
            versionSum += BinaryToDecimal(p.version);
            p.type = binary.Substring(3, 3);
            var t = BinaryToDecimal(p.type);
            var i = 6;
            if (t == 4)
            {
                var d = ReadData(binary.Substring(i));
                p.data = d.data;
                p.metadata = d.metadata;
            }
            else
            {
                var lengthTypeId = binary[i++];
                p.metadata += lengthTypeId;
                if (lengthTypeId == '1')
                {
                    var totalNumberOfSubpackets = BinaryToDecimal(binary.Substring(i, 11));
                    p.metadata += binary.Substring(i, 11);
                    i += 11;
                    for (int j = 0; j < totalNumberOfSubpackets; j++)
                    {
                        var subPacket = ProcessPacket(binary.Substring(i));
                        i += subPacket.PacketLength;
                        p.data += subPacket.ToString();
                        p.subpackets.Add(subPacket);
                    }
                }
                else
                {
                    var totalLengthInBits = BinaryToDecimal(binary.Substring(i, 15));
                    p.metadata += binary.Substring(i, 15);
                    i += 15;
                    var currentLength = 0;
                    while (currentLength < totalLengthInBits)
                    {
                        Packet sp = ProcessPacket(binary.Substring(i));
                        currentLength += sp.PacketLength;
                        i += sp.PacketLength;
                        p.data += sp.ToString();
                        p.subpackets.Add(sp);
                    }
                }
            }

            if (log)
            {
                string s = $"Packet read, length: {p.PacketLength} version: {p.version}({BinaryToDecimal(p.version)}), type: {p.type}({BinaryToDecimal(p.type)}), data (length: {p.data.Length}): {p.data}";
                if (p.type == "100")
                {
                    s += $", decimal value: {BinaryToDecimal(p.data)}";
                }
                Console.WriteLine(s);
            }
            return p;
        }

        (string data, string metadata) ReadData(string dataBlock)
        {
            StringBuilder data = new StringBuilder();
            StringBuilder metadata = new StringBuilder();
            int i = 0;
            while (i < dataBlock.Length)
            {
                char flag = dataBlock[i];
                metadata.Append(flag);
                data.Append(dataBlock, i + 1, 4);
                if (flag == '0')
                    break;
                i += 5;
            }
            return (data.ToString(), metadata.ToString());
        }

        public static long BinaryToDecimal(string binary)
        {
            long result = 0;
            for (int i = 0; i < binary.Length; i++)
            {
                result += (long)Math.Pow(2, binary.Length - i - 1) * (binary[i] - '0');
            }
            return result;
        }

        string HexToBinary(string hex)
        {
            Dictionary<char, string> map = new Dictionary<char, string>()
            {
                { '0', "0000"},
                { '1', "0001"},
                { '2', "0010"},
                { '3', "0011"},
                { '4', "0100"},
                { '5', "0101"},
                { '6', "0110"},
                { '7', "0111"},
                { '8', "1000"},
                { '9', "1001"},
                { 'A', "1010"},
                { 'B', "1011"},
                { 'C', "1100"},
                { 'D', "1101"},
                { 'E', "1110"},
                { 'F', "1111"},
            };

            StringBuilder sb = new StringBuilder();
            foreach (char c in hex)
            {
                sb.Append(map[c]);
            }
            return sb.ToString();
        }
    }
}