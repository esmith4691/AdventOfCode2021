using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;

namespace AdventOfCode2021.Week1
{
    static class Day3
    {
        private static double GetFromBinary(IEnumerable<int> binary)
        {
            var binaryReversed = binary.Reverse().ToArray();
            var result = 0.0;

            for (int i = 0; i < binary.Count(); i++)
            {
                result += binaryReversed[i] * Math.Pow(2, i);
            }

            return result;
        }

        internal static void Day3A()
        {
            var input = File.ReadAllLines("./Inputs/Day3Input.txt").ToArray();
            var reportLength = input.First().Length;
            
            var gammaBinary = Enumerable.Range(0, reportLength).Select(index => GetMostCommonValueAtPosition(index, input)).Select(c => int.Parse(c)).ToArray();
            var epsilonBinary = Enumerable.Range(0, reportLength).Select(index => GetLeastCommonValueAtPosition(index, input)).Select(c => int.Parse(c)).ToArray();
            
            var gammaRate = GetFromBinary(gammaBinary);
            var epsilonRate = GetFromBinary(epsilonBinary);

            Console.WriteLine($"Day3A: gamma = {gammaRate}, epsilon = {epsilonRate}, result = {gammaRate * epsilonRate}");
        }

        internal static void Day3B()
        {
            var input = File.ReadAllLines("./Inputs/Day3Input.txt").ToArray();
            var reportLength = input.First().Length; 
            
            var linesToKeep = new List<string>(input);
            var oxygenRatingBinary = string.Empty;

            for (int i = 0; i < reportLength; i++)
            {
                var mostCommonCharacter = GetMostCommonValueAtPosition(i, linesToKeep);
                linesToKeep = linesToKeep.Where(l => l[i].ToString() == mostCommonCharacter).ToList();

                if (linesToKeep.Count == 1)
                {
                    oxygenRatingBinary = linesToKeep[0];
                    continue;
                }
            }

            linesToKeep = new List<string>(input);
            var co2RatingBinary = string.Empty;

            for (int i = 0; i < reportLength; i++)
            {
                var leastCommonCharacter = GetLeastCommonValueAtPosition(i, linesToKeep);
                linesToKeep = linesToKeep.Where(l => l[i].ToString() == leastCommonCharacter).ToList();

                if (linesToKeep.Count == 1)
                {
                    co2RatingBinary = linesToKeep[0];
                    continue;
                }
            }

            var oxygenRating = GetFromBinary(oxygenRatingBinary.ToCharArray().Select(c => int.Parse(c.ToString())));
            var co2Rating = GetFromBinary(co2RatingBinary.ToCharArray().Select(c => int.Parse(c.ToString())));

            Console.WriteLine($"Day3B: oxygen = {oxygenRating}, co2 = {co2Rating}, result = {oxygenRating * co2Rating}");
        }

        private static string GetMostCommonValueAtPosition(int position, IEnumerable<string> lines)
        {
            return lines.GroupBy(l => l[position]).OrderByDescending(g => g.Count()).ThenByDescending(g => g.Key).First().Key.ToString();
        }

        private static string GetLeastCommonValueAtPosition(int position, IEnumerable<string> lines)
        {
            return lines.GroupBy(l => l[position]).OrderByDescending(g => g.Count()).ThenByDescending(g => g.Key).Last().Key.ToString();
        }
    }
}
