using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
    /// <summary>
    /// --- Day 11: Dumbo Octopus ---
    /// </summary>
    public class Day11 : ISolveAoc
    {
        private string _unitTest = @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526";

        private int _partOneCounter = 0;

        private List<Point> _points;
        public string Solve1stPart()
        {
            // var input = _unitTest;
            var input = Utils.ReadInput("InputDay11.txt");

            var matrix = input.Split("\r\n").ToList().Select(x => x.Select(y => Convert.ToInt32(new string(new char[] { y }))).ToArray()).ToArray();

            _points = Point.GetPoints(matrix);

            _points.ForEach(x => x.OnPointFlashing += X_OnPointFlashing);

            for (int i = 1; i <= 100; i++)
            {
                IncreaseAll();
            }

            var result = _partOneCounter;

            return $"{result}";
        }

        private void X_OnPointFlashing(Point p)
        {
            _partOneCounter++;
        }

        public string Solve2ndPart()
        {
            var input = Utils.ReadInput("InputDay11.txt");

            var matrix = input.Split("\r\n").ToList().Select(x => x.Select(y => Convert.ToInt32(new string(new char[] { y }))).ToArray()).ToArray();

            _points = Point.GetPoints(matrix);

            var step = 1;

            while (IncreaseAll())
            {
                step++;
            }

            var result = step;

            return $"{result}";
        }

        private bool IncreaseAll()
        {
            _points.ForEach(p =>
            { p.Value += 1; });
            _points.ForEach(p => { if (p.Value == 10) p.Flash(); });
            _points.ForEach(p => { if (p.Value == -1) p.Value = 0; });

            return !_points.All(p => (p.Value == 0));

        }



        private class Point
        {
            static List<Point> _internalList;
            public delegate void PointFlashing(Point p);

            public event PointFlashing OnPointFlashing;

            public int X { get; set; }

            public int Y { get; set; }

            public int? Value { get; set; }


            public List<Point> PointsAround
            {
                get
                {
                    // Moche mais pas le temps :p
                    var toReturn = new List<Point>();
                    var pa = _internalList.Where(p => p.X == this.X - 1 && p.Y == this.Y - 1).FirstOrDefault();
                    if (pa != null) toReturn.Add(pa);
                    pa = _internalList.Where(p => p.X == this.X && p.Y == this.Y - 1).FirstOrDefault();
                    if (pa != null) toReturn.Add(pa);
                    pa = _internalList.Where(p => p.X == this.X + 1 && p.Y == this.Y - 1).FirstOrDefault();
                    if (pa != null) toReturn.Add(pa);
                    pa = _internalList.Where(p => p.X == this.X - 1 && p.Y == this.Y).FirstOrDefault();
                    if (pa != null) toReturn.Add(pa);
                    pa = _internalList.Where(p => p.X == this.X + 1 && p.Y == this.Y).FirstOrDefault();
                    if (pa != null) toReturn.Add(pa);
                    pa = _internalList.Where(p => p.X == this.X - 1 && p.Y == this.Y + 1).FirstOrDefault();
                    if (pa != null) toReturn.Add(pa);
                    pa = _internalList.Where(p => p.X == this.X && p.Y == this.Y + 1).FirstOrDefault();
                    if (pa != null) toReturn.Add(pa);
                    pa = _internalList.Where(p => p.X == this.X + 1 && p.Y == this.Y + 1).FirstOrDefault();
                    if (pa != null) toReturn.Add(pa);

                    return toReturn;
                }
            }


            public Point()
            {
            }

            public void Flash()
            {
                if (OnPointFlashing != null) OnPointFlashing(this);
                // on passe à -1 pour éviter de flasher 2 fois
                this.Value = -1;
                this.PointsAround.ForEach(n =>
                {
                    if (n.Value != -1)
                    {
                        n.Value++;
                        if (n.Value >= 10)
                            n.Flash();
                    }
                });
            }

            public static List<Point> GetPoints(int[][] innerTable)
            {

                var toReturn = new List<Point>();
                var x = 0;
                var y = 0;
                foreach (var line in innerTable)
                {
                    x = 0;
                    foreach (var value in line)
                    {
                        toReturn.Add(new Point() { X = x, Y = y, Value = innerTable[y][x++] });
                    }
                    y++;
                }

                _internalList = toReturn;

                return toReturn;
            }
        }

    }
}
