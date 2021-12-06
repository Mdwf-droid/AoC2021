using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
    public class Day04 : ISolveAoc
    {
        public string Solve1stPart()
        {
            var input = Utils.ReadInput("InputDay04.txt");

            var splittedInput = input.Split(Environment.NewLine);

            var tirage = splittedInput[0].Split(",");

            var grids = new List<Grid>();

            for (int i = 1; i < splittedInput.Length; i += 6)
            {
                splittedInput.Skip(i + 1).Take(5);

                grids.Add(new Grid(splittedInput.Skip(i + 1).Take(5).ToArray()));
            }

            Grid winnerGrid = null;

            var result = 0;

            for (int i = 0; i < tirage.Length; i += 5)
            {
                var currentTirage = tirage.Skip(i).Take(5);

                currentTirage.ToList().ForEach(x =>
                {
                    if (winnerGrid == null)
                    {
                        grids.ForEach(g =>
                    {
                        var number = Convert.ToInt32(x);
                        g.MarkElement(number);

                        if (g.CheckWinGrid())
                        {
                            winnerGrid = g;
                            result = g.GetUnmarkerSum() * number;
                        }
                    });
                    }

                });

                if (winnerGrid != null)
                    break;
            }


            return $"{typeof(Day04).Name}:1stPart Part:{result}";

        }

        public string Solve2ndPart()
        {
            var input =  Utils.ReadInput("InputDay04.txt");

            var splittedInput = input.Split(Environment.NewLine);

            var tirage = splittedInput[0].Split(",");

            var grids = new List<Grid>();

            for (int i = 1; i < splittedInput.Length; i += 6)
            {
                splittedInput.Skip(i + 1).Take(5);

                grids.Add(new Grid(splittedInput.Skip(i + 1).Take(5).ToArray()));
            }

            List<Grid> winnerGrids = new List<Grid>();

            var result = 0;

            for (int i = 0; i < tirage.Length; i += 5)
            {
                var currentTirage = tirage.Skip(i).Take(5);

                currentTirage.ToList().ForEach(x =>
                {

                    grids.ForEach(g =>
                    {
                        var number = Convert.ToInt32(x);
                        g.MarkElement(number);

                        if (!winnerGrids.Contains(g) && g.CheckWinGrid())
                        {
                            winnerGrids.Add(g);
                            result = g.GetUnmarkerSum() * number;
                        }
                    });


                });


            }


            return $"{typeof(Day04).Name}:2ndPart Part:{result}";
        }

        class Grid
        {
            public GridElement[][] Elements { get; private set; }

            public Grid(string[] grid)
            {
                var lines = new List<GridElement[]>();
                var lineElements = new List<GridElement>();

                grid.ToList().ForEach(line =>
                {
                    lineElements = new List<GridElement>();
                    line += " ";
                    for (int i = 0; i < 5; i++)
                    {
                        lineElements.Add(new GridElement() { Value = Convert.ToInt32(line.Substring(i * 3, 3).Trim()) });
                    }
                    lines.Add(lineElements.ToArray());
                });



                Elements = lines.ToArray();
            }

            public void MarkElement(int value)
            {
                Elements.ToList().ForEach(x => x.ToList().ForEach(y => y.Marked |= y.Value == value));
            }

            public bool CheckWinGrid()
            {
                var rowWin = false;
                var colWin = false;
                for (int x = 0; x < 5; x++)
                {
                    colWin = true;
                    for (int y = 0; y < 5; y++)
                    {
                        colWin = colWin && Elements[y][x].Marked;
                    }
                    if (colWin)
                        break;

                }
                for (int y = 0; y < 5; y++)
                {
                    rowWin = true;
                    for (int x = 0; x < 5; x++)
                    {
                        rowWin = rowWin && Elements[y][x].Marked;
                    }
                    if (rowWin)
                        break;
                }

                return rowWin || colWin;
            }

            public int GetUnmarkerSum()
            {
                var result = 0;
                Elements.ToList().ForEach(x => result += x.ToList().Where(z => !z.Marked).Select(z => z.Value).Sum());
                return result;
            }


        }

        class GridElement
        {
            public int Value { get; set; }

            public bool Marked { get; set; }
        }

        private string unitTest = @"7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7";
    }
}
