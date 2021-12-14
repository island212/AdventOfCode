using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

var reports = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input.txt"));

var folds = new List<Point>();
var dots = new List<Point>();
for (int i = 0; i < reports.Length; i++)
{
    if(reports[i] == "")
        continue;

    if (reports[i][0] == 'f')
    {
        var split = reports[i].Split('=');
        if (split[0][split[0].Length - 1] == 'x')
            folds.Add(new Point(int.Parse(split[1]), 0));
        else
            folds.Add(new Point(0, int.Parse(split[1])));

    }
    else
    {
        var split = reports[i].Split(',');
        dots.Add(new Point
        {
            x = int.Parse(split[0]),
            y = int.Parse(split[1])
        });
    }
}

Console.WriteLine("########## Day 13 2021 ##########");
Console.WriteLine($"Part one solution: {SolvePartOne(dots, folds)}");
Console.WriteLine($"Part two solution:");
Console.WriteLine(SolvePartTwo(dots, folds));
Console.WriteLine("################################");

static int SolvePartOne(List<Point> initDots, List<Point> folds)
{
    var dots = new List<Point>(initDots);

    int width = 0;
    int height = 0;
    foreach (var point in dots)
    {
        width = Math.Max(point.x, width);
        height = Math.Max(point.y, height);
    }

    if (folds[0].x != 0)
    {
        //X fold
        for (int i = 0; i < dots.Count; i++)
        {
            int distance = dots[i].x - folds[0].x;
            if (distance > 0)
            {
                Point value = dots[i];
                value.x -= distance * 2;
                dots[i] = value;
            }
        }
    }
    else
    {
        //Y fold
        for (int i = 0; i < dots.Count; i++)
        {
            int distance = dots[i].y - folds[0].y;
            if (distance > 0)
            {
                Point value = dots[i];
                value.y -= distance * 2;
                dots[i] = value;
            }
        }
    }

    return dots.Distinct().Count();
}

static string SolvePartTwo(List<Point> initDots, List<Point> folds)
{
    var dots = new List<Point>(initDots);

    int width = 0;
    int height = 0;
    foreach (var point in dots)
    {
        width = Math.Max(point.x + 1, width);
        height = Math.Max(point.y + 1, height);
    }

    foreach (var fold in folds)
    {
        if (fold.x != 0)
        {
            //X fold
            for (int i = 0; i < dots.Count; i++)
            {
                int distance = dots[i].x - fold.x;
                if (distance > 0)
                {
                    Point value = dots[i];
                    value.x -= distance * 2;
                    dots[i] = value;
                }
            }

            width = fold.x;
        }
        else
        {
            //Y fold
            for (int i = 0; i < dots.Count; i++)
            {
                int distance = dots[i].y - fold.y;
                if (distance > 0)
                {
                    Point value = dots[i];
                    value.y -= distance * 2;
                    dots[i] = value;
                }
            }

            height = fold.y;
        }
    }

    var array = new int[height, width];
    foreach (var point in dots)
    {
        array[point.y, point.x]++;
    }

    StringBuilder sb = new StringBuilder();
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            sb.Append(array[i, j] > 0 ? '#' : '.');
        }
        sb.AppendLine();
    }

    return sb.ToString();
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
}