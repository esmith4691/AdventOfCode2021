using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime;

namespace AdventOfCode2021.Week1
{
    static class Day11
    {
        static int NumberOfFlashes = 0;

        public static void Day11A()
        {
            var lines = File.ReadAllLines("./Inputs/Day11Input.txt").ToArray();
            var grid = CreateGrid(lines);

            for (int i = 1; i <= 100; i++)
            {
                DoSingleStep(grid);
            }

            Console.WriteLine($"Day11A: result = {NumberOfFlashes}");
            NumberOfFlashes = 0;
        }

        public static void Day11B()
        {
            var lines = File.ReadAllLines("./Inputs/Day11Input.txt").ToArray();
            var grid = CreateGrid(lines);
            var step = 1;
            while(true)
            {
                if (DoSingleStep(grid)) break;

                step++;
            }

            Console.WriteLine($"Day11B: result = {step}");
        }

        private static bool DoSingleStep(Octopus[,] grid)
        {
            var pointsInGrid = Enumerable.Range(0, grid.GetLength(0)).SelectMany(x => Enumerable.Range(0, grid.GetLength(1)).Select(y => (x, y)));
            ProcessOctopiAtPoints(grid, pointsInGrid);
            var isSimultaneousFlash = pointsInGrid.All(p => grid[p.x, p.y].HasFlashed);
            ResetGrid(grid);
            return isSimultaneousFlash;
        }

        private static void ProcessOctopiAtPoints(Octopus[,] grid, IEnumerable<(int x, int y)> points)
        {
            foreach (var point in points)
            {
                var octopusAtPoint = grid[point.x, point.y];
                octopusAtPoint.Score++;
                if (octopusAtPoint.Score > 9) Flash(grid, point, octopusAtPoint);
            }
        }

        private static void ResetGrid(Octopus[,] grid)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    var octopus = grid[x, y];

                    if (octopus.HasFlashed)
                    {
                        octopus.HasFlashed = false;
                        octopus.Score = 0;
                    }
                }
        }

        private static void Flash(Octopus[,] grid, (int x, int y) element, Octopus octopus)
        {
            if (octopus.HasFlashed) return;

            NumberOfFlashes++;
            octopus.HasFlashed = true;

            var neighbouringPoints = FindNeighbouringPoints(grid, element);
            ProcessOctopiAtPoints(grid, neighbouringPoints);
        }

        private static Octopus[,] CreateGrid(string[] input)
        {
            var grid = new Octopus[input.First().Length, input.Count()];

            for (int i = 0; i < input.First().Length; i++)
                for (int j = 0; j < input.Count(); j++)
                {
                    grid[i, j] = new Octopus { Score = int.Parse(input[j][i].ToString()) };
                }

            return grid;
        }

        private static IEnumerable<(int x, int y)> FindNeighbouringPoints(Octopus[,] grid, (int x, int y) coordinates)
        {
            if (coordinates.y > 0) yield return (coordinates.x, coordinates.y - 1); // above

            if (coordinates.x > 0) yield return (coordinates.x - 1, coordinates.y); // left

            if (coordinates.y > 0 && coordinates.x > 0) yield return (coordinates.x - 1, coordinates.y - 1); // above left

            if (coordinates.y < grid.GetLength(1) - 1) yield return (coordinates.x, coordinates.y + 1); // below

            if (coordinates.y < grid.GetLength(1) - 1 && coordinates.x > 0) yield return (coordinates.x - 1, coordinates.y + 1); // below left

            if (coordinates.x < grid.GetLength(0) - 1) yield return (coordinates.x + 1, coordinates.y); // right

            if (coordinates.y > 0 && coordinates.x < grid.GetLength(0) - 1) yield return (coordinates.x + 1, coordinates.y - 1); // above right

            if (coordinates.y < grid.GetLength(1) - 1 && coordinates.x < grid.GetLength(0) - 1) yield return (coordinates.x + 1, coordinates.y + 1); // below right


        }

        private class Octopus
        {
            public int Score;
            public bool HasFlashed;
        }
    }
}

