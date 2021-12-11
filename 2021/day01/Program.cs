using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input.txt"));
            int[] report = (Array.ConvertAll(input, s => Int32.Parse(s)));

            Console.WriteLine("########## Day 1 2021 ##########");
            Console.WriteLine($"Part one solution: {SolvePartOne(report)}");
            Console.WriteLine($"Part two solution: {SolvePartTwo(report)}");
            Console.WriteLine("################################");
        }

        static int SolvePartOne(int[] report)
        {
            int counter = 0;

            int last = report[0];
            for (int i = 1; i < report.Length; i++)
            {
                int current = report[i];
                if (current > last)
                    counter++;
                last = current;
            }
            return counter;
        }

        static int SolvePartTwo(int[] report)
        {
            int counter = 0;

            int last = report[0] + report[1] + report[2];
            for (int i = 1; i < report.Length - 2; i++)
            {
                int current = report[i] + report[i + 1] + report[i + 2];
                if (current > last)
                    counter++;
                last = current;
            }
            return counter;
        }
    }
}
