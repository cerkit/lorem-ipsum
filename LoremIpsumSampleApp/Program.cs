﻿using System;
using System.Diagnostics;
using System.Threading;
using static LoremIpsum.LoremIpsumUtil;

namespace LoremIpsum
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                // toggle between words and paragraphs
                bool isEven = i % 2 == 0;
                bool isThird = i % 3 == 0;
                LipsumType lipsumType = isEven ? LipsumType.Paragraphs : LipsumType.Words;

                // only start with Lorem Ipsum every third call
                Debug.WriteLine(LoremIpsumUtil.GetNewLipsum(lipsumType, 7, isThird).feed.lipsum + Environment.NewLine);

                // sleep for 1 second to give the server a rest
                Thread.Sleep(1000);
            }
        }
    }
}