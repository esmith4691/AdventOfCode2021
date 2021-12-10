using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime;

namespace AdventOfCode2021.Week1
{
    static class Day10
    {
        public static void Day10A()
        {
            var lines = File.ReadAllLines("./Inputs/Day10Input.txt").ToArray();
            var result = lines.Select(line => FindFirstIllegalCharacter(line)).Where(l => l != null).Sum(c =>
            {
                if (c.Value == ')') return 3;
                if (c.Value == ']') return 57;
                if (c.Value == '}') return 1197;
                if (c.Value == '>') return 25137;
                return 0;
            });
            Console.WriteLine($"Day10A: result = {result}");
        }

        public static void Day10B()
        {
            var lines = File.ReadAllLines("./Inputs/Day10Input.txt");
            var repairs = lines.Where(line => FindFirstIllegalCharacter(line) == null).Select(line => FindRepair(line)).ToArray();
            var scores = repairs.Select(repair =>
            {
                long score = 0;
                foreach (var character in repair)
                {
                    score *= 5;

                    if (character == ')') score += 1;
                    if (character == ']') score += 2;
                    if (character == '}') score += 3;
                    if (character == '>') score += 4;
                }
                return score;
            }).OrderBy(score => score).ToArray();

            var result = scores[scores.Length / 2];
            Console.WriteLine($"Day10B: result = {result}");
        }

        private static string FindRepair(string line)
        {
            var firstCharacters = new Dictionary<char, char>() {
                { '(', ')' },
                { '[', ']' },
                { '{', '}' },
                { '<', '>' }
            };

            var characters = new Stack<char>();
            var repair = string.Empty;

            foreach (var character in line)
            {
                if (firstCharacters.ContainsKey(character))
                    characters.Push(character);
                else
                {
                    characters.Pop();
                }
            }

            while (characters.Count > 0)
            {
                var endOfPair = firstCharacters[characters.Pop()];
                repair += endOfPair;
            }

            return repair;
        }

        private static char? FindFirstIllegalCharacter(string line)
        {
            var firstCharacters = new Dictionary<char, char>() {
                { '(', ')' },
                { '[', ']' },
                { '{', '}' },
                { '<', '>' }
            };

            var characters = new Stack<char>();

            foreach (var character in line)
            {
                if (firstCharacters.ContainsKey(character)) 
                    characters.Push(character);
                else
                {
                    if (characters.Count == 0) return character;

                    var shouldBe = firstCharacters[characters.Pop()];
                    if (character != shouldBe) return character;
                }
            }


            return null;
        }
    }
}

