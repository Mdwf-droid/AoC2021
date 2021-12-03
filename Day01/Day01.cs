using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace AoC2021
{
    public class Day01 : ISolveAoc
    {

        private int CountIncreased(List<int> values)
        {
            var oldValue = -1;
            var increasedCounter = 0;
            values.ToList().ForEach(value =>
            {
                if (oldValue != -1 && value > oldValue)
                {
                    increasedCounter++;

                }
                oldValue = value;

            });

            return increasedCounter;
        }

        private List<int> ReadInputValues()
        {
            var valueList = new List<int>();
            List<int> slideWindow = new();

            using (var reader = File.OpenText("InputDay01.txt"))
            {
                var content = reader.ReadToEnd();
                var valueListStr = content.Split(Environment.NewLine);
                valueListStr.ToList().ForEach(x => valueList.Add(int.Parse(x)));
            }

            return valueList;
        }

        public string Solve1stPart()
        {
            var result = CountIncreased(ReadInputValues());



            return $"{typeof(Day01).Name}:1st Part:{result}";

        }

        public string Solve2ndPart()
        {
            var valueList = ReadInputValues();
            var slideWindow = new List<int>();

            var counter = 0;
            valueList.ForEach(value =>
            {
                slideWindow.Add(value);
                if (counter >= 2)
                {
                    slideWindow[counter - 2] += value;
                }
                if (counter >= 1)
                {
                    slideWindow[counter - 1] += value;
                }
                counter++;
            });

            var result = CountIncreased(slideWindow);

            return $"{typeof(Day01).Name}:2nd Part:{result}";

        }
    }
}
