﻿namespace Day18
{
    class Day18
    {
        [STAThread]
        static void Main(string[] args)
        {
            string path = "input.txt";

            Console.WriteLine("Part 1");
            Part1.Run(path);

            Console.WriteLine();
            Console.WriteLine("Part 2");
            Part2.Run(path);

            return;
        }
    }
}