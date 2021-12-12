using System;
using System.Collections.Generic;
using System.IO;

var reports = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input.txt"));

var keys = new List<string>();
var nodes = new List<Node>();

keys.Add("start");
nodes.Add(new Node("start"));
keys.Add("end");
nodes.Add(new Node("end"));

for (int i = 0; i < reports.Length; i++)
{
    string[] link = reports[i].Split("-");

    Node node1 = FindOrCreate(link[0]);
    Node node2 = FindOrCreate(link[1]);

    node1.Paths.Add(node2);
    node2.Paths.Add(node1);
}

Node FindOrCreate(string key)
{
    int index = keys.IndexOf(key);
    if (index == -1)
    {
        index = keys.Count;
        keys.Add(key);
        nodes.Add(new Node(key));
    }

    return nodes[index];
}

Console.WriteLine("########## Day 12 2021 ##########");
Console.WriteLine($"Part one solution: {SolvePartOne(nodes)}");
Console.WriteLine($"Part two solution: {SolvePartTwo(nodes)}");
Console.WriteLine("################################");

static int SolvePartOne(List<Node> nodes)
{
    int uniquePathFound = 0;
    Explore(nodes[0]);

    void Explore(Node node)
    {
        if (node.Visited != -1)
            node.Visited++;

        foreach (var path in node.Paths)
        {
            if (path == nodes[1])
                uniquePathFound++;
            else if (path.Visited <= 0)
                Explore(path);
        }

        if (node.Visited != -1)
            node.Visited--;
    }

    return uniquePathFound;
}

static long SolvePartTwo(List<Node> nodes)
{
    int uniquePathFound = 0;
    bool hasVisitedTwice = false;

    //Disable visiting twice on start and end nodes
    nodes[0].Visited = 10;
    nodes[0].Visited = 20;

    Explore(nodes[0]);

    void Explore(Node node)
    {
        if (node.Visited != -1)
        {
            node.Visited++;
        }

        foreach (var path in node.Paths)
        {
            if (path == nodes[1])
                uniquePathFound++;
            else if (path.Visited == 1 && !hasVisitedTwice)
            {
                hasVisitedTwice = true;
                Explore(path);
                hasVisitedTwice = false;
            }
            else if (path.Visited <= 0)
                Explore(path);
        }

        if (node.Visited != -1)
            node.Visited--;
    }

    return uniquePathFound;
}

public class Node
{
    public int Visited { get; set; }
    public List<Node> Paths { get; private set; }
    public string Name { get; private set; }

    public Node(string name)
    {
        Name = name;
        Paths = new List<Node>();
        Visited = char.IsLower(name[0]) ? 0 : -1;
    }

    public override string ToString()
    {
        return Name;
    }
}