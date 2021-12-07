using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Week1
{
    static class Day7
    {
        private static Dictionary<int, int> _fuelMap = new Dictionary<int, int>();

        public static void Day7A()
        {
            var positions = File.ReadAllText("./Inputs/Day7Input.txt").Split(",").Select(i => int.Parse(i)).ToArray();
            
            var min = positions.Min();
            var max = positions.Max();
            var lowestFuel = int.MaxValue;

            for (int i = min; i <= max; i++)
            {
                var fuelToMove = positions.Sum(p => Math.Abs(p - i));
                if (fuelToMove < lowestFuel) lowestFuel = fuelToMove;
            }

            Console.WriteLine($"Day7A: result = {lowestFuel}");
        }

        public static void Day7B()
        {
            var positions = File.ReadAllText("./Inputs/Day7Input.txt").Split(",").Select(i => int.Parse(i)).ToArray();

            var min = positions.Min();
            var max = positions.Max();
            var lowestFuel = int.MaxValue;

            for (int i = min; i <= max; i++)
            {
                var fuelToMove = positions.Sum(p => GetFuelCost(Math.Abs(p - i)));
                if (fuelToMove < lowestFuel) lowestFuel = fuelToMove;
            }

            Console.WriteLine($"Day7A: result = {lowestFuel}");
        }

        private static int GetFuelCost(int distance)
        {
            if (_fuelMap.ContainsKey(distance)) return _fuelMap[distance];

            var result = distance > 0 ? GetFuelCost(distance - 1) + distance : 0;

            _fuelMap[distance] = result;
            return result;
        }
    }
}
