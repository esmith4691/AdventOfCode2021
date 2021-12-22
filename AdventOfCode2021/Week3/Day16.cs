using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime;

namespace AdventOfCode2021.Week1
{
    static class Day16
    {
        private static Dictionary<string, string> _codes = new Dictionary<string, string>
        {
            { "0", "0000" },
            { "1", "0001" },
            { "2", "0010" },
            { "3", "0011" },
            { "4", "0100" },
            { "5", "0101" },
            { "6", "0110" },
            { "7", "0111" },
            { "8", "1000" },
            { "9", "1001" },
            { "A", "1010" },
            { "B", "1011" },
            { "C", "1100" },
            { "D", "1101" },
            { "E", "1110" },
            { "F", "1111" }
        };

        public static void Day16A()
        {
            var input = string.Join("", File.ReadAllText("./Inputs/Day16Input.txt").ToCharArray().Select(c => _codes[c.ToString()]));
            IEnumerable<char> text = input.ToCharArray();
            var packet = Parse(ref text);

            Console.WriteLine($"Day16A: result = {packet.GetWithDescendents().Sum(s => s.Version)}");
        }

        public static void Day16B()
        {
            var input = string.Join("", File.ReadAllText("./Inputs/Day16Input.txt").ToCharArray().Select(c => _codes[c.ToString()]));
            IEnumerable<char> text = input.ToCharArray();
            var packet = Parse(ref text);

            Console.WriteLine($"Day16B: result = {packet.Value}");
        }

        private static SubPacket Parse(ref IEnumerable<char> text)
        {
            var version = Convert.ToInt32(ReadFromText(ref text, 3), 2);
            var packet = new SubPacket(version);
            
            var typeId = Convert.ToInt32(ReadFromText(ref text, 3), 2);

            if (typeId == 4) // literal
            {
                packet.Value = ParseLiteral(ref text);
            }
            else
            {
                var subPackets = GetPackets(ref text);

                switch (typeId)
                {
                    case 0:
                        packet.Value = subPackets.Sum(p => p.Value);
                        break;
                    case 1:
                        packet.Value = subPackets.Select(p => p.Value).Aggregate((a, b) => a * b);
                        break;
                    case 2:
                        packet.Value = subPackets.Min(p => p.Value);
                        break;
                    case 3:
                        packet.Value = subPackets.Max(p => p.Value);
                        break;
                    case 5:
                        packet.Value = subPackets.First().Value > subPackets.Last().Value ? 1 : 0;
                        break;
                    case 6:
                        packet.Value = subPackets.First().Value < subPackets.Last().Value ? 1 : 0;
                        break;
                    case 7:
                        packet.Value = subPackets.First().Value == subPackets.Last().Value ? 1 : 0;
                        break;
                }

                packet.SubPackets.AddRange(subPackets);
            }

            return packet;
        }

        private static IEnumerable<SubPacket> GetPackets(ref IEnumerable<char> text)
        {
            var lengthTypeId = int.Parse(ReadFromText(ref text, 1));
            var lengthIndicatorCharacters = lengthTypeId == 0 ? 15 : 11;
            var length = Convert.ToInt32(ReadFromText(ref text, lengthIndicatorCharacters), 2);
            var subPackets = new List<SubPacket>();

            if (lengthTypeId == 0)
            {
                IEnumerable<char> textToConsider = ReadFromText(ref text, length).ToCharArray();

                while (textToConsider.Count() > 0)
                {
                    subPackets.Add(Parse(ref textToConsider));
                }
            }
            else if (lengthTypeId == 1)
            {
                var count = 0;
                while (count < length)
                {
                    subPackets.Add(Parse(ref text));
                    count++;
                }
            }

            return subPackets;
        }

        private static long ParseLiteral(ref IEnumerable<char> text)
        {
            var hasReadFinalGroup = false;
            var result = string.Empty;

            while (!hasReadFinalGroup)
            {
                if (ReadFromText(ref text, 1) == "0") hasReadFinalGroup = true;

                result += ReadFromText(ref text, 4);
            }

            return Convert.ToInt64(result, 2);
        }

        private static string ReadFromText(ref IEnumerable<char> text, int numberOfCharacters)
        {
            var result = text.Take(numberOfCharacters);
            text = text.Skip(numberOfCharacters);
            return string.Join("", result);
        }

        private class SubPacket
        {
            public List<SubPacket> SubPackets = new List<SubPacket>();

            public SubPacket(int version)
            {
                Version = version;
            }

            public readonly int Version;
            public long Value;

            public IEnumerable<SubPacket> GetWithDescendents() => new[] { this }.Concat(SubPackets.SelectMany(s => s.GetWithDescendents()));
        }
    }
}

