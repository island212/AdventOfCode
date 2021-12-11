using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        enum Direction { Forward, Down, Up }

        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input.txt"));
            (Direction, int)[] commands = new (Direction, int)[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                Direction direction;
                string[] split = input[i].Split(" ");
                switch (split[0])
                {
                    case "forward":
                        direction = Direction.Forward;
                        break;
                    case "down":
                        direction = Direction.Down;
                        break;
                    case "up":
                        direction = Direction.Up;
                        break;
                    default:
                        throw new System.NotImplementedException(split[0]);
                }

                commands[i] = (direction, Int32.Parse(split[1]));
            }

            Console.WriteLine("########## Day 2 2021 ##########");
            Console.WriteLine($"Part one solution: {SolvePartOne(commands)}");
            Console.WriteLine($"Part two solution: {SolvePartTwo(commands)}");
            Console.WriteLine("################################");
        }

        static (int, int, int) SolvePartOne((Direction, int)[] commands)
        {
            int depth = 0;
            int position = 0;

            for (int i = 0; i < commands.Length; i++)
            {
                switch (commands[i].Item1)
                {
                    case Direction.Forward:
                        position += commands[i].Item2;
                        break;
                    case Direction.Down:
                        depth += commands[i].Item2;
                        break;
                    case Direction.Up:
                        depth -= commands[i].Item2;
                        break;
                }
            }

            return (position, depth, depth * position);
        }

        static (int, int, int) SolvePartTwo((Direction, int)[] commands)
        {
            int depth = 0;
            int position = 0;
            int aim = 0;

            for (int i = 0; i < commands.Length; i++)
            {
                switch (commands[i].Item1)
                {
                    case Direction.Forward:
                        position += commands[i].Item2;
                        depth += aim * commands[i].Item2;
                        break;
                    case Direction.Down:
                        aim += commands[i].Item2;
                        break;
                    case Direction.Up:
                        aim -= commands[i].Item2;
                        break;
                }
            }

            return (position, depth, depth * position);
        }
    }
}
