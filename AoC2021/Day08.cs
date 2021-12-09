using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
    public class Day08 : ISolveAoc
    {

        private string _unitTest = @"be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb |fdgacbe cefdb cefbgd gcbe
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec |fcgedb cgb dgebacf gc
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef |cg cg fdcagb cbg
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega |efabcd cedba gadfec cb
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga |gecf egdcabf bgf bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf |gebdcfa ecba ca fadegcb
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf |cefg dcbef fcge gbcadfe
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd |ed bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg |gbdfcae bgc cg cgb
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc |fgae cfgab fg bagce";

        public string Solve1stPart()
        {
            //var input = _unitTest;
            var input = Utils.ReadInput("InputDay08.txt");

            var lines = input.Split((new string[] { "|", "\r\n" }), StringSplitOptions.RemoveEmptyEntries);

            // grosse bidouille classique pour prendre une ligne sur 2... *sifflote*
            var odd = 1;
            var testingSample = lines.Where(x => odd++ % 2 == 0).Select(x => x.Split(" ")).ToArray();
                       
            var counter = 0;

            testingSample.ToList().ForEach(x => x.ToList().ForEach(y =>
             {
                 if (new int[] { 2, 3, 4, 7 }.Contains(y.Length))
                     counter++;

             }));

            var result = counter;

            return $"{typeof(Day08).Name}:1stPart Part:{result}";
        }


        public string Solve2ndPart()
        {
            //var input = _unitTest;
            var input = Utils.ReadInput("InputDay08.txt");

            var map = new List<KeyValuePair<string[], string[]>>();

            input.Split("\r\n").ToList().ForEach(line =>
            {
                var splittedLine = line.Split("|");
                map.Add(new KeyValuePair<string[], string[]>(
                    splittedLine[0].Trim().Split(" ").Select(v => String.Concat(v.OrderBy(c => c))).ToArray(),
                    splittedLine[1].Trim().Split(" ").Select(v => String.Concat(v.OrderBy(c => c))).ToArray()));
            });           

            var result = 0;
            map.ForEach(line =>
            {
                var dicoDeco = new DigitDecoder(line.Key.ToList());
                var decodedValue = "";
                line.Value.ToList().ForEach(entry =>
                {
                    decodedValue += $"{dicoDeco.GetValue(entry)}";
                });

                result += int.Parse(decodedValue);

            });

            return $"{typeof(Day08).Name}:2ndPart Part:{result}";
        }


    }

    class DigitDecoder
    {
        private string[] _entryMapper;

        public int GetValue(String code) => Array.IndexOf(_entryMapper, code);

        // on verifie que code est entièrement inclu dans value (quelque soit la position des caractères)
        private bool SegmentsMatches(string value, string code) => code.Intersect(value).Count() == code.Length;


        public DigitDecoder(List<string> entries)
        {
            var orderedEntries = entries.OrderBy(x => x.Length).ToArray();

            _entryMapper = new string[]
            {
                /*0- 6 segments */null,
                /*1- 2 segments */orderedEntries[0],
                /*2- 5 segments */null,
                /*3- 5 segments */null,
                /*4- 4 segments */orderedEntries[2],
                /*5- 5 segments */null,
                /*6- 6 segments */null,
                /*7- 3 segments */orderedEntries[1],
                /*8- 7 segments */orderedEntries[9],
                /*9- 6 segments*/null
            };

            // Liste ordonnée de manière identique à chaque fois, on sait qu'il y a 6 possibilité 2,3,4,5,6 ou 7 segments
            // position 0: 2 segments - identifié 1
            // position 1: 3 segments - identifié 7
            // position 2: 4 segments - identifié 4
            // position 3: 5 segments - non identifié
            // position 4: 5 segments - non identifié
            // position 5: 5 segments - non identifié
            // position 6: 6 segments - non identifié
            // position 7: 6 segments - non identifié
            // position 8: 6 segments - non identifié
            // position 9: 7 segments - identifié 8
            // On a donc deux groupe de 5 et 6 segments ordonnés à determiner et 4 positions déjà identifiées

            // Dans les digits à 6 segments, on sait que 0 et 9 ont des segments en commun avec 1 et 7, sauf le 6 
            // On peu donc rechercher dans ce groupe le 6 (pas de match exact avec 1 et 7)
            // Ensuite, le 9 a des segments communs avec le 4 (alors que le 0 non)
            // Le 0 est donc le dernier du groupe des 6 segments.
            Enumerable.Range(6, 3).Select(idx => orderedEntries[idx]).ToList().ForEach(entry =>
              {
                  if (SegmentsMatches(entry, _entryMapper[1]) && SegmentsMatches(entry, _entryMapper[7]))
                  {
                      if (SegmentsMatches(entry, _entryMapper[4]))
                          _entryMapper[9] = entry;
                      else
                      {
                          _entryMapper[0] = entry;
                      }
                  }
                  else
                  {
                      _entryMapper[6] = entry;
                  }


              });

            // il reste le groupe de 5 segments correspondant aux digits 2,3 et 5
            // 2 et 5 n'ont pas tous les segments de 1, il reste donc que le 3
            // le 6 contient les segements du 5
            // sinon, le dernier du groupe est le 2
            Enumerable.Range(3, 3).Select(idx => orderedEntries[idx]).ToList().ForEach(entry =>
            {
                if (!SegmentsMatches(entry, _entryMapper[1]))
                {
                    // important d'avoir décodé le 6 dans la première phase !!!
                    if (SegmentsMatches(_entryMapper[6], entry))
                        _entryMapper[5] = entry;
                    else
                    {
                        _entryMapper[2] = entry;
                    }
                }
                else
                {
                    _entryMapper[3] = entry;
                }
            });
        }
    }
}

