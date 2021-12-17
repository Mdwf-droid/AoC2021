using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
    /// <summary>
    /// --- Day 15: Chiton ---
    /// </summary>
    public class Day15 : ISolveAoc
    {
        private readonly string _unitTest = @"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581";
        public string Solve1stPart()
        {
            //var input = _unitTest.Trim().Split("\r\n");
            var input = Utils.ReadInput("InputDay15.txt").Trim().Split("\r\n");

            int result = new Graph(input, false).DijkstraCounter();

            return $"{result}";
        }

        public string Solve2ndPart()
        {
            //var input = _unitTest.Trim().Split("\r\n");
            var input = Utils.ReadInput("InputDay15.txt").Trim().Split("\r\n");

            var graph = new Graph(input, false);
            graph.SetPart2();

            int result = graph.DijkstraCounter();

            return $"{result}";
        }

        private class Graph
        {
            private int[,] _graph;
            
            private int[] _dist;
            private int _height;
            private int _width;

            private List<int> GetIntList(int max) => Enumerable.Range(0, max).ToList();

            public Graph(string[] lines, bool part2)
            {
                var height = lines.Length;
                var width = lines[0].Length;

                _graph = new int[height, width];

                GetIntList(height).ForEach(y =>
                    GetIntList(width).ForEach(x =>
                        _graph[y, x] = int.Parse(lines[y].Substring(x, 1))
                    )
                );

                _height = height;
                _width = width;
            }

            public void SetPart2()
            {
                var newHeight = _height * 5;
                var newWidth = _width * 5;
                var newGraph = new int[newHeight, newWidth];

                GetIntList(_width).ForEach(x =>
                    GetIntList(_height).ForEach(y =>
                    newGraph[y, x] = _graph[y, x]
                    )
                ); ;


                GetIntList(newHeight).ForEach(y =>
                   GetIntList(newWidth).ForEach(x =>
                            newGraph[y, x] = (newGraph[y % _height, x % _width] + x / _width + (y / _height) - 1) % 9 + 1
                   ));

                _height = newHeight;
                _width = newWidth;
                _graph = newGraph;
            }

            public int DijkstraCounter()
            {
                var endPoint = new Point(_width - 1, _height - 1);
                var queue = new PrioQueue<Point, int>();
                queue.Enqueue(new Point(0, 0), 0);

                _dist = new int[_width * _height];

                Array.Fill(_dist, int.MaxValue);
                _dist[0] = 0;

                while (queue.Count > 0)
                {
                    var point = queue.Dequeue();

                    if (point == endPoint)
                    {
                        return _dist[point.X + (_width * point.Y)];
                    }
                    point.GetAdjacents(_width, _height).ForEach(neigbour =>
                    {
                        var newDist = _dist[point.X + (_width * point.Y)] + _graph[neigbour.Y, neigbour.X];
                        if (newDist < _dist[neigbour.X + (_width * neigbour.Y)])
                        {

                            _dist[neigbour.X + (_width * neigbour.Y)] = newDist;
                            queue.Enqueue(neigbour, newDist);
                        }
                    });
                }

                return 0; // erreur, ça ne doit pas arriver :)
            }


        }
    }

    static class PointExtend
    {
        public static List<Point> GetAdjacents(this Point point, int maxWidth, int maxHeight)
        {
            var toReturn = new List<Point>();
            if (point.X > 0)
            {
                toReturn.Add(new Point(point.X - 1, point.Y));
            }
            if (point.Y > 0)
            {
                toReturn.Add(new Point(point.X, point.Y - 1));
            }
            if (point.X < maxWidth - 1)
            {
                toReturn.Add(new Point(point.X + 1, point.Y));
            }
            if (point.Y < maxHeight - 1)
            {
                toReturn.Add(new Point(point.X, point.Y + 1));
            }
            return toReturn;
        }
    }

}
