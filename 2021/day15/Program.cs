using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

Console.WriteLine("########## Day 15 2021 ##########");
Console.WriteLine($"Part one solution: {SolvePartOne(map, width, 0, map.Length - 1)}");
Console.WriteLine($"Part two solution: {SolvePartTwo(map, width, height)}");
Console.WriteLine("################################");

static int SolvePartOne(int[] map, int width, int start, int goal)
{
    IEnumerable<int> GetValidNeighbors(int i)
    {
        if (i % width > 0) yield return i - 1; //Left
        if ((i + 1) % width > 0) yield return i + 1; //Right
        if (i >= width) yield return i - width; //Up
        if (i < map.Length - width) yield return i + width; //Down
    }

    var dist = new int[map.Length];
    var prev = new int[map.Length];

    Array.Fill(dist, int.MaxValue / 2);
    Array.Fill(prev, -1);

    dist[start] = 0;

    var queue = new PriorityQueue<int, int>();
    queue.Enqueue(start, 0);

    while (queue.TryDequeue(out int element, out int _))
    {
        if (element == goal)
        {
            //PrintMap(map, prev, width, start, goal);
            return dist[goal];
        }

        foreach (var neighbor in GetValidNeighbors(element))
        {
            int alt = dist[element] + map[neighbor];
            if (alt < dist[neighbor])
            {
                dist[neighbor] = alt;
                prev[neighbor] = element;
                queue.Enqueue(neighbor, alt);
            }
        }
    }
   
    return -1;
}

static int SolvePartTwo(int[] map, int width, int height)
{
    const int fullSize = 5;

    int newWidth = width * fullSize;
    int lineSize = fullSize * width * height;

    var newMap = new int[map.Length * fullSize * fullSize];
    for (int i = 0; i < newMap.Length; i++)
    {
        int section = i / width % fullSize + i / lineSize;
        int line = i / newWidth % height;
        int value = map[i % width + line * width] + section;
        newMap[i] = (value - 1) % 9 + 1;
    }

    return SolvePartOne(newMap, newWidth, 0, newMap.Length - 1);
}

static void PrintMap(int[] map, int[] cameFrom, int width, int start, int goal)
{
    var letters = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    int height = map.Length / width;

    var sb = new StringBuilder();
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            sb.Append(letters[map[i * width + j]]);
        }
        sb.Append('\n');
    }

    int current = goal;
    while (current != start)
    {
        sb[current + current / width] = '.';
        current = cameFrom[current];
    }
    sb[start + start / width] = '.';

    Console.WriteLine(sb.ToString());
}

public struct KeyValueList
{
    public int Count => keys.Count;

    public List<int> keys;
    public List<int> values;

    public KeyValueList(int capacity)
    {
        keys = new List<int>(capacity);
        values = new List<int>(capacity);
    }

    public int FindMin(out int value)
    {
        int minIndex = 0;
        int minValue = keys[0];
        for (int i = 1; i < keys.Count; i++)
        {
            if (minValue > keys[i])
            {
                minValue = keys[i];
                minIndex = i;
            }
        }
        value = values[minIndex];
        return minIndex;
    }

    public void Add(int key, int value)
    {
        keys.Add(key);
        values.Add(value);
    }

    public void RemoveAtSwapBack(int index)
    {
        keys[index] = keys[keys.Count - 1];
        keys.RemoveAt(keys.Count - 1);

        values[index] = values[values.Count - 1];
        values.RemoveAt(values.Count - 1);
    }
}