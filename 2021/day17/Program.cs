using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var reports = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input.txt"));

var area = reports[0].Split(',');

var xAxis = area[0].Split('=')[1].Split("..");
int x1 = int.Parse(xAxis[0]);
int x2 = int.Parse(xAxis[1]);

var yAxis = area[1].Split('=')[1].Split("..");
int y1 = int.Parse(yAxis[0]);
int y2 = int.Parse(yAxis[1]);

var bounds = new Bounds(x1, x2, y1, y2);

Console.WriteLine("########## Day 15 2021 ##########");
Console.WriteLine($"Part one solution: {SolvePartOne(bounds)}");
Console.WriteLine($"Part two solution: {SolvePartTwo(bounds)}");
Console.WriteLine("################################");

static (int,int) SolvePartOne(Bounds bounds)
{
    int probeY = 0;
    int velocityY = 0;
    while (probeY > bounds.y2)
        probeY += --velocityY;

    (int, int) lastValid = (0, 0);

    int newHeight = 0;
    for (int i = 0; i < 100; i++)
    {
        newHeight += i;
        probeY += i;
        if (probeY > bounds.y2)
            probeY += --velocityY;

        if (probeY >= bounds.y1 && probeY <= bounds.y2)
        {
            lastValid = (i, newHeight);
        }
    }

    return lastValid;
}

static int SolvePartTwo(Bounds bounds)
{
    //We only have one target so we can brute force the issue.
    int distinct = 0;
    for (int i = 1; i <= 1000; i++)
    {
        for (int j = 1; j < 100; j++)
            distinct += TestCombination(i, j, bounds) ? 1 : 0;

        for (int j = 0; j >= -100; j--)
            distinct += TestCombination(i, j, bounds) ? 1 : 0;
    }

    return distinct;
}

static bool TestCombination(int velX, int velY, Bounds bounds)
{
    bool found = false;
    int probeX = 0, probeY = 0;
    do
    {
        probeX += velX;
        probeY += velY;

        velX = Math.Max(0, velX - 1);
        velY--;
        if (bounds.IsInside(probeX, probeY))
            found = true;
    }
    while (!bounds.IsTooFar(probeX, probeY) && !found);

    return found;
}

public struct Bounds
{
    public int x1, x2, y1, y2;

    public Bounds(int x1, int x2, int y1, int y2)
    {
        this.x1 = x1 < x2 ? x1 : x2;
        this.x2 = x1 < x2 ? x2 : x1;
        this.y1 = y1 < y2 ? y1 : y2;
        this.y2 = y1 < y2 ? y2 : y1;
    }

    public bool IsInside(int x, int y)
    {
        return x1 <= x && x <= x2 && y1 <= y && y <= y2;
    }

    //Only work if the probe laucher is always over and left to the target
    public bool IsTooFar(int x, int y)
    {
        return y < y1 || x > x2;
    }
}