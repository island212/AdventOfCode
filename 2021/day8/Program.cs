using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var reports = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input.txt"));

Console.WriteLine("########## Day 8 2021 ##########");
Console.WriteLine($"Part one solution: {SolvePartOne(reports)}");
Console.WriteLine($"Part two solution: {SolvePartTwo(reports)}");
Console.WriteLine("################################");

static int SolvePartOne(string[] reports)
{
    int[] segemntCount = new int[9];
    segemntCount[6]++; //Zero
    segemntCount[2]++; //One
    segemntCount[5]++; //Two
    segemntCount[5]++; //Three
    segemntCount[4]++; //Four
    segemntCount[5]++; //Five
    segemntCount[6]++; //Six
    segemntCount[3]++; //Seven
    segemntCount[7]++; //Eight
    segemntCount[6]++; //Nine

    int counter = 0;
    for (int i = 0; i < reports.Length; i++)
    {
        int end = 0;
        int pos = reports[i].IndexOf(" | ") + 3;
        while ((end = reports[i].IndexOf(' ', pos)) != -1)
        {
            counter += segemntCount[end - pos] == 1 ? 1 : 0;
            pos = end + 1;
        }
        end = reports[i].Length;
        counter += segemntCount[end - pos] == 1 ? 1 : 0;
    }

    return counter;
}

static int SolvePartTwo(string[] reports)
{
    int[] segmentKeys = new int[10];

    int totalNumber = 0;
    for (int i = 0; i < reports.Length; i++)
    {
        var signalOutput = reports[i].Split(" | ");
        FindDigitKeys(segmentKeys, signalOutput[0]);

        int end = 0;
        int pos = 0;

        int GetDigit()
        {
            int key = LineToIndex(signalOutput[1], pos, end);
            return Array.FindIndex(segmentKeys, s => s == key);
        }

        int number = 0;
        int multiplier = 1000;
        while ((end = signalOutput[1].IndexOf(' ', pos)) != -1)
        {
            number += GetDigit() * multiplier;

            pos = end + 1;
            multiplier /= 10;
        }
        end = signalOutput[1].Length;
        number += GetDigit();

        totalNumber += number;
    }

    return totalNumber;
}

static void FindDigitKeys(int[] segmentKeys, string signal)
{
    string digitOne, digitFour, digitSeven;
    digitOne = digitFour = digitSeven = null;

    int[] letterCounts = new int[7];
    {
        int end = 0;
        int pos = 0;

        void ProcessChain()
        {
            for (int i = pos; i < end; i++)
            {
                letterCounts[signal[i] - 'a']++;
            }
            switch (end - pos)
            {
                case 2:
                    digitOne = signal.Substring(pos, end - pos);
                    break;
                case 3:
                    digitSeven = signal.Substring(pos, end - pos);
                    break;
                case 4:
                    digitFour = signal.Substring(pos, end - pos);
                    break;
            }
        }

        while ((end = signal.IndexOf(' ', pos)) != -1)
        {
            ProcessChain();
            pos = end + 1;
        }
        end = signal.Length;
        ProcessChain();
    }

    //   dddd
    //  e    a
    //  e    a
    //   ffff
    //  g    b
    //  g    b
    //   cccc
    //
    //  Using this as an example 
    //  -acedgfb: 8
    //  -cdfbe: 5
    //  -gcdfa: 2
    //  -fbcad: 3
    //  -dab: 7
    //  -cefabd: 9
    //  -cdfgeb: 6
    //  -eafb: 4
    //  -cagedb: 0
    //  -ab: 1
    //
    //  We learn in part one that the number 1, 4, 7 can be found by just the length of the signal
    //  Likewise we can find the position of a segment by the number of time we can find it in the signal
    //  The segements are use to represent the digit and some segment are less used than other
    //
    //  The segment a and d appear 8 time in the signal
    //  The segment f and c appear 7 time in the signal 
    //  The segment e appear 6 time in the signal 
    //  The segment g appear 4 time in the signal 
    //  The segment b appear 9 time in the signal 


    int[] segments = new int[7];
    segments[1] = Array.FindIndex(letterCounts, s => s == 9); //Find the segement b
    segments[4] = Array.FindIndex(letterCounts, s => s == 6); //Find the segement e
    segments[6] = Array.FindIndex(letterCounts, s => s == 4); //Find the segement g

    List<int> segmentFinder = new List<int>(7);
    {
        segmentFinder.AppendText(digitOne);     //ab
        segmentFinder.Remove(segments[1]);      //a
        segments[0] = segmentFinder[0];
    }

    segmentFinder.Clear();
    {
        segmentFinder.AppendText(digitSeven);   //dab
        segmentFinder.Remove(segments[0]);      //db
        segmentFinder.Remove(segments[1]);      //d
        segments[3] = segmentFinder[0];
    }

    segmentFinder.Clear();
    {
        segmentFinder.AppendText(digitFour);    //eafb
        segmentFinder.Remove(segments[0]);      //efb
        segmentFinder.Remove(segments[1]);      //ef
        segmentFinder.Remove(segments[4]);      //f
        segments[5] = segmentFinder[0];
    }

    segmentFinder.Clear();
    {
        segmentFinder.AppendText("abcdefg");    //abcdefg
        segmentFinder.Remove(segments[0]);      //bcdefg
        segmentFinder.Remove(segments[1]);      //cdefg
        segmentFinder.Remove(segments[3]);      //cefg
        segmentFinder.Remove(segments[4]);      //cfg
        segmentFinder.Remove(segments[5]);      //cg
        segmentFinder.Remove(segments[6]);      //c
        segments[2] = segmentFinder[0];
    }

    //Create an unique for every possible digit.
    segmentKeys[0] = SegmentsToIndex("cagedb", segments);
    segmentKeys[1] = SegmentsToIndex("ab", segments);
    segmentKeys[2] = SegmentsToIndex("gcdfa", segments);
    segmentKeys[3] = SegmentsToIndex("fbcad", segments);
    segmentKeys[4] = SegmentsToIndex("eafb", segments);
    segmentKeys[5] = SegmentsToIndex("cdfbe", segments);
    segmentKeys[6] = SegmentsToIndex("cdfgeb", segments);
    segmentKeys[7] = SegmentsToIndex("dab", segments);
    segmentKeys[8] = SegmentsToIndex("acedgfb", segments);
    segmentKeys[9] = SegmentsToIndex("cefabd", segments);
}

static int SegmentsToIndex(string chain, int[] segments)
{
    int value = 0;
    for (int i = 0; i < chain.Length; i++)
    {
        value |= 1 << segments[chain[i] - 'a'];
    }
    return value;
}

static int LineToIndex(string line, int pos, int end)
{
    int value = 0;
    for (int i = pos; i < end; i++)
    {
        value |= 1 << (line[i] - 'a');
    }
    return value;
}

static class Extensions
{
    public static void AppendText(this List<int> array, string text)
    {
        for (int i = 0; i < text.Length; i++)
            array.Add(text[i] - 'a');
    }

    public static string RemoveSegment(this string text, char removed)
    {
        return text.Replace(char.ToString(removed), "");
    }

    public static string RemoveSegment(this string text, string removed)
    {
        for (int i = 0; i < removed.Length; i++)
        {
            text = text.RemoveSegment(removed[i]);
        }
        return text;
    }
}