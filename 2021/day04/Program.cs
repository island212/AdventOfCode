using System;
using System.Collections.Generic;
using System.IO;

const int CARD_SIZE = 5;
const int CARD_AREA = CARD_SIZE * CARD_SIZE;
const int CARD_START_AT = 2;

var reports = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input.txt"));

var numSequences = Array.ConvertAll(reports[0].Split(","), s => int.Parse(s));

var cardCount = (reports.Length - CARD_START_AT + 1) / 6;
var cards = new int[cardCount * CARD_AREA];
for (int i = 0; i < cardCount; i++)
{
    for (int j = 0; j < CARD_SIZE; j++)
    {
        cards[i * CARD_AREA + j * CARD_SIZE]     = int.Parse(reports[i * (CARD_SIZE + 1) + j + CARD_START_AT].Substring(0, 2));
        cards[i * CARD_AREA + j * CARD_SIZE + 1] = int.Parse(reports[i * (CARD_SIZE + 1) + j + CARD_START_AT].Substring(3, 2));
        cards[i * CARD_AREA + j * CARD_SIZE + 2] = int.Parse(reports[i * (CARD_SIZE + 1) + j + CARD_START_AT].Substring(6, 2));
        cards[i * CARD_AREA + j * CARD_SIZE + 3] = int.Parse(reports[i * (CARD_SIZE + 1) + j + CARD_START_AT].Substring(9, 2));
        cards[i * CARD_AREA + j * CARD_SIZE + 4] = int.Parse(reports[i * (CARD_SIZE + 1) + j + CARD_START_AT].Substring(12, 2));
    }
}

Console.WriteLine("########## Day 4 2021 ##########");
Console.WriteLine($"Part one solution: {SolvePartOne(numSequences, cards)}");
Console.WriteLine($"Part two solution: {SolvePartTwo(numSequences, cards)}");
Console.WriteLine("################################");

static (int, int, int) SolvePartOne(int[] numSequences, int[] cards)
{
    const int MARKER_PER_CARD = 10;

    var cardCount = cards.Length / CARD_AREA;

    var points = new int[cardCount];
    var markers = new int[cardCount * MARKER_PER_CARD];

    int cardIndex = -1;
    int numIndex = -1;
    for (numIndex = 0; numIndex < numSequences.Length; numIndex++)
    {
        for (int j = 0; j < cards.Length; j++)
        {
            if (cards[j] == numSequences[numIndex])
            {
                cardIndex  = j / CARD_AREA;
                int row    = cardIndex * 5 + j / CARD_SIZE;
                int column = cardIndex * 10 + j % CARD_SIZE + CARD_SIZE;

                points[cardIndex] += numSequences[numIndex];
                if (++markers[row] == CARD_SIZE || ++markers[column] == CARD_SIZE)
                    goto Found;
            }
        }
    }

Found:
    int sumCard = 0;
    for (int i = 0; i < CARD_AREA; i++)
    {
        sumCard += cards[cardIndex * CARD_AREA + i];
    }

    sumCard -= points[cardIndex];

    return (sumCard, numSequences[numIndex], sumCard * numSequences[numIndex]);
}

static (int, int, int) SolvePartTwo(int[] numSequences, int[] cards)
{
    const int MARKER_PER_CARD = 10;

    var cardCount = cards.Length / CARD_AREA;

    var points = new int[cardCount];
    var markers = new int[cardCount * MARKER_PER_CARD];

    int lastBoardIndex = -1;

    int notFinishedCard;
    int numIndex = 0;
    do
    {
        for (int j = 0; j < cards.Length; j++)
        {
            if (cards[j] == numSequences[numIndex])
            {
                int cardIndex = j / CARD_AREA;
                int row = cardIndex * 5 + j / CARD_SIZE;
                int column = cardIndex * 10 + j % CARD_SIZE + CARD_SIZE;

                points[cardIndex] += numSequences[numIndex];
                markers[row]++;
                markers[column]++;
            }
        }

        notFinishedCard = 0;
        for (int i = 0; i < cardCount && notFinishedCard < 2; i++)
        {
            int index = 0;
            while (index < MARKER_PER_CARD && markers[i * MARKER_PER_CARD + index] != CARD_SIZE)
                index++;

            if (index == MARKER_PER_CARD)
            {
                lastBoardIndex = i;
                notFinishedCard++;
            }
        }
    } 
    while (notFinishedCard != 0 && ++numIndex < numSequences.Length);

    int sumCard = 0;
    for (int i = 0; i < CARD_AREA; i++)
    {
        sumCard += cards[lastBoardIndex * CARD_AREA + i];
    }

    sumCard -= points[lastBoardIndex];

    return (sumCard, numSequences[numIndex], sumCard * numSequences[numIndex]);
}
