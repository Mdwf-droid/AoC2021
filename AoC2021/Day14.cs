using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
    /// <summary>
    /// --- Day 14: Extended Polymerization ---
    /// </summary>
    public class Day14 : ISolveAoc
    {
        private readonly string _unitTest = @"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C";

        public string Solve1stPart()
        {
            //var input = _unitTest.Split("\r\n");
            var input = Utils.ReadInput("InputDay14.txt").Split("\r\n");

            var template = input[0];
            var pairInsertion = new List<string>(input.AsEnumerable().TakeLast(input.Length - 2))
                .Select(x =>
                {
                    return new { Sequence = x.Split(" -> ")[0], Value = x.Split(" -> ")[1] };
                });


            Enumerable.Range(0, 10).ToList().ForEach(it =>
             {
                 var pairs = Enumerable.Range(0, template.Length - 1).Select(idx => new { Index = idx + 1, Pair = template.Substring(idx, 2) }).ToList();

                 pairs.Reverse();
                 pairs.ForEach(pair =>
                 {
                     var toInsert = pairInsertion.Where(p => p.Sequence == pair.Pair).FirstOrDefault().Value;
                     template = template.Insert(pair.Index, toInsert);
                 }
                     );

             });

            var counters = template.GroupBy(car => car, car => car, (key, g) => new { Car = key, Value = g.Count() });
            var min = counters.Min(x => x.Value);
            var max = counters.Max(x => x.Value);

            var result = max - min;

            //Evidemment, en part2.... c'est la catastrophe :)

            return $"{result}";
        }

        public string Solve2ndPart()
        {
            //var input = _unitTest.Split("\r\n");
            var input = Utils.ReadInput("InputDay14.txt").Split("\r\n");

            // règles d'interpolation
            var rules = input.AsEnumerable().TakeLast(input.Length - 2)
                .Select(x =>
                {
                    return new { Sequence = x.Split(" -> ")[0], Value = x.Split(" -> ")[1][0] };
                }).ToDictionary(x => x.Sequence, x => x.Value);

            // template de base
            var template = input[0];
            var basePairs = Enumerable.Range(0, template.Length - 1).Select(idx => template.Substring(idx, 2)).ToList();
            var pairs = new Dictionary<string, long>();

            //Initialisation des compteurs avec les paires de base
            basePairs.ForEach(x =>
            {
                IncreaseValueInDictionnary(pairs, x, 1L);
            });

           
            //  NB -> O => compteur NO = 1
            // si NB alors NO et OB => NO = 1 et OB = 1 je multiplie l'occurence de O par 2 (penser à diviser le resultat à la fin...)
            Enumerable.Range(0, 40).ToList().ForEach(it =>
             {
                 var newPairs = new Dictionary<string, long>();

                 // current NB compteur 'X'
                 pairs.ToList().ForEach(current =>
                 {
                     // resultat attendu NOB donc :

                     // incrémente compteur premier-valeur (NO) de 'X'
                     IncreaseValueInDictionnary(newPairs, $"{current.Key[0]}{rules[current.Key]}", current.Value);
                     // incrémente compteur valeur-second (OB) de 'X'
                     IncreaseValueInDictionnary(newPairs, $"{rules[current.Key]}{current.Key[1]}", current.Value);

                     // on aura O -> 2X pour une seule occurence...
                 });

                 pairs = newPairs;
             });

            // Héwala !!! y'a plus qu'à compter... en long évidemment :)

            var countDico = new Dictionary<char, long>();

            pairs.ToList().ForEach(keyValue =>
            {
                IncreaseValueInDictionnary(countDico, keyValue.Key[0], keyValue.Value);
                IncreaseValueInDictionnary(countDico, keyValue.Key[1], keyValue.Value);
            });

            // On n'oublie pas de rajouter le premier et le dernier du template de base !!! (sinon... on perds *BEAUCOUP* de temps à chercher.. grr....)
            IncreaseValueInDictionnary(countDico, template.First(), 1);
            IncreaseValueInDictionnary(countDico, template.Last(), 1);

            //Comme on a travaillé sur des paires, il faut penser à diviser par 2 le resultat pour avoir le nombre exact !
            var max = countDico.Max(x => x.Value) >> 1;
            var min = countDico.Min(x => x.Value) >> 1;


            long result = max - min;

            return $"{result}";
        }

        // On va profiter un peu du "dynamic"... une fois n'est pas coutume :p
        private void IncreaseValueInDictionnary(dynamic dico, dynamic key, long value)
        {
            if (dico.ContainsKey(key)) dico[key] += value; else dico[key] = value;
        }
    }
}
