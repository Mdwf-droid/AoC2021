using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
    public class Day02 : ISolveAoc
    {
        private const string Forward = "forward";
        private const string Down = "down";
        private const string Up = "up";

        private Func<string, string, int> GetCommandValue = (commandName, command) => { return int.Parse(command.Replace(commandName,"").Trim()); };

        public string Solve1stPart()
        {
            var input = Utils.ReadInput("InputDay02.txt");
            var commands = input.Split(Environment.NewLine);

            var hpos = 0;
            var depth = 0;

            commands.ToList().ForEach(command =>
            {
                if(command.StartsWith(Forward))
                {
                    hpos += GetCommandValue(Forward, command);
                }
                else if(command.StartsWith(Down))
                {
                    depth += GetCommandValue(Down, command);
                }
                else if(command.StartsWith(Up))
                {
                    depth -= GetCommandValue(Up, command);
                }
            });

            var result = hpos * depth;

            return $"{typeof(Day02).Name}:1st Part:{result}";
        }

        public string Solve2ndPart()
        {
            var input = Utils.ReadInput("InputDay02.txt");
            var commands = input.Split(Environment.NewLine);

            var hpos = 0;
            var depth = 0;
            var aim = 0;

            commands.ToList().ForEach(command =>
            {
                if (command.StartsWith(Forward))
                {
                    var value = GetCommandValue(Forward, command);
                    hpos += value;
                    depth += aim * value;
                }
                else if (command.StartsWith(Down))
                {
                    aim += GetCommandValue(Down, command);
                }
                else if (command.StartsWith(Up))
                {
                    aim -= GetCommandValue(Up, command);
                }
            });

            var result = hpos * depth;

            return $"{typeof(Day02).Name}:2ndPart Part:{result}";
        }
    }
}
