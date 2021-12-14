using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AoC2021
{
    /// <summary>
    /// --- Day 13: Transparent Origami ---
    /// </summary>
    public class Day13 : ISolveAoc
    {
        private readonly string _unitTest = @"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5";

        string ISolveAoc.Solve1stPart()
        {
            //var input = _unitTest;
            var input = Utils.ReadInput("InputDay13.txt");

            var coords = input.Split("\r\n").Where(line => !line.StartsWith("fold") && !String.IsNullOrEmpty(line)).ToList();
            var positions = input.Split("\r\n").Where(line => line.StartsWith("fold")).ToList();

            var points = GetPoints(coords);

            points = Fold(positions.First(), points);

            var result = points.Count();
            return $"{result}";
        }

        string ISolveAoc.Solve2ndPart()
        {
            //var input = _unitTest;
            var input = Utils.ReadInput("InputDay13.txt");

            var coords = input.Split("\r\n").Where(line => !line.StartsWith("fold") && !String.IsNullOrEmpty(line)).ToList();
            var positions = input.Split("\r\n").Where(line => line.StartsWith("fold")).ToList();

            var points = GetPoints(coords);

            positions.ForEach(positionToFold =>
            {
                points = Fold(positionToFold, points);

            });

            var maxX = points.Max(p => p.X);
            var maxY = points.Max(p => p.Y);

            // C'est moche, mais ça marche !!!
            var resultMatrix = new List<string>();
            Enumerable.Range(0, maxY + 1).ToList().ForEach(y => resultMatrix.Add(new string(' ', maxX + 1)));
            points.ToList().ForEach(p =>
            {
                var line = resultMatrix[p.Y].ToArray();
                line[p.X] = '#';
                resultMatrix[p.Y] = new string(line);

            }
            );
            var result = "\r\n";
            resultMatrix.ForEach(x =>
            result += x + "\r\n");


            return $"{result}";
        }

        private IEnumerable<Point> FoldHorizontal(IEnumerable<Point> points, int y)
        {
            return points.Select(p => p.Y < y ? p : new Point(p.X, (y << 1) - p.Y)).Distinct();
        }

        private IEnumerable<Point> FoldVertical(IEnumerable<Point> points, int x)
        {
            return points.Select(p => p.X < x ? p : new Point((x << 1) - p.X, p.Y)).Distinct();
        }

        private IEnumerable<Point> Fold(string position, IEnumerable<Point> pointsToFold)
        {
            var alongx = position.StartsWith("fold along x");
            var foldingPos = int.Parse(position.Split("=")[1]);
            if (alongx)
            {
                return FoldVertical(pointsToFold, foldingPos);
            }
            else
            {
                return FoldHorizontal(pointsToFold, foldingPos);
            }
        }

        private IEnumerable<Point> GetPoints(List<string> coords)
        {
            return coords.Select(coord =>
              {
                  var splitted = coord.Split(",");
                  var x = int.Parse(splitted[0]);
                  var y = int.Parse(splitted[1]);
                  return new Point(x, y);
              });
        }
    }
}
