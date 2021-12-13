using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
    /// <summary>
    /// --- Day 6: Lanternfish ---
    /// </summary>
    public class Day06 : ISolveAoc
    {

        private string unitTest = @"3,4,3,1,2";

        public string Solve1stPart()
        {
            var result = 0;
            var input = Utils.ReadInput("InputDay06.txt");
            var fishes = new List<int>();
            input.Split(",").ToList().ForEach(x => fishes.Add(Convert.ToInt32(x)));

            // Mauvaise idée de la 1st part :D
            for (int i = 0; i < 80; i++)
            {
                var addOne = 0;
                var cpt = 0;
                var fcopy = new List<int>(fishes.ToArray());
                fcopy.ForEach(f =>
                {
                    if (f == 0)
                    {
                        fishes[cpt] = 6;
                        addOne++;
                    }
                    else
                        fishes[cpt] = f - 1;

                    cpt++;

                });
                for (int j = 0; j < addOne; j++) fishes.Add(8);               
            }

            result = fishes.Count();

            return $"{result}";
        }

        public string Solve2ndPart()
        {
            var result = 0L;
            var input = Utils.ReadInput("InputDay06.txt");
            var fishes = new List<int>();
            
            input.Split(",").ToList().ForEach(x => fishes.Add(Convert.ToInt32(x)));

            // On change complètement d'algo.. on se concentre sur les ages uniquements avec un tableau contenant uniquement le nombre de poissons par age...
            var fishAgeShifter = new Fishes(fishes);
            fishAgeShifter.Shift(256);


            result = fishAgeShifter.Total;
            return $"{result}";
        }


    }

    class Fishes
    {
        long[] ages;
        public Fishes(List<int> fishes)
        {
            ages = new long[9];
            Array.Fill(ages, 0);
            foreach (int age in fishes)
            {
                ages[age]++;
            }
        }
        public void Shift(int days)
        {
            for (int d = 0; d < days; d++)
            {
                long nb0 = ages[0];
                Array.Copy(ages, 1, ages, 0, 8);
                ages[6] += nb0;
                ages[8] = nb0;
            }
        }

        public long Total
        {
            get
            {
                return ages.Sum();
            }
        }
    }

}
