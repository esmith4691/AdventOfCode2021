using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime;

namespace AdventOfCode2021.Week1
{
    static class Day15
    {
        static Dictionary<(int x, int y), int> _distanceToPoint = new Dictionary<(int x, int y), int>();
        static HashSet<(int x, int y)> _unvisitedPoints = new HashSet<(int x, int y)>();
        static HashSet<(int x, int y)> _paritiallyVisitedPoints = new HashSet<(int x, int y)>();
        static int[,] _grid;
        static (int x, int y) _end;

        public static void Day15A()
        {
            var lines = File.ReadAllLines("./Inputs/Day15Input.txt").ToArray();
            _grid = Common.CreateGrid(lines);
            _end = (_grid.GetLength(0) - 1, _grid.GetLength(1) - 1);

            FindShortestPath();

            Console.WriteLine($"Day15A: result = {_distanceToPoint[_end]}");
        }

        public static void Day15B()
        {
            _grid = CreateRepeatedGrid();
            _end = (_grid.GetLength(0) - 1, _grid.GetLength(1) - 1);

            FindShortestPath();

            Console.WriteLine($"Day15B: result = {_distanceToPoint[_end]}");
        }

        private static int[,] CreateRepeatedGrid()
        {
            var length = _grid.GetLength(0);
            var newGrid = new int[length * 5, length * 5];
            
            for (int x = 0; x < 5; x++)
                for (int y = 0; y < 5; y++)
                {
                    Common.DoToAllPointsInGrid(_grid, p =>
                    {
                        var initialValue = _grid[p.x, p.y];
                        var newValue = initialValue + x + y;
                        newGrid[p.x + x * length, p.y + y * length] = newValue > 9 ? newValue - 9 : newValue;
                    });
                }

            return newGrid;
        }

        private static void FindShortestPath()
        {
            Common.DoToAllPointsInGrid(_grid, p =>
            {
                _unvisitedPoints.Add(p);
                _distanceToPoint[p] = int.MaxValue;
            });

            UpdateDistance((0, 0), 0);
            CheckAllDistances();
        }


        private static void CheckAllDistances()
        {
            while (_unvisitedPoints.Any())
            {
                var point = _paritiallyVisitedPoints.OrderBy(p => _distanceToPoint[p]).First();
                UpdateAllNeighbouringDistances(point);

                _unvisitedPoints.Remove(point);
                _paritiallyVisitedPoints.Remove(point);

                if (point == _end) break;
            }
        }

        private static void UpdateAllNeighbouringDistances((int x, int y) coordinate)
        {
            var points = Common.FindNeighbouringPoints(_grid, coordinate);
            var distanceToCoord = _distanceToPoint[coordinate];

            foreach (var point in points.Where(p => _unvisitedPoints.Contains(p)))
            {
                var distance = distanceToCoord + _grid[point.x, point.y];
                UpdateDistance(point, distance);
            }
        }

        private static void UpdateDistance((int x, int y) coordinate, int distance)
        {
            if (_distanceToPoint[coordinate] > distance)
            {
                _paritiallyVisitedPoints.Add(coordinate);
                _distanceToPoint[coordinate] = distance;
            }
        }
    }
}

