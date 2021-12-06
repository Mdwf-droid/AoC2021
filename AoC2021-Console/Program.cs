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
                new Day03(),
                new Day04(),
                new Day05()
                // new Day06()  <--- Nonononon ! pas encore !!! il va falloir patienter (surtout pour la partie 2...) :D
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
