using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime;

namespace AdventOfCode2021.Week1
{
    static class Day8
    {
        public static void Day8A()
        {
            var outputs = File.ReadAllLines("./Inputs/Day8Input.txt").Select(line => line.Split("|")[1]).SelectMany(output => output.Split(" "));
            var uniqueLengths = new HashSet<int>(new[] { 2, 3, 4, 7 });

            var result = outputs.Count(o => uniqueLengths.Contains(o.Length));
            Console.WriteLine($"Day8A: result = {result}");
        }

        public static void Day8B()
        {
            var outputs = File.ReadAllLines("./Inputs/Day8Input.txt").ToArray();

            var result = outputs.Sum(o => GetOutput(o));
            Console.WriteLine($"Day8B: result = {result}");
        }

        private static int GetOutput(string line)
        {
            var inputs = GetCodes(line.Split("|")[0]);
            var knownValues = GetKnownValues(inputs);

            var outputs = GetCodes(line.Split("|")[1]);
            var result = new int[4];

            for (int outputPosition = 0; outputPosition < 4; outputPosition++)
            {
                var output = outputs[outputPosition];
                var match = knownValues.First(v => v.Length == output.Length && ContainsAllCharacters(v, output));
                
                result[outputPosition] = Array.IndexOf(knownValues, match);
            }

            return int.Parse(string.Join("", result));
        }

        private static string[] GetCodes(string line)
        {
            return line.Trim().Split(" ").ToArray();
        }

        private static string[] GetKnownValues(string[] inputs)
        {
            var knownValues = new string[10];

            knownValues[1] = inputs.First(input => input.Length == 2);
            knownValues[4] = inputs.First(input => input.Length == 4);
            knownValues[7] = inputs.First(input => input.Length == 3);
            knownValues[8] = inputs.First(input => input.Length == 7);

            //  0 -> abcefg -> remove 1 (cf) -> abeg 
            //* 1 -> cf -> remove 1 (cf) -> 
            //  2 -> acdeg -> remove 1 (cf) -> adeg 
            //* 3 -> acdfg -> remove 1 (cf) -> adg 
            //* 4 -> bcdf -> remove 1 (cf) -> bd 
            //  5 -> abdfg -> remove 1 (cf) -> abdg            
            //* 6 -> abdefg -> remove 1 (cf) -> abdeg 
            //* 7 -> acf -> remove 1 (cf) -> a 
            //* 8 -> abcdefg -> remove 1 (cf) -> abdeg 
            //  9 -> abcdfg -> remove 1 (cf) -> abdg 

            var charactersOf1 = knownValues[1].ToCharArray();
            var remainingToCalc = inputs.Except(knownValues).ToArray(); // 0, 2, 3, 5, 6, 9

            knownValues[3] = remainingToCalc.First(input => input.ToCharArray().Except(charactersOf1).Count() == 3);
            knownValues[6] = remainingToCalc.First(input => input.ToCharArray().Except(charactersOf1).Count() == 5);

            remainingToCalc = inputs.Except(knownValues).ToArray(); // 0, 2, 5, 9

            knownValues[5] = remainingToCalc.First(input => input.Length == 5 && remainingToCalc.Any(other => other.Length == 6 && ContainsAllCharacters(other, input)));
            knownValues[9] = remainingToCalc.First(input => input.Length == 6 && ContainsAllCharacters(input, knownValues[5]));
            
            remainingToCalc = inputs.Except(knownValues).ToArray(); // 0, 2

            knownValues[0] = remainingToCalc.First(input => input.Length == 6);
            knownValues[2] = remainingToCalc.First(input => input.Length == 5);

            return knownValues;
        }

        private static bool ContainsAllCharacters(string longer, string shorter)
        {
            var longerCharacters = longer.ToCharArray();
            return shorter.ToCharArray().All(c => longerCharacters.Contains(c));
        }
    }
}
