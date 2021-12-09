using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var reports = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input.txt"));

int[] crabs = Array.ConvertAll(reports[0].Split(","), s => int.Parse(s));

Array.Sort(crabs);

Console.WriteLine("########## Day 7 2021 ##########");
Console.WriteLine($"Part one solution: {SolvePartOne(crabs)}");
Console.WriteLine($"Part two solution: {SolvePartTwo(crabs)}");
Console.WriteLine("################################");

static (int, int) SolvePartOne(int[] crabs)
{
    int median = crabs[crabs.Length / 2];

    int fuel = 0;
    for (int i = 0; i < crabs.Length; i++)
    {
        fuel += Math.Abs(crabs[i] - median);
    }

    return (median, fuel);
}

static (int, int) SolvePartTwo(int[] crabs)
{
    int lastIndex = crabs.Length - 1;

    int[] factorials = new int[crabs[crabs.Length - 1] + 1];
    for (int i = 1; i < factorials.Length; i++)
    {
        factorials[i] = i + factorials[i - 1];
    }

    int average = (int)Math.Round((double)crabs.Sum() / crabs.Length);
    int fuel = GetFuel(average, crabs, factorials);

    int align = average;
    int bestFuel = fuel;

    int upperAlign = average + 1;
    int upperFuel = GetFuel(upperAlign, crabs, factorials);
    {
        int lastFuel = fuel;
        while (upperFuel < lastFuel)
        {
            lastFuel = upperFuel;
            upperFuel = GetFuel(++upperAlign, crabs, factorials);
        }

        if (lastFuel < bestFuel)
        {
            bestFuel = lastFuel;
            align = upperAlign - 1;
        }
    }

    int lowerAlign = average - 1;
    int lowerFuel = GetFuel(lowerAlign, crabs, factorials);
    {
        int lastFuel = fuel;
        while (lowerFuel < lastFuel)
        {
            lastFuel = lowerFuel;
            lowerFuel = GetFuel(--lowerAlign, crabs, factorials);
        }

        if (lastFuel < bestFuel)
        {
            bestFuel = lastFuel;
            align = lowerAlign + 1;
        }
    }

    return (align, bestFuel);
}

static int GetFuel(int align, int[] crabs, int[] factorials)
{
    int fuel = 0;
    for (int i = 0; i < crabs.Length; i++)
    {
        int test = crabs[i] - align;
        fuel += factorials[Math.Abs(test)];
    }
    return fuel;
}