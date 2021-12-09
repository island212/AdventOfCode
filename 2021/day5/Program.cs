using System;
using System.Collections.Generic;
using System.IO;

var reports = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input.txt"));

var lines = new Line[reports.Length];

for (int i = 0; i < reports.Length; i++)
{
    var coordinates = reports[i].Split(" -> ");

    var coord1 = coordinates[0].Split(",");
    lines[i].x1 = int.Parse(coord1[0]);
    lines[i].y1 = int.Parse(coord1[1]);

    var coord2 = coordinates[1].Split(",");
    lines[i].x2 = int.Parse(coord2[0]);
    lines[i].y2 = int.Parse(coord2[1]);

    //So p1 is always closer to 0,0 than p2 
    if (lines[i].y2 < lines[i].y1 || (lines[i].y2 == lines[i].y1 && lines[i].x2 < lines[i].x1))
    {
        int swap = lines[i].x1;
        lines[i].x1 = lines[i].x2;
        lines[i].x2 = swap;

        swap = lines[i].y1;
        lines[i].y1 = lines[i].y2;
        lines[i].y2 = swap;
    }
}

Console.WriteLine("########## Day 5 2021 ##########");
Console.WriteLine($"Part one solution: {SolvePartOne(lines)}");
Console.WriteLine($"Part two solution: {SolvePartTwo(lines)}");
Console.WriteLine("################################");

static int SolvePartOne(Line[] lines)
{
    var terrains = new Dictionary<Point, int>(lines.Length * 2);

    for (int i = 0; i < lines.Length; i++)
    {
        if (lines[i].x1 == lines[i].x2)
        {
            //Horizontal
            for (int j = lines[i].y1; j <= lines[i].y2; j++)
            {
                var p = new Point(lines[i].x1, j);
                if (!terrains.TryAdd(p, 1))
                    terrains[p]++;
            }
        }
        else if (lines[i].y1 == lines[i].y2)
        {
            //Vertical
            for (int j = lines[i].x1; j <= lines[i].x2; j++)
            {
                var p = new Point(j, lines[i].y1);
                if (!terrains.TryAdd(p, 1))
                    terrains[p]++;
            }
        }
    }

    int counter = 0;
    foreach (var value in terrains.Values)
        counter += value > 1 ? 1 : 0;

    return counter;
}

static int SolvePartTwo(Line[] lines)
{
    var terrains = new Dictionary<Point, int>(lines.Length * 2);

    for (int i = 0; i < lines.Length; i++)
    {
        if (lines[i].x1 == lines[i].x2)
        {
            //Horizontal
            for (int j = lines[i].y1; j <= lines[i].y2; j++)
            {
                var p = new Point(lines[i].x1, j);
                if (!terrains.TryAdd(p, 1))
                    terrains[p]++;
            }
        }
        else if (lines[i].y1 == lines[i].y2)
        {
            //Vertical
            for (int j = lines[i].x1; j <= lines[i].x2; j++)
            {
                var p = new Point(j, lines[i].y1);
                if (!terrains.TryAdd(p, 1))
                    terrains[p]++;
            }
        }
        else
        {
            //Diagonal
            int increment = lines[i].x1 < lines[i].x2 ? 1 : -1;
            for (int j = lines[i].y1, k=lines[i].x1; j <= lines[i].y2; j++, k+=increment)
            {
                var p = new Point(k, j);
                if (!terrains.TryAdd(p, 1))
                    terrains[p]++;
            }
        }
    }

    int counter = 0;
    foreach (var value in terrains.Values)
        counter += value > 1 ? 1 : 0;

    return counter;
}

struct Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override int GetHashCode()
    {
        //100_000 is good enough for this case.
        return x + y * 100_000;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Point))
            return false;

        var p = (Point)obj;
        return (x == p.x) && (y == p.y);
    }
}

struct Line
{
    public int x1;
    public int y1;

    public int x2;
    public int y2;

    public override string ToString()
    {
        return $"{x1},{y1} -> {x2},{y2}";
    }
}
