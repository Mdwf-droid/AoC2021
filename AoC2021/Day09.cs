using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
    /// <summary>
    /// --- Day 9: Smoke Basin ---
    /// </summary>
    public class Day09 : ISolveAoc
    {
        private string _unitTest = @"2199943210
3987894921
9856789892
8767896789
9899965678";

        public string Solve1stPart()
        {
            //var input = _unitTest;
            var input = Utils.ReadInput("InputDay09.txt");

            var matrix = input.Split("\r\n").ToList().Select(x => x.Select(y => Convert.ToInt32(new string(new char[] { y }))).ToArray()).ToArray();
            var result = 0;

            var tbl = new Table(matrix);
            result = tbl.GetLowest().Select(x => x.Value + 1).Sum();


            return $"{result}";
        }

        public string Solve2ndPart()
        {
            //var input = _unitTest;
            var input = Utils.ReadInput("InputDay09.txt");

            var matrix = input.Split("\r\n").ToList().Select(x => x.Select(y => Convert.ToInt32(new string(new char[] { y }))).ToArray()).ToArray();
            var result = 0;

            var tbl = new Table(matrix);

            result = tbl.GetBasins();


            return $"{result}";
        }

        private class Table
        {
            readonly int[][] _table;

            public Table(int[][] table)
            {
                _table = table;
            }

            public Point GetValue(int posX, int posY) => new(this)
            {
                PosX = posX,
                PosY = posY,
                //Si on est hors des clous, bha int.MaxValue hein ! comme ça les points adjacents sont forcéments inférieurs :)
                Value = (posX < 0 || posX > _table[0].Length - 1 || posY < 0 || posY > _table.Length - 1) ? int.MaxValue : _table[posY][posX]
            };



            public bool IsTheLowest(Point point) => point.Directions.All(x => x.Value > point.Value);


            public int GetBasins()
            {
                var result = 0;

                var realPoints = new List<Point>();

                for (int y = 0; y < _table.Length; y++)
                {
                    for (var x = 0; x < _table[0].Length; x++)
                    {
                        if (_table[y][x] != 9)
                            realPoints.Add(new Point(this) { PosX = x, PosY = y, Value = _table[y][x] });
                    }
                }

                var mostPos = realPoints.Select(x => FindBasin(x)).ToList();
                var group = (from pos in mostPos
                             group pos by pos.Key into posGroup
                             select posGroup.Count()).OrderByDescending(x => x).ToArray();


                var toMul = group.Take(3).ToArray();
                result = toMul[0] * toMul[1] * toMul[2];

                return result;
            }


            public Point FindBasin(Point p)
            {
                foreach (var d in p.Directions)
                {
                    if (d.Value < p.Value)
                    {
                        // Recursion !!!
                        return FindBasin(d);
                    }
                };

                return p;
            }

            public List<Point> GetLowest()
            {
                var realPoints = new List<Point>();

                for (var y = 0; y < _table.Length; y++)
                {
                    for (var x = 0; x < _table[0].Length; x++)
                    {
                        realPoints.Add(new Point(this) { PosX = x, PosY = y, Value = _table[y][x] });
                    }
                }

                return realPoints.Where(x => IsTheLowest(x)).ToList();


            }

            public class Point
            {
                public int PosX { get; set; }
                public int PosY { get; set; }

                public int Value { get; set; }

                public string Key => $"{PosX}:{PosY}";

                Point Up => _table.GetValue(PosX, PosY - 1);
                Point Down => _table.GetValue(PosX, PosY + 1);
                Point Left => _table.GetValue(PosX - 1, PosY);
                Point Right => _table.GetValue(PosX + 1, PosY);

                public List<Point> Directions => new List<Point>() { Up, Down, Left, Right };

                private Table _table;
                public Point(Table table)
                {
                    _table = table;
                }


            }
        }
    }
}
