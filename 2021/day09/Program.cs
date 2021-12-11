using System;
using System.Collections.Generic;
using System.IO;

var reports = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input.txt"));

int width = reports[0].Length;
int heigth = reports.Length;
var maps = new int[width * heigth];
for (int i = 0; i < heigth; i++)
{
    for (int j = 0; j < width; j++)
    {
        maps[i * width + j] = (int)char.GetNumericValue(reports[i][j]);
    }
}

var lowPoints = new Stack<int>();

Console.WriteLine("########## Day 9 2021 ##########");
Console.WriteLine($"Part one solution: {SolvePartOne(width, heigth, maps, lowPoints)}");
Console.WriteLine($"Part two solution: {SolvePartTwo(width, heigth, maps, lowPoints)}");
Console.WriteLine("################################");

static int SolvePartOne(int width, int heigth, int[] maps, Stack<int> lowPoints)
{
    int GetUp(int i) => i - width >= 0 ? maps[i - width] : 9;
    int GetDown(int i) => i + width < maps.Length ? maps[i + width] : 9;
    int GetLeft(int i) => i % width > 0 ? maps[i - 1] : 9;
    int GetRight(int i) => (i + 1) % width > 0 ? maps[i + 1] : 9;

    int riskLevel = 0;
    for (int i = 0; i < maps.Length; i++)
    {
        if (GetLeft(i) > maps[i] && GetRight(i) > maps[i] && GetDown(i) > maps[i] && GetUp(i) > maps[i])
        {
            riskLevel += maps[i] + 1;
            lowPoints.Push(i);
        }
    }

    return riskLevel;
}

static int SolvePartTwo(int width, int heigth, int[] maps, Stack<int> lowPoints)
{
    int GetUp(int i) => i - width >= 0 ? maps[i - width] : 9;
    int GetDown(int i) => i + width < maps.Length ? maps[i + width] : 9;
    int GetLeft(int i) => i % width > 0 ? maps[i - 1] : 9;
    int GetRight(int i) => (i + 1) % width > 0 ? maps[i + 1] : 9;

    var basinSize = new int[3];
    {
        while (lowPoints.Count > 0)
        {
            var visited = new List<int>();
            var explore = new Stack<int>();
            explore.Push(lowPoints.Pop());

            while (explore.Count > 0)
            {
                int i = explore.Pop();
                if (!visited.Contains(i))
                {
                    visited.Add(i);
                    if (GetUp(i) < 9) explore.Push(i - width);
                    if (GetDown(i) < 9) explore.Push(i + width);
                    if (GetLeft(i) < 9) explore.Push(i - 1);
                    if (GetRight(i) < 9) explore.Push(i + 1);
                }
            }

            if (basinSize[2] < visited.Count)
            {
                basinSize[0] = basinSize[1];
                basinSize[1] = basinSize[2];
                basinSize[2] = visited.Count;
            }
            else if (basinSize[1] < visited.Count)
            {
                basinSize[0] = basinSize[1];
                basinSize[1] = visited.Count;
            }
            else if (basinSize[0] < visited.Count)
            {
                basinSize[0] = visited.Count;
            }
        }
    }

    return basinSize[0] * basinSize[1] * basinSize[2];
}
