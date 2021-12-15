using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime;

namespace AdventOfCode2021.Week1
{
    static class Day12
    {
        static Dictionary<string, List<string>> _connections;

        public static void Day12A()
        {
            var lines = File.ReadAllLines("./Inputs/Day12Input.txt").ToArray();
            _connections = ParseAllConnections(lines);
            var routes = FindRoutesFromPoint(new[] { "start" }, false);

            var result = routes.Count(r => r.Any(c => c != "start" && c != "end" && c.ToLower() == c));
            Console.WriteLine($"Day12A: result = {result}");
        }

        public static void Day12B()
        {
            var lines = File.ReadAllLines("./Inputs/Day12Input.txt").ToArray();
            _connections = ParseAllConnections(lines);
            var routes = FindRoutesFromPoint(new[] { "start" }, true);

            Console.WriteLine($"Day12B: result = {routes.Count()}");
        }

        private static bool VisitsAtMostOneSmallCaveTwice(string[] route)
        {
            var smallCaves = route.Where(r => r != "start" && r != "end" && r.ToLower() == r);
            return smallCaves.GroupBy(s => s).Count(s => s.Count() > 1) <= 1;
        }

        private static Dictionary<string, List<string>> ParseAllConnections(IEnumerable<string> connections)
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var connection in connections)
            {
                var start = connection.Split("-").First();
                var end = connection.Split("-").Last();

                if (result.ContainsKey(start))
                {
                    result[start].Add(end);
                }
                else
                {
                    result.Add(start, new List<string> { end });
                }

                if (result.ContainsKey(end))
                {
                    result[end].Add(start);
                }
                else
                {
                    result.Add(end, new List<string> { start });
                }
            }

            return result;
        }

        private static IEnumerable<string[]> FindRoutesFromPoint(string[] routeSoFar, bool allowDoubleVisitToSingleSmallCave)
        {
            var routes = new List<string[]>();
            var toProcess = new Queue<string>(FindConnectionsFromPoint(routeSoFar, allowDoubleVisitToSingleSmallCave));

            while (toProcess.Any())
            {
                var point = toProcess.Dequeue();
                var routeIncludingThisPoint = routeSoFar.Concat(new[] { point }).ToArray();

                if (point == "end")
                {
                    routes.Add(routeIncludingThisPoint);
                    continue;
                }

                var routesFromPoint = FindRoutesFromPoint(routeIncludingThisPoint, allowDoubleVisitToSingleSmallCave);
                routes.AddRange(routesFromPoint);
            }

            return routes;
        }

        private static IEnumerable<string> FindConnectionsFromPoint(string[] routeSoFar, bool allowDoubleVisitToSingleSmallCave)
        {
            var point = routeSoFar.Last();

            var connections = _connections.TryGetValue(point, out var result) ? result : Enumerable.Empty<string>();

            return connections.Where(c => 
            {
                if (c == "start") return false;

                if (c.ToLower() != c) return true;

                if (!allowDoubleVisitToSingleSmallCave && !routeSoFar.Contains(c)) return true;

                if (allowDoubleVisitToSingleSmallCave) 
                {
                    var alreadyVisitedASmallCaveTwice = routeSoFar.Where(r => r != "start" && r != "end" && r.ToLower() == r).GroupBy(r => r).Any(r => r.Count() > 1);

                    if (!alreadyVisitedASmallCaveTwice && routeSoFar.Count(r => r == c) < 2) return true;

                    if (alreadyVisitedASmallCaveTwice && routeSoFar.Count(r => r == c) < 1) return true;
                }

                return false;
            });
        }
    }
}

