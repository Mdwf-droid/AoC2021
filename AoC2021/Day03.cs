using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
    public class Day03 : ISolveAoc
    {
        public string Solve1stPart()
        {
            var input = Utils.ReadInput("InputDay03.txt");
            var diagnostic = input.Split(Environment.NewLine);
            var gamaRate = string.Empty;
            var diagLength = diagnostic.Max(diag => diag.Length);

            var bitCounter = new int[diagLength];

            diagnostic.ToList().ForEach(diagBin =>
            {
                for (int b = 0; b < diagBin.Length; b++)
                    if (diagBin[b] == '1') bitCounter[b]++;
            });

            bitCounter.ToList().ForEach(bit =>
            {
                if (bit > diagnostic.Length / 2) gamaRate += "1"; else gamaRate += "0";
            });

            var epsylonRate = gamaRate.Replace("0", "i").Replace("1", "0").Replace("i", "1");
            int gama = Convert.ToInt32(gamaRate, 2);
            int epsylon = Convert.ToInt32(epsylonRate, 2);


            var result = gama * epsylon;
            return $"{typeof(Day03).Name}:1stPart Part:{result}";
        }

        public string Solve2ndPart()
        {
            var input = Utils.ReadInput("InputDay03.txt");
            var diagnostic = input.Split(Environment.NewLine);

            var oxygenGeneratorRating = 0;
            var co2ScrubberRating = 0;

            var diagWindow = diagnostic;
            var pos = 0;
            while (diagWindow.Length > 1)
            {
                diagWindow = GetSubDiag(diagWindow, pos++, true, true);
            }            
            oxygenGeneratorRating = Convert.ToInt32(diagWindow[0], 2);

            diagWindow = diagnostic;
            pos = 0;
            while (diagWindow.Length > 1)
            {
                diagWindow = GetSubDiag(diagWindow, pos++, false, false);
            }
            co2ScrubberRating = Convert.ToInt32(diagWindow[0], 2);

            var result = oxygenGeneratorRating * co2ScrubberRating;
            return $"{typeof(Day03).Name}:2ndPart Part:{result}";
        }

        private string[] GetSubDiag(string[] diagWindow, int diagPos, bool oneOnEquals, bool getActiveBits)
        {
            var result0 = new List<string>();
            var result1 = new List<string>();

            diagWindow.ToList().ForEach(diag =>
            {
                if (diag[diagPos] == '1')
                    result1.Add(diag);
                else
                    result0.Add(diag);
            });


            if (result1.Count == result0.Count)
               return oneOnEquals ? result1.ToArray() : result0.ToArray();


            if (getActiveBits)
                return result1.Count > result0.Count ? result1.ToArray() : result0.ToArray();
            else
                return result1.Count > result0.Count ? result0.ToArray() : result1.ToArray();


        }

        private string unitTest = @"00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010";
    }
}
