using System;
using System.IO;
using System.Linq;
using System.Runtime;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            Day1A();
            Day1B();
            Day2A();
            Day2B();
        }

        static void Day1A()
        {
            var input = File.ReadAllLines("./Day1Input.txt").Select(i => int.Parse(i)).ToArray();
            var count = 0;
            var previousValue = int.MaxValue;

            for (int i = 0; i < input.Length; i++)
            {
                var currentValue = input[i];

                if (currentValue > previousValue) count++;

                previousValue = currentValue;
            }

            Console.WriteLine($"Day1A: {count}");
        }

        static void Day1B()
        {
            var input = File.ReadAllLines("./Day1Input.txt").Select(i => int.Parse(i)).ToArray();
            var count = 0;
            var previousValue = int.MaxValue;

            for (int i = 0; i < input.Length - 2; i++)
            {
                var currentValue = input[i] + input[i + 1] + input[i + 2];

                if (currentValue > previousValue) count++;

                previousValue = currentValue;
            }

            Console.WriteLine($"Day1B: {count}");
        }

        static void Day2A()
        {
            var input = File.ReadAllLines("./Day2Input.txt").ToArray();
            var forward = 0;
            var depth = 0;

            foreach (var item in input)
            {
                var command = Enum.Parse<Direction>(item.Substring(0, item.IndexOf(' ')));
                var distancePosition = command.ToString().Length + " ".Length;
                var distance = int.Parse(item.Substring(distancePosition, item.Length - distancePosition));

                switch(command)
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

        static void Day2B()
        {
            var input = File.ReadAllLines("./Day2Input.txt").ToArray();
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
