using AdventOfCode2021.Week1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;

namespace AdventOfCode2021.Week1
{
    static class Day2
    {
        internal static void Day2A()
        {
            var input = File.ReadAllLines("./Inputs/Day2Input.txt").ToArray();
            var forward = 0;
            var depth = 0;

            foreach (var item in input)
            {
                var command = Enum.Parse<Direction>(item.Substring(0, item.IndexOf(' ')));
                var distancePosition = command.ToString().Length + " ".Length;
                var distance = int.Parse(item.Substring(distancePosition, item.Length - distancePosition));

                switch (command)
                {
                    case Direction.forward:
                        forward += distance;
                        break;
                    case Direction.down:
                        depth += distance;
                        break;
                    case Direction.up:
                        depth -= distance;
                        break;
                }
            }

            Console.WriteLine($"Day2A: forward = {forward}, depth = {depth}, result = {forward * depth}");
        }

        internal static void Day2B()
        {
            var input = File.ReadAllLines("./Inputs/Day2Input.txt").ToArray();
            var forward = 0;
            var depth = 0;
            var aim = 0;

            foreach (var item in input)
            {
                var command = Enum.Parse<Direction>(item.Substring(0, item.IndexOf(' ')));
                var distancePosition = command.ToString().Length + " ".Length;
                var distance = int.Parse(item.Substring(distancePosition, item.Length - distancePosition));

                switch (command)
                {
                    case Direction.forward:
                        forward += distance;
                        depth += distance * aim;
                        break;
                    case Direction.down:
                        aim += distance;
                        break;
                    case Direction.up:
                        aim -= distance;
                        break;
                }
            }

            Console.WriteLine($"Day2B: forward = {forward}, depth = {depth}, aim = {aim}, result = {forward * depth}");
        }

        enum Direction
        {
            forward,
            down,
            up
        }

    }
}
