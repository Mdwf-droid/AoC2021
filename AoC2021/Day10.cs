using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2021
{
    public class Day10 : ISolveAoc
    {
        private readonly string _unitTest = @"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]";

        private readonly Dictionary<char, int> _scorePart1 = new()
        {
            { ')', 3 },
            { ']', 57 },
            { '}', 1197 },
            { '>', 25137 }
        };

        private readonly Dictionary<char, int> _scorePart2 = new()
        {
            { ')', 1 },
            { ']', 2 },
            { '}', 3 },
            { '>', 4 }
        };

        private readonly Dictionary<char, char> _pairs = new()
        {
            { '(', ')' },
            { '[', ']' },
            { '{', '}' },
            { '<', '>' }
        };




        public string Solve1stPart()
        {
            var input = Utils.ReadInput("InputDay10.txt");

            var lines = input.Split("\r\n").ToList();

            var stack = new Stack<char>();

            var result = 0;

            lines.ForEach(line => line.All(car =>
            {
                if (_scorePart1.ContainsKey(car))
                {
                    if (car != _pairs[stack.Pop()])
                    {
                        result += _scorePart1[car];
                        return true;
                    }
                }
                else
                {
                    stack.Push(car);
                }
                return true;
            }));

            return $"{this.GetType().Name}:1stPart Part:{result}";
        }

        public string Solve2ndPart()
        {
            //var input = _unitTest;
            var input = Utils.ReadInput("InputDay10.txt");

            var lines = input.Split("\r\n").ToList();

            var result = 0L;

            var lineScores = new List<long>();

            lines.ForEach(line => lineScores.Add(ProcessLine(line)));

            lineScores = lineScores.Where(x => x != 0).OrderBy(x => x).ToList();
            result = lineScores[lineScores.Count >> 1];

            return $"{this.GetType().Name}:2ndPart Part:{result}";
        }


        private long ProcessLine(string line)
        {
            var stack = new Stack<char>();

            foreach (var car in line)
            {
                if (_scorePart1.ContainsKey(car))
                {
                    if (car != _pairs[stack.Pop()])
                    {
                        return 0;
                    }
                }
                else
                {                    
                    stack.Push(car);
                }
            }

            long score = 0;
            stack.Select(x => _pairs[x])
                .ToList()
                .ForEach(car => score = score * 5 + _scorePart2[car]);

            return score;
        }
    }
}
