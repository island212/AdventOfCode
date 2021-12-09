using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

var reports = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input.txt"));

int[] fishs = Array.ConvertAll(reports[0].Split(","), s => int.Parse(s));

Console.WriteLine("########## Day 6 2021 ##########");
Console.WriteLine($"Part one solution: {SolvePartOne(fishs, 80)}");
Console.WriteLine($"Part two solution: {SolvePartOne(fishs, 256)}");
Console.WriteLine("################################");

static long SolvePartOne(int[] fishs, int days)
{
    long[] calendar = new long[9];
    for (int i = 0; i < fishs.Length; i++)
        calendar[fishs[i]]++;

    for (int i = 0; i < days; i++)
    {
        long newFish = calendar[0];
        for (int j = 1; j < 9; j++)
            calendar[j - 1] = calendar[j];

        calendar[6] += newFish; //Reset timer from the old fish
        calendar[8] = newFish; //New fish spawn
    }

    return calendar.Sum();
}