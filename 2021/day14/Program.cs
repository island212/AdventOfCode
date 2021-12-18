using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var reports = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input.txt"));

var insertFormulas = new PairInsert[reports.Length - 2];
for (int i = 0; i < insertFormulas.Length; i++)
{
    var split = reports[i + 2].Split(" -> ");
    insertFormulas[i] = new PairInsert
    {
        pair = new Pair
        {
            element1 = split[0][0],
            element2 = split[0][1]
        },
        insert = split[1][0]
    };
}

var polyFormulas = new PolyFormula[insertFormulas.Length];
for (int i = 0; i < insertFormulas.Length; i++)
{
    polyFormulas[i].left = Array.FindIndex(insertFormulas, x => x.pair == insertFormulas[i].LeftPair);
    polyFormulas[i].right = Array.FindIndex(insertFormulas, x => x.pair == insertFormulas[i].RightPair);
}

Console.WriteLine("########## Day 14 2021 ##########");
Console.WriteLine($"Part one solution: {SolvePartOne(reports[0], insertFormulas, polyFormulas, 10)}");
Console.WriteLine($"Part two solution: {SolvePartOne(reports[0], insertFormulas, polyFormulas, 40)}");
Console.WriteLine("################################");

static long SolvePartOne(string template, PairInsert[] insertFormulas, PolyFormula[] polyFormulas, int iterations)
{
    var pairs = new long[polyFormulas.Length];
    var polyCount = new long[polyFormulas.Length];
    for (int i = 1; i < template.Length; i++)
    {
        int index = Array.FindIndex(insertFormulas, x => x.pair == new Pair(template[i - 1], template[i]));
        if(index >= 0)
            pairs[index]++;
    }

    var newStepResults = new long[polyFormulas.Length];
    for (int step = 0; step < iterations; step++)
    {
        for (int i = 0; i < polyFormulas.Length; i++)
        {
            if (pairs[i] > 0)
            {
                newStepResults[polyFormulas[i].left] += polyFormulas[i].left >= 0 ? pairs[i] : 0;
                newStepResults[polyFormulas[i].right] += polyFormulas[i].right >= 0 ? pairs[i] : 0;
                polyCount[i] += pairs[i];

                pairs[i] = 0;
            }
        }

        var swapped = pairs;
        pairs = newStepResults;
        newStepResults = swapped;
    }

    var counter = new ElementCounter();
    counter.Init(template);
    for (int i = 0; i < polyCount.Length; i++)
    {
        counter.Add(insertFormulas[i].insert, polyCount[i]);
    }

    return counter.counts.Max() - counter.counts.Min();
}

public struct ElementCounter
{
    public List<char> elements;
    public List<long> counts;

    public void Init(string template)
    {
        elements = new List<char>();
        counts = new List<long>();

        for (int i = 0; i < template.Length; i++)
            Add(template[i], 1);
    }

    public void Add(char element, long count)
    {
        int index = elements.IndexOf(element);
        if (index == -1)
        {
            elements.Add(element);
            counts.Add(count);
        }
        else
            counts[index]+=count;
    }
}

public struct PolyFormula
{
    public int left;
    public int right;
}

public struct PairInsert
{
    public Pair pair;
    public char insert;

    public Pair LeftPair => new Pair(pair.element1, insert);
    public Pair RightPair => new Pair(insert, pair.element2);
}

public struct Pair
{
    public char element1;
    public char element2;

    public Pair(char element1, char element2)
    {
        this.element1 = element1;
        this.element2 = element2;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Pair))
            return false;

        return (Pair)obj == this;
    }

    public static bool operator ==(Pair left, Pair right)
    {
        return left.element1 == right.element1 && left.element2 == right.element2;
    }

    public static bool operator !=(Pair left, Pair right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        int hash = 13;
        hash = (hash * 7) + element1.GetHashCode();
        hash = (hash * 7) + element2.GetHashCode();
        return hash;
    }

    public override string ToString()
    {
        return $"{element1}{element2}";
    }
}