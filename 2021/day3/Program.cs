using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] reports = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input.txt"));

            Console.WriteLine("########## Day 3 2021 ##########");
            Console.WriteLine($"Part one solution: {SolvePartOne(reports)}");
            Console.WriteLine($"Part two solution: {SolvePartTwo(reports)}");
            Console.WriteLine("################################");
        }

        static (int, int, int) SolvePartOne(string[] commands)
        {
            int bitsLength = commands[0].Length;

            var mostCommon = new int[bitsLength];
            for (int i = 0; i < commands.Length; i++)
            {
                for (int j = 0; j < bitsLength; j++)
                {
                    mostCommon[j] += commands[i][j] == '1' ? 1 : 0;
                }
            }

            int gamma = 0;
            for (int i = 0; i < bitsLength; i++)
            {
                gamma |= mostCommon[i] > commands.Length - mostCommon[i] ? 1 << bitsLength - i - 1 : 0;
            }

            int epsilon = ~gamma & ~(int.MaxValue << bitsLength);

            return (gamma, epsilon, gamma * epsilon);
        }

        static (int, int, int) SolvePartTwo(string[] commands)
        {
            var temps = new List<string>();
            var valids = new List<string>(commands);
            int oxygen = FindRating(valids, temps, (a,b) => a >= b);

            temps.Clear();
            valids.Clear();
            valids.AddRange(commands);
            int co2 = FindRating(valids, temps, (a, b) => a < b);

            return (oxygen, co2, oxygen * co2);
        }

        static int FindRating(List<string> valids, List<string> temps, Func<int,int, bool> compare)
        {
            int bitsLength = valids[0].Length;

            var pattern = "";
            var common = new int[bitsLength];

            for (int j = 0; j < bitsLength && valids.Count > 1; j++)
            {
                foreach (var command in valids)
                {
                    common[j] += command[j] == '1' ? 1 : 0;
                }

                pattern += compare.Invoke(common[j], valids.Count - common[j]) ? "1" : "0";

                foreach (var command in valids)
                {
                    if (command.StartsWith(pattern))
                    {
                        temps.Add(command);
                    }
                }

                var swap = valids;
                valids = temps;
                temps = swap;
                temps.Clear();
            }

            int rating = 0;
            for (int i = 0; i < bitsLength; i++)
            {
                rating |= valids[0][i] == '1' ? 1 << bitsLength - i - 1 : 0;
            }

            return rating;
        }
    }
}
