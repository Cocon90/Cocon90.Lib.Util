using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Utility
{
    /// <summary>
    /// 将UTF8编码的文本文档中的内容，按行读出，每行中以|分隔的内容，将作为数组。
    /// </summary>
    public class TextFormat
    {
        public static string[][] Load(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return new string[0][];
            }
            StringBuilder sb = new StringBuilder();
            var textLines = File.ReadAllLines(filepath, Encoding.UTF8);
            List<string[]> rows = new List<string[]>();
            foreach (var line in textLines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith("#")) continue;
                var values = line.Split('|');
                List<string> cells = new List<string>();
                foreach (var val in values)
                {
                    cells.Add(val.Trim('\t').Trim(' ').Trim('\t').Trim(' '));
                }
                rows.Add(cells.ToArray());
            }
            return rows.ToArray();

        }
    }
}
