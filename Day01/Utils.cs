using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021
{
    public class Utils
    {
        public static string ReadInput(string fileName)
        {
            var content = string.Empty;

            using (var reader = File.OpenText(fileName))
            {
                content = reader.ReadToEnd();
            }

            return content;
        }
    }
}
