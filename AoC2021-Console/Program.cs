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
                new Day01(),
                new Day02(),
                new Day03()
            };

            days.ToList().ForEach(day =>
            {
                Console.WriteLine(day.Solve1stPart());
                Console.WriteLine(day.Solve2ndPart());
                Console.WriteLine("---------------------------------------");
            }
            );

        }
    }
}
