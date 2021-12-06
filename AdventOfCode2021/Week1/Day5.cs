using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;

namespace AdventOfCode2021.Week1
{
    public static class Day5
    {
        public static void Day5A()
        {
            var input = File.ReadAllLines("./Inputs/Day5Input.txt");
            var coordinates = input.Select(line => line.Split(" -> ")).SelectMany(pair => GetCoordinatesInPath(new Coordinate(pair[0]), new Coordinate(pair[1]), false));
            
            var result = coordinates.GroupBy(c => (c.X, c.Y)).Count(group => group.Count() >= 2);

            Console.WriteLine($"Day5A: result = {result}");
        }

        private static IEnumerable<Coordinate> GetCoordinatesInPath(Coordinate start, Coordinate end, bool includeDiagonals)
        {
            if(start.X == end.X || start.Y == end.Y)
            {
               return GetCoordinates(start, end);
            }
            else if (includeDiagonals && Math.Abs(start.X - end.X) == Math.Abs(start.Y - end.Y))
            {
                return GetCoordinates(start, end);
            }

            return Enumerable.Empty<Coordinate>();
        }

        private static IEnumerable<Coordinate> GetCoordinates(Coordinate start, Coordinate end)
        {
            var x = start.X;
            var y = start.Y;

            yield return new Coordinate($"{x},{y}");

            while(x != end.X || y != end.Y)
            {
                if (end.X > start.X)
                    x++;
                else if (end.X < start.X)
                    x--;

                if (end.Y > start.Y)
                    y++;
                else if (end.Y < start.Y)
                    y--;

                yield return new Coordinate($"{x},{y}");
            }
        }

        public static void Day5B()
        {
            var input = File.ReadAllLines("./Inputs/Day5Input.txt");
            var coordinates = input.Select(line => line.Split(" -> ")).SelectMany(pair => GetCoordinatesInPath(new Coordinate(pair[0]), new Coordinate(pair[1]), true));

            var result = coordinates.GroupBy(c => (c.X, c.Y)).Count(group => group.Count() >= 2);

            Console.WriteLine($"Day5B: result = {result}");
        }

        [DebuggerDisplay("({X}, {Y})")]
        private class Coordinate
        {
            public Coordinate(string input)
            {
                var values = input.Split(",");
                X = int.Parse(values.First());
                Y = int.Parse(values.Last());
            }
            public int X;
            public int Y;
        }
    }
}
