using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime;

namespace AdventOfCode2021.Week1
{
    static class Day9
    {
        public static void Day9A()
        {
            var lines = File.ReadAllLines("./Inputs/Day9Input.txt").ToArray();
            var grid = CreateGrid(lines);
            var lowPoints = FindLowPoints(grid);
            var result = lowPoints.Select(l => grid[l.x, l.y] + 1).Sum();
            Console.WriteLine($"Day9A: result = {result}");
        }

        public static void Day9B()
        {
            var lines = File.ReadAllLines("./Inputs/Day9Input.txt");
            var grid = CreateGrid(lines);
            var threeBiggest = FindLowPoints(grid).Select(l => FindAllPointsInBasin(grid, l)).Select(b => b.Count()).OrderByDescending(x => x).Take(3).ToArray();
            var result = threeBiggest[0] * threeBiggest[1] * threeBiggest[2];
            Console.WriteLine($"Day9B: result = {result}");
        }

        private static int[,] CreateGrid(string[] input)
        {
            var grid = new int[input.First().Length, input.Count()];

            for (int i = 0; i < input.First().Length; i++)
                for (int j = 0; j < input.Count(); j++)
                {
                    grid[i, j] = int.Parse(input[j][i].ToString());
                }

            return grid;
        }

        private static IEnumerable<(int x, int y)> FindLowPoints(int[,] grid)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    var element = grid[x, y];

                    if (FindNeighbouringPoints(grid, (x, y)).All(p => element < grid[p.x, p.y])) yield return (x, y);
                }
        }

        private static IEnumerable<(int x, int y)> FindAllPointsInBasin(int[,] grid, (int x, int y) coordinates)
        {
            var toProcess = new Queue<(int x, int y)>();
            toProcess.Enqueue(coordinates);
            var result = new HashSet<(int x, int y)>();

            while (toProcess.Any())
            {
                var item = toProcess.Dequeue();

                var inBasin = FindNeighbouringPoints(grid, item).Where(p => grid[p.x, p.y] != 9);

                foreach (var coord in inBasin.Except(result)) toProcess.Enqueue(coord);

                result.Add(item);
            }

            return result;
        }

        private static IEnumerable<(int x, int y)> FindNeighbouringPoints(int[,] grid, (int x, int y) coordinates)
        {
            if (coordinates.y > 0) yield return (coordinates.x, coordinates.y - 1);

            if (coordinates.x > 0) yield return (coordinates.x - 1, coordinates.y);

            if (coordinates.y < grid.GetLength(1) - 1) yield return (coordinates.x, coordinates.y + 1);

            if (coordinates.x < grid.GetLength(0) - 1) yield return (coordinates.x + 1, coordinates.y);
        }
    }
}
