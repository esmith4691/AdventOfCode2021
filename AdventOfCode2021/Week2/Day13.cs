using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime;

namespace AdventOfCode2021.Week1
{
    static class Day13
    {
        public static void Day13A()
        {
            var lines = File.ReadAllLines("./Inputs/Day13Input.txt").ToArray();
            var (grid, instructions) = Parse(lines);
            grid = Fold(grid, instructions[0]);

            var result = Enumerable.Range(0, grid.GetLength(0)).Sum(x => Enumerable.Range(0, grid.GetLength(1)).Sum(y => grid[x, y]));
            Console.WriteLine($"Day13A: result = {result}");
        }

        public static void Day13B()
        {
            var lines = File.ReadAllLines("./Inputs/Day13Input.txt").ToArray();
            
            var (grid, instructions) = Parse(lines);

            foreach (var instruction in instructions)
            {
                grid = Fold(grid, instruction);
            }

            var builder = new StringBuilder();
            builder.AppendLine("Day13B: result = ");

            foreach (var y in Enumerable.Range(0, grid.GetLength(1)))
            {
                builder.AppendLine(string.Join("", Enumerable.Range(0, grid.GetLength(0)).Select(x => grid[x, y])));
            }

            Console.WriteLine($"{builder.ToString()}");
        }

        private static (int[,] grid, (string axis, int position)[] instructions) Parse(string[] lines)
        {
            var processingFolds = false;
            var pointCoordinates = new List<(int x, int y)>();
            var foldInstructions = new List<(string axis, int position)>();

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    processingFolds = true;
                    continue;
                }

                if (processingFolds)
                {
                    var instruction = line.Substring("fold along ".Length).Split("=");
                    foldInstructions.Add((instruction.First(), int.Parse(instruction.Last())));
                }
                else
                {
                    var instruction = line.Split(",");
                    pointCoordinates.Add((int.Parse(instruction.First()), int.Parse(instruction.Last())));
                }
            }

            var grid = new int[pointCoordinates.Max(c => c.x) + 1, pointCoordinates.Max(c => c.y) + 1];

            foreach (var coord in pointCoordinates)
            {
                grid[coord.x, coord.y] = 1;
            }

            return (grid, foldInstructions.ToArray());
        }

        private static int[,] Fold(int[,] grid, (string axis, int position) foldInstruction)
        {
            int[,] newGrid = null;

            if (foldInstruction.axis == "y")
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                    for (int y = foldInstruction.position + 1; y < grid.GetLength(1); y++)
                    {
                        if (grid[x, y] == 1) grid[x, foldInstruction.position - (y - foldInstruction.position)] = 1;
                    }

                newGrid = new int[grid.GetLength(0), foldInstruction.position];
            }
            else if (foldInstruction.axis == "x")
            {
                for (int x = foldInstruction.position + 1; x < grid.GetLength(0); x++)
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        if (grid[x, y] == 1) grid[foldInstruction.position - (x - foldInstruction.position), y] = 1;
                    }

                newGrid = new int[foldInstruction.position, grid.GetLength(1)];
            }

            for (int x = 0; x < newGrid.GetLength(0); x++)
                for (int y = 0; y < newGrid.GetLength(1); y++)
                {
                    newGrid[x, y] = grid[x, y];
                }

            return newGrid;
        }
    }
}

