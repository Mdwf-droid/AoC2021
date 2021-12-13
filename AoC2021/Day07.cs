using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
    /// <summary>
    /// --- Day 7: The Treachery of Whales ---
    /// </summary>
    public class Day07 : ISolveAoc
    {
        private string unitTest = @"16,1,2,0,4,2,7,1,2,14";

        public string Solve1stPart()
        {
            var input = Utils.ReadInput("InputDay07.txt").Split(",").ToList().Select(x => Convert.ToInt32(x)).ToArray();
            //var input = unitTest.Split(",").ToList().Select(x => Convert.ToInt32(x)).ToArray();

            var MaxPos = input.Max();

            Crabs c = new Crabs(MaxPos);

            input.ToList().ForEach(x =>
            {
                c.AddCrab(new CrabFuel(x, MaxPos,false));
            });

            var result = c.BestPosition();

            return $"{result}";
        }

        public string Solve2ndPart()
        {
            var input = Utils.ReadInput("InputDay07.txt").Split(",").ToList().Select(x => Convert.ToInt32(x)).ToArray();
            //var input = unitTest.Split(",").ToList().Select(x => Convert.ToInt32(x)).ToArray();

            var MaxPos = input.Max();

            Crabs c = new Crabs(MaxPos);

            input.ToList().ForEach(x =>
            {
                c.AddCrab(new CrabFuel(x, MaxPos,true));
            });

            var result = c.BestPosition();

            return $"{result}";
        }

        private class CrabFuel
        {
            public int Position { get; set; }

            public Dictionary<int,long> PossiblePos { get; set; }

            public CrabFuel(int position, int MaxPos, bool inc)
            {
                Position = position;
                PossiblePos = new Dictionary<int, long>();
                Enumerable.Range(0, MaxPos).ToList().ForEach(x =>
                {
                    if (!inc)

                        PossiblePos[x] = Math.Abs(Position - x);
                    else
                        PossiblePos[x] = Enumerable.Range(0, Math.Abs(Position - x)+1).ToArray().Sum();

                    });
            
            }
        }

        private class Crabs
        {
            public List<CrabFuel> CrabsFuel { get; set; }

            public Dictionary<int, long> FuelPerPos { get; set; }

            public Crabs(int MaxPos)
            {
                CrabsFuel = new List<CrabFuel>();
                FuelPerPos = new Dictionary<int, long>(MaxPos);
                Enumerable.Range(0, MaxPos).ToList().ForEach(x => FuelPerPos.Add(x, 0));
            }

            public void AddCrab(CrabFuel crab)
            {
                CrabsFuel.Add(crab);

                crab.PossiblePos.Keys.ToList().ForEach(x => FuelPerPos[x] += crab.PossiblePos[x]);
            }

            public long BestPosition()
            {
                // return FuelPerPos.Keys.AsParallel().Where(x => FuelPerPos[x] == FuelPerPos.Values.Min()).FirstOrDefault();
                return FuelPerPos.Values.Min();
            }

            
        }
    }
}
