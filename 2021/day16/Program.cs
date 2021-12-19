using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var reports = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input.txt"));

bool isOdd = reports.Length % 2 == 1;
int length = reports.Length / 2 + (isOdd ? 1 : 0);

var data = new byte[length];
for (int i = 0; i < length; i++)
{
    data[i] = (byte)(HexToByte(reports[i * 2]) << 4 | HexToByte(reports[i * 2 + 1]));
}

if (isOdd)
{
    data[length - 1] = (byte)(HexToByte(reports[reports.Length - 1]) << 4);
}

Console.WriteLine("########## Day 16 2021 ##########");
Console.WriteLine($"Part one solution: {SolvePartOne(data)}");
Console.WriteLine($"Part two solution: {SolvePartTwo(data)}");
Console.WriteLine("################################");

static int SolvePartOne(byte[] data)
{
    var reader = new BitReader(data);

    int Decode()
    {
        int version = reader.ReadBits(3);
        int type = reader.ReadBits(3);

        switch (type)
        {
            case 4:
                int literal = 0;
                bool hasNext;
                do
                {
                    hasNext = reader.ReadBits(1) == 1;

                    literal <<= 4;
                    literal |= reader.ReadBits(4);
                }
                while (hasNext);
                return version;
            default:
                int versionSum = 0;
                if (reader.ReadBits(1) == 1)
                {
                    int count = reader.ReadBits(11);
                    for (int i = 0; i < count; i++)
                        versionSum += Decode();
                }
                else
                {
                    int length = reader.ReadBits(15);
                    int target = reader.pos + length;
                    while (reader.pos < target)
                        versionSum += Decode();
                }
                return versionSum + version;
        }
    }

    return Decode();
}

static long SolvePartTwo(byte[] data)
{
    var reader = new BitReader(data);

    long Decode()
    {
        int debugPos = reader.pos;
        int version = reader.ReadBits(3);
        int type = reader.ReadBits(3);

        switch (type)
        {
            case 4:
                long literal = 0;
                bool hasNext;
                do
                {
                    hasNext = reader.ReadBits(1) == 1;

                    literal <<= 4;
                    literal |= (long)reader.ReadBits(4);
                }
                while (hasNext);
                return literal;
            default:
                long value = 0;
                if (reader.ReadBits(1) == 1)
                {
                    int count = reader.ReadBits(11);
                    switch (type)
                    {
                        case 0:
                            value = Decode();
                            for (int i = 1; i < count; i++)
                                value += Decode();
                            break;
                        case 1:
                            value = Decode();
                            for (int i = 1; i < count; i++)
                                value *= Decode();
                            break;
                        case 2:
                            value = Decode();
                            for (int i = 1; i < count; i++)
                                value = Math.Min(Decode(), value);
                            break;
                        case 3:
                            value = Decode();
                            for (int i = 1; i < count; i++)
                                value = Math.Max(Decode(), value);
                            break;
                        case 5:
                            value = Decode() > Decode() ? 1 : 0;
                            break;
                        case 6:
                            value = Decode() < Decode() ? 1 : 0;
                            break;
                        case 7:
                            value = Decode() == Decode() ? 1 : 0;
                            break;
                        default:
                            throw new System.NotImplementedException();
                    }
                }
                else
                {
                    int length = reader.ReadBits(15);
                    int target = reader.pos + length;
                    switch (type)
                    {
                        case 0:
                            value = Decode();
                            while (reader.pos < target)
                                value += Decode();
                            break;
                        case 1:
                            value = Decode();
                            while (reader.pos < target)
                                value *= Decode();
                            break;
                        case 2:
                            value = Decode();
                            while (reader.pos < target)
                                value = Math.Min(Decode(), value);
                            break;
                        case 3:
                            value = Decode();
                            while (reader.pos < target)
                                value = Math.Max(Decode(), value);
                            break;
                        case 5:
                            value = Decode() > Decode() ? 1 : 0;
                            break;
                        case 6:
                            value = Decode() < Decode() ? 1 : 0;
                            break;
                        case 7:
                            value = Decode() == Decode() ? 1 : 0;
                            break;
                        default:
                            throw new System.NotImplementedException();
                    }
                }
                return value;
        }
    }

    return Decode();
}

static byte HexToByte(char hex) => hex switch
{
    '0' => 0,
    '1' => 1,
    '2' => 2,
    '3' => 3,
    '4' => 4,
    '5' => 5,
    '6' => 6,
    '7' => 7,
    '8' => 8,
    '9' => 9,
    'A' => 10,
    'B' => 11,
    'C' => 12,
    'D' => 13,
    'E' => 14,
    'F' => 15,
    _ => throw new System.NotImplementedException()
};

public struct BitReader
{
    public int pos;
    public byte[] stream;

    public BitReader(byte[] stream)
    {
        pos = 0;
        this.stream = stream;
    }

    public int ReadBits(int bits)
    {
        int value = 0;
        int target = pos + bits;
        while (pos < target)
        {
            int remains = 8 - pos % 8;                              //Remaining bits in this index
            int read = Math.Min(remains, target - pos);             //The number of bits we can or want to read in this index

            value <<= read;                                         //Add space for the new bits

            int mask = (255 >> (8 - read)) << (remains - read);     //Find the mask for the value
            value |= (stream[pos / 8] & mask) >> (remains - read);  //Get the value and move it to the start
            pos += read;
        }
        return value;
    }
}