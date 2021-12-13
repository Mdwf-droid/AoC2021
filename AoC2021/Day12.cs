using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
    /// <summary>
    /// --- Day 12: Passage Pathing ---
    /// </summary>
    public class Day12 : ISolveAoc
    {
        private readonly string _unitTest1 = @"start-A
start-b
A-c
A-b
b-d
A-end
b-end";

        private readonly string _unitTest2 = @"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc";

        private readonly string _unitTest3 = @"fs-end
he-DX
fs-he
start-DX
pj-DX
end-zg
zg-sl
zg-pj
pj-he
RW-he
fs-DX
pj-RW
zg-RW
start-pj
he-WI
zg-he
pj-fs
start-RW";

       

        private bool _secondPartFilter = false;

        public string Solve1stPart()
        {
            //var input = _unitTest1.Split("\r\n").ToList();
            //var input = _unitTest2.Split("\r\n").ToList();
            //var input = _unitTest3.Split("\r\n").ToList();
            var input = Utils.ReadInput("InputDay12.txt").Split("\r\n").ToList();

            var edges = FindAllEdges(input);

            var result = FindPathsRecursive("start", edges).Count;

            return $"{result}";


        }


        public string Solve2ndPart()
        {
            _secondPartFilter = true;

            //var input = _unitTest1.Split("\r\n").ToList();
            //var input = _unitTest2.Split("\r\n").ToList();
            //var input = _unitTest3.Split("\r\n").ToList();
            var input = Utils.ReadInput("InputDay12.txt").Split("\r\n").ToList();

            var edges = FindAllEdges(input);

            var result = FindPathsRecursive("start", edges).Count;

            return $"{result}";
        }

        private Dictionary<string, List<string>> FindAllEdges(List<string> input)
        {
            var edges = new Dictionary<string, List<string>>();

            var edgesPairs = input.Select(line => line.Split('-')).SelectMany(allEdges => new[] { (allEdges[0], allEdges[1]), (allEdges[1], allEdges[0]) });

            foreach (var (from, to) in edgesPairs)
            {
                if (edges.ContainsKey(from))
                    edges[from].Add(to);
                else
                    edges[from] = new List<string> { to };
            }

            return edges;
        }

        private List<string[]> FindPathsRecursive(string cave, Dictionary<string, List<string>> edges, string[] alreadyVisited = null)
        {
            alreadyVisited ??= new[] { cave };
            if (cave == "end") return new List<string[]> { alreadyVisited };
            return edges[cave]
                .Where(neighbour => IsValidNeighbour(neighbour, alreadyVisited))
                .SelectMany(neighbour => FindPathsRecursive(neighbour, edges, alreadyVisited.Append(neighbour).ToArray())).ToList();
        }

        private bool IsValidNeighbour(string neighbour, string[] alreadyVisited)
        {
            // start déja visité... non valide
            if (neighbour == "start")
                return false;

            // cave large, valide
            if (neighbour == neighbour.ToUpper())
                return true;

            // pas encore visité, valide
            if (alreadyVisited.All(cave => cave != neighbour))
                return true;

            // partie 2 uniquement, 2 petites caves visitées, valide 
            if (_secondPartFilter &&
                !alreadyVisited.Where(cave => (cave != cave.ToUpper())).Any(smallcave => alreadyVisited.Count(v => v == smallcave) == 2))
                return true; 

            // Sinon, non valide
            return false;
        }
    }
}
