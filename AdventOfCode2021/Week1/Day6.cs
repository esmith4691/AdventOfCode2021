using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode2021.Week1
{
    public static class Day6
    {
        static Dictionary<int, int[]> ChildrenMap = new Dictionary<int, int[]>();
        static Dictionary<int, long> InitialFishDescendentsMap = new Dictionary<int, long>();
        static Dictionary<int, long> AllDescendentsMap = new Dictionary<int, long>();

        public static void Day6A()
        {
            var input = File.ReadAllText("./Inputs/Day6Input.txt");
            var allFish = input.Split(",").Select(number => int.Parse(number)).ToList();

            Console.WriteLine($"Day6A: 80 days = {PassDays(allFish, 80)}");
        }

        private static long PassDays(List<int> initialFish, int daysToPass)
        {
            long totalFish = 0;
            InitialFishDescendentsMap.Clear();

            foreach (var fish in initialFish)
            {
                if (InitialFishDescendentsMap.ContainsKey(fish))
                {
                    totalFish += InitialFishDescendentsMap[fish] + 1;
                    continue;
                }

                var children = GetChildrenWithDaysRemaining(daysToPass, fish).ToList();
                long numberOfDescendents = children.Sum(daysRemaining => GetAllDescendents(daysRemaining) + 1);

                InitialFishDescendentsMap[fish] = numberOfDescendents;
                totalFish += numberOfDescendents + 1;
            }

            return totalFish;
        }

        public static void Day6B()
        {
            var input = File.ReadAllText("./Inputs/Day6Input.txt");
            var allFish = input.Split(",").Select(number => int.Parse(number)).ToList();
            
            Console.WriteLine($"Day6B: 256 days = {PassDays(allFish, 256)}");
        }

        private static IEnumerable<int> GetChildrenWithDaysRemainingAtBirth(int daysRemaining)
        {
            if (ChildrenMap.ContainsKey(daysRemaining)) return ChildrenMap[daysRemaining];
            var result = GetChildrenWithDaysRemaining(daysRemaining, 8);

            ChildrenMap[daysRemaining] = result.ToArray();
            return result;
        }

        private static IEnumerable<int> GetChildrenWithDaysRemaining(int daysRemaining, int daysToSpawn)
        {
            while(daysRemaining > daysToSpawn)
            {
                var dayOfBirth = daysRemaining - daysToSpawn - 1;
                yield return dayOfBirth;

                daysRemaining = daysRemaining - daysToSpawn;
                daysToSpawn = 7;
            }
        }

        private static long GetAllDescendents(int daysRemaining)
        {
            if (AllDescendentsMap.ContainsKey(daysRemaining)) return AllDescendentsMap[daysRemaining];
            
            var children = GetChildrenWithDaysRemainingAtBirth(daysRemaining);
            var result = children.Sum(c => GetAllDescendents(c) + 1);
            AllDescendentsMap[daysRemaining] = result;
            return result;
        }
    }
}
