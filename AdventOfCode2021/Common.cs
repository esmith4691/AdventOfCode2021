using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2021
{
    public static class Common
    {
        public static int[,] CreateGrid(string[] input)
        {
            var grid = new int[input.First().Length, input.Count()];

            for (int i = 0; i < input.First().Length; i++)
                for (int j = 0; j < input.Count(); j++)
                {
                    grid[i, j] = int.Parse(input[j][i].ToString());
                }

            return grid;
        }

        public static IEnumerable<(int x, int y)> FindNeighbouringPoints(int[,] grid, (int x, int y) coordinates)
        {
            if (coordinates.y > 0) yield return (coordinates.x, coordinates.y - 1);

            if (coordinates.x > 0) yield return (coordinates.x - 1, coordinates.y);

            if (coordinates.y < grid.GetLength(1) - 1) yield return (coordinates.x, coordinates.y + 1);

            if (coordinates.x < grid.GetLength(0) - 1) yield return (coordinates.x + 1, coordinates.y);
        }

        public static void DoToAllPointsInGrid(int[,] grid, Action<(int x, int y)> action)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    action((x, y));
                }
        }
    }
}
