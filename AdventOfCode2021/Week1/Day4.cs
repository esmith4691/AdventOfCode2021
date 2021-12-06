using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;

namespace AdventOfCode2021.Week1
{
    public static class Day4
    {
        public static void Day4A()
        {
            var input = File.ReadAllText("./Inputs/Day4Input.txt").Split("\r\n\r\n");

            var bingoNumbers = input[0].Split(",").Select(i => int.Parse(i)).ToArray();
            var grids = input.Skip(1).Select(text => Grid.Parse(text)).ToList();
            Grid winningGrid = null;
            int winningNumber = 0;

            foreach (var number in bingoNumbers)
            {
                if (winningGrid != null)
                    break;

                foreach (var grid in grids)
                {
                    var hasWon = grid.Mark(number);

                    if (hasWon)
                    {
                        winningGrid = grid;
                        winningNumber = number;
                        break; ;
                    }
                }
            }

            var result = winningNumber * winningGrid.SumOfUnmarkedNumbers();

            Console.WriteLine($"Day4A: result = {result}");
        }

        public static void Day4B()
        {
            var input = File.ReadAllText("./Inputs/Day4Input.txt").Split("\r\n\r\n");

            var bingoNumbers = input[0].Split(",").Select(i => int.Parse(i)).ToArray();
            var grids = input.Skip(1).Select(text => Grid.Parse(text)).ToList();
            var gridsNotWon = new List<Grid>(grids);
            Grid lastGrid = null;
            int lastNumber = 0;

            foreach (var number in bingoNumbers)
            {
                var gridsToCheck = new List<Grid>(gridsNotWon);
                foreach (var grid in gridsToCheck)
                {
                    var hasWon = grid.Mark(number);

                    if (hasWon)
                    {
                        gridsNotWon.Remove(grid);
                        if (gridsNotWon.Count == 0)
                        {
                            lastGrid = grid;
                            lastNumber = number;
                            break;
                        }
                    }

                    if (gridsNotWon.Count == 0) break;
                }
            }

            Console.WriteLine($"Day4B: result = {lastNumber * lastGrid.SumOfUnmarkedNumbers()}");
        }

        public class Grid
        {
            public List<GridElement[]> Rows = new List<GridElement[]>();
            public List<GridElement[]> Columns = new List<GridElement[]>();
            public bool IsWon;

            internal static Grid Parse(string text)
            {
                var grid = new Grid();
                var rows = text.Split("\r\n");

                foreach(var row in rows)
                {
                    var elements = row.Split(" ").Where(s => !string.IsNullOrEmpty(s)).Select(e => new GridElement { Number = int.Parse(e) }).ToArray();
                    grid.Rows.Add(elements);
                }

                for (int position = 0; position < grid.Rows[0].Length; position++)
                {
                    var newColumn = grid.Rows.Select(row => row[position]).ToArray();
                    grid.Columns.Add(newColumn);
                }

                return grid;
            }

            public bool Mark(int number)
            {
                foreach(var row in Rows)
                {
                    var hasWon = MarkRow(row, number);

                    if (hasWon)
                    {
                        IsWon = true;
                        return true;
                    }
                }

                foreach (var column in Columns)
                {
                    var hasWon = CheckColumn(column);

                    if (hasWon)
                    {
                        IsWon = true;
                        return true;
                    }
                }

                return false;
            }

            public int SumOfUnmarkedNumbers()
            {
                return Rows.Sum(row => row.Where(e => !e.IsMarked).Sum(e => e.Number));
            }

            private bool MarkRow(GridElement[] row, int number)
            {
                foreach (var element in row.Where(e => !e.IsMarked))
                {
                    if (number == element.Number)
                    {
                        element.IsMarked = true;
                        continue;
                    }
                }

                return row.All(e => e.IsMarked);
            }

            private bool CheckColumn(GridElement[] column)
            {
                return column.All(e => e.IsMarked);
            }
        }

        public class GridElement
        {
            public int Number;
            public bool IsMarked;
        }
    }
}
