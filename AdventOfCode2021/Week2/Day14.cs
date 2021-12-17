using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime;

namespace AdventOfCode2021.Week1
{
    static class Day14
    {
        static Dictionary<string, char> _rules;
        static string _polymerTemplate;
        static Dictionary<char, long> _letterCount = new Dictionary<char, long>();
        static Dictionary<(int steps, string pair), Dictionary<char, long>> _letterCache = new Dictionary<(int steps, string pair), Dictionary<char, long>>();
        
        public static void Day14A()
        {
            var lines = File.ReadAllLines("./Inputs/Day14Input.txt").ToArray();
            _rules = lines.Skip(2).ToDictionary(l => l.Split(" -> ")[0], l => l.Split(" -> ")[1][0]);
            _polymerTemplate = lines[0];
            _letterCount = _polymerTemplate.ToCharArray().GroupBy(c => c).ToDictionary(c => c.Key, c => (long) c.Count());

            DoSteps(10);

            Console.WriteLine($"Day14A: result = {_letterCount.Values.Max() - _letterCount.Values.Min()}");
        }

        public static void Day14B()
        {
            _letterCount = _polymerTemplate.ToCharArray().GroupBy(c => c).ToDictionary(c => c.Key, c => (long) c.Count());
            DoSteps(40);

            Console.WriteLine($"Day14B: result = {_letterCount.Values.Max() - _letterCount.Values.Min()}");
        }

        private static void DoSteps(int numberOfSteps)
        {
            var pairs = SplitIntoPairs(_polymerTemplate);

            foreach (var pair in pairs) ApplyRules(pair, numberOfSteps);
        }

        private static IEnumerable<string> SplitIntoPairs(string segment)
        {
            return Enumerable.Range(0, segment.Length - 1)
                    .Select(r => $"{segment[r]}{segment[r + 1]}").ToArray();
        }

        private static Dictionary<char, long> ApplyRules(string pair, int steps)
        {
            if (steps > 0 && _rules.TryGetValue(pair, out var rule))
            {
                Dictionary<char, long> letters = null;

                if (_letterCache.TryGetValue((steps, pair), out letters))
                {
                    MergeDictionaries(letters, _letterCount);
                }
                else
                {
                    letters = new Dictionary<char, long>();
                    letters.Add(rule, 1);
                    MergeDictionaries(ApplyRules($"{pair[0]}{rule}", steps - 1), letters);
                    MergeDictionaries(ApplyRules($"{rule}{pair[1]}", steps - 1), letters);

                    _letterCache.Add((steps, pair), letters);

                    AddLetterToCount(rule);
                }

                return letters;
            }

            return new Dictionary<char, long>();
        }

        private static void AddLetterToCount(char letter)
        {
            if (_letterCount.ContainsKey(letter))
            {
                _letterCount[letter]++;
            }
            else
            {
                _letterCount.Add(letter, 1);
            }
        }

        private static void MergeDictionaries(Dictionary<char, long> from, Dictionary<char, long> to)
        {
            foreach (var letter in from.Keys)
            {
                if (to.ContainsKey(letter))
                {
                    to[letter] += from[letter];
                }
                else
                {
                    to.Add(letter, from[letter]);
                }
            }
        }
    }
}

