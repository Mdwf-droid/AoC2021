using AoC2021;
using System.Linq;
using System;
using System.Diagnostics;

namespace AoC2021_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ISolveAoc[] days = new ISolveAoc[]
            {
                new Day01(),
                new Day02(),
                new Day03(),
                new Day04(),
                new Day05(),
                new Day06(),
                new Day07(),
                new Day08(),
                new Day09(),
                new Day10(),
                new Day11(),
                new Day12(),
                new Day13()
            };

            var watch = new Stopwatch();
            
            days.ToList().ForEach(day =>
            {
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine($"{DateTime.Now} - Class Name     : {day.GetType().Name} ");              
                watch.Start();
                Console.WriteLine($"{DateTime.Now} - First Part     : {day.Solve1stPart()}");
                watch.Stop();
                Console.WriteLine($"{DateTime.Now} - Execution time : {watch.ElapsedMilliseconds} milliseconds");
                watch.Restart();
                Console.WriteLine($"{DateTime.Now} - Second Part    : {day.Solve2ndPart()}");
                watch.Stop();
                Console.WriteLine($"{DateTime.Now} - Execution time : {watch.ElapsedMilliseconds} milliseconds");
                Console.WriteLine("------------------------------------------------");
            }
            );

        }
    }
}
