using System;
using System.Collections.Generic;
using System.IO;

var reports = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input.txt"));

int width = reports[0].Length;
int height = reports.Length;
int[] map = new int[width * height];
for (int i = 0; i < reports.Length; i++)
{
    for (int j = 0; j < reports[i].Length; j++)
    {
        map[i * width + j] = (int)char.GetNumericValue(reports[i][j]);
    }
}

int[] copyMap = new int[width * height];
Array.Copy(map, copyMap, map.Length);

Console.WriteLine("########## Day 9 2021 ##########");
Console.WriteLine($"Part one solution: {SolvePartOne(width, map)}");
Console.WriteLine($"Part two solution: {SolvePartTwo(width, copyMap)}");
Console.WriteLine("################################");

static int SolvePartOne(int width, int[] map)
{
    int totalFlashCount = 0;
    for (int step = 0; step < 100; step++)
    {
        for (int i = 0; i < map.Length; i++)
            map[i]++;

        totalFlashCount += ProcessExplosion(width, map);

        for (int i = 0; i < map.Length; i++)
            map[i] = map[i] == -1 ? 0 : map[i];
    }

    return totalFlashCount;
}

static long SolvePartTwo(int width, int[] map)
{
    int step = 0;
    int flashCount;
    do
    {
        flashCount = 0;
        step++;

        for (int i = 0; i < map.Length; i++)
            map[i]++;

        flashCount = ProcessExplosion(width, map);

        for (int i = 0; i < map.Length; i++)
            if (map[i] == -1)
                map[i] = 0;
    } 
    while (flashCount != map.Length);

    return step;
}

static int ProcessExplosion(int width, int[] map)
{
    int totalFlashCount = 0;
    int flashCount;
    do
    {
        flashCount = 0;
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] > 9)
            {
                flashCount++;

                bool hasLeft = i % width > 0;
                bool hasRight = (i + 1) % width > 0;
                bool hasUp = i >= width;
                bool hasDown = i < map.Length - width;

                if (hasUp)
                {
                    if (hasLeft && map[i - width - 1] >= 0) map[i - width - 1]++;   //TopLeft
                    if (map[i - width] >= 0) map[i - width]++;                      //TopCenter
                    if (hasRight && map[i - width + 1] >= 0) map[i - width + 1]++;  //TopRight
                }

                if (hasLeft && map[i - 1] >= 0) map[i - 1]++;                       //Left
                map[i] = -1;                                                        //Center
                if (hasRight && map[i + 1] >= 0) map[i + 1]++;                      //Right

                if (hasDown)
                {
                    if (hasLeft && map[i + width - 1] >= 0) map[i + width - 1]++;   //TopLeft
                    if (map[i + width] >= 0) map[i + width]++;                      //TopMiddle
                    if (hasRight && map[i + width + 1] >= 0) map[i + width + 1]++;  //TopRight
                }

            }
        }
        totalFlashCount += flashCount;
    }
    while (flashCount > 0);

    return totalFlashCount;
}
