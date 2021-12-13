using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AoC2021
{
    /// <summary>
    /// --- Day 5: Hydrothermal Venture ---
    /// </summary>
    public class Day05 : ISolveAoc
    {
        public string Solve1stPart()
        {
            var input = Utils.ReadInput("InputDay05.txt");
            var coords = input.Split(Environment.NewLine).ToList();

            List<Point[]> points = new List<Point[]>();

            coords.ForEach(coord =>
            {
                var coordSplitted = coord.Split("->");
                var fstPoint = coordSplitted[0].Split(",");
                var sndPoint = coordSplitted[1].Split(",");
                Point p1 = new Point(Convert.ToInt32(fstPoint[0].Trim()), Convert.ToInt32(fstPoint[1].Trim()));
                Point p2 = new Point(Convert.ToInt32(sndPoint[0].Trim()), Convert.ToInt32(sndPoint[1].Trim()));
                points.Add(new Point[] { p1, p2 });
            });

            var maxCoordsX = Math.Max(points.Select(x => x[0].X).Max(), points.Select(x => x[1].X).Max());
            var maxCoordsY = Math.Max(points.Select(x => x[0].Y).Max(), points.Select(x => x[1].Y).Max());

            Matrix m = new Matrix(maxCoordsX, maxCoordsY);

            points.ForEach(x => m.AddLine(x[0], x[1]));
            //m.Draw();

            var result = m.GetOverlap();

            return $"{result}";


        }

        public string Solve2ndPart()
        {
            var input = Utils.ReadInput("InputDay05.txt");
            var coords = input.Split(Environment.NewLine).ToList();

            List<Point[]> points = new List<Point[]>();

            coords.ForEach(coord =>
            {
                var coordSplitted = coord.Split("->");
                var fstPoint = coordSplitted[0].Split(",");
                var sndPoint = coordSplitted[1].Split(",");
                Point p1 = new Point(Convert.ToInt32(fstPoint[0].Trim()), Convert.ToInt32(fstPoint[1].Trim()));
                Point p2 = new Point(Convert.ToInt32(sndPoint[0].Trim()), Convert.ToInt32(sndPoint[1].Trim()));
                points.Add(new Point[] { p1, p2 });
            });

            var maxCoordsX = Math.Max(points.Select(x => x[0].X).Max(), points.Select(x => x[1].X).Max());
            var maxCoordsY = Math.Max(points.Select(x => x[0].Y).Max(), points.Select(x => x[1].Y).Max());

            Matrix m = new Matrix(maxCoordsX, maxCoordsY);

            points.ForEach(x => m.AddLineAll(x[0], x[1]));


           // m.Draw();

            var result = m.GetOverlap();

            return $"{result}";
        }

        private string unitTest = @"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2";
    }

    class Matrix
    {

        public List<List<int>> Table
        {
            get;
            private set;
        }

        public Matrix(int x, int y)
        {
            List<List<int>> table = new List<List<int>>();
            for (int i = 0; i <= y; i++)
            {
                var newline = new List<int>();
                for (int j = 0; j <= x; j++) newline.Add(0);
                table.Add(newline);
            }

            Table = table;
        }

        public void AddLine(Point begin, Point end)
        {
            if (begin.X == end.X || begin.Y == end.Y)
            {
                Rectangle r = GetRectangle(begin, end);
                for (int x = r.Left; x <= r.Right; x++)
                {
                    for (int y = r.Top; y <= r.Bottom; y++)
                        Table[y][x] += 1;
                }

            }

        }

        public void AddLineAll(Point begin, Point end)
        {
            var sensX = 1;
            var sensY = 1;

            if (begin.X > end.X)
            {
                sensX = -1;
            }
            else if (begin.X == end.X)
                sensX = 0;

            if (begin.Y > end.Y)
            {
                sensY = -1;
            }
            else if (begin.Y == end.Y)
                sensY = 0;

            var endLineX = false;
            var endLineY = false;
            var counterX = 0;
            var counterY = 0;
            while (!(endLineX && endLineY))
            {

                Table[begin.Y + counterY][begin.X + counterX] += 1;

                if (begin.X + counterX == end.X)
                    endLineX = true;
                else
                    counterX += sensX;

                if (begin.Y + counterY == end.Y)
                    endLineY = true;
                else
                    counterY += sensY;

            }


        }

        private Rectangle GetRectangle(Point p1, Point p2)
        {
            int left = Math.Min(p1.X, p2.X);
            int right = Math.Max(p1.X, p2.X);
            int top = Math.Min(p1.Y, p2.Y);
            int bottom = Math.Max(p1.Y, p2.Y);
            int width = right - left;
            int height = bottom - top;
            return new Rectangle(left, top, width, height);
        }

        public void Draw()
        {
            Table.ForEach(y =>
            {
                y.ForEach(x => Console.Write(x != 0 ? x : "."));
                Console.WriteLine();
            });
        }

        public int GetOverlap()
        {
            var counter = 0;
            counter = Table.Select(x => x.Where(y => y >= 2).Count()).Sum();

            return counter;
        }

    }
}
