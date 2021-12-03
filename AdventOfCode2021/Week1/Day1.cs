using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;

namespace AdventOfCode2021.Week1
{
    static class Day1
    {
        internal static void Day1A()
        {
            var input = File.ReadAllLines("./Inputs/Day1Input.txt").Select(i => int.Parse(i)).ToArray();
            var count = 0;
            var previousValue = int.MaxValue;

            for (int i = 0; i < input.Length; i++)
            {
                var currentValue = input[i];

                if (currentValue > previousValue) count++;

                previousValue = currentValue;
            }

            Console.WriteLine($"Day1A: {count}");
        }

        internal static void Day1B()
        {
            var input = File.ReadAllLines("./Inputs/Day1Input.txt").Select(i => int.Parse(i)).ToArray();
            var count = 0;
            var previousValue = int.MaxValue;

            for (int i = 0; i < input.Length - 2; i++)
            {
                var currentValue = input[i] + input[i + 1] + input[i + 2];

                if (currentValue > previousValue) count++;

                previousValue = currentValue;
            }

            Console.WriteLine($"Day1B: {count}");
        }
    }
}
