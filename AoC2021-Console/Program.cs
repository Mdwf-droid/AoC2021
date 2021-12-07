using AoC2021;
using System.Linq;
using System;

namespace AoC2021_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ISolveAoc[] days = new ISolveAoc[]
            {
               /* new Day01(),
                new Day02(),
                new Day03(),
                new Day04(),
                new Day05(),
                new Day06(),*/
                new Day07()
            };

            days.ToList().ForEach(day =>
            {
                Console.WriteLine($"{DateTime.Now} - {day.Solve1stPart()}");
                Console.WriteLine($"{DateTime.Now} - {day.Solve2ndPart()}");
                Console.WriteLine("---------------------------------------");
            }
            );

        }
    }
}
