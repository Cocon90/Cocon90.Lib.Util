using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Sorter
{
    /// <summary>
    /// 排序器，可对如：高一年级、高二年级、高三年级 类似数字进行排序。
    /// </summary>
    public class NumberStringSorter : IComparer<string>
    {

        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            x = x.Replace("一", "1").Replace("二", "2").Replace("三", "3").Replace("四", "4").Replace("五", "5").Replace("六", "6").Replace("七", "7").Replace("八", "8").Replace("九", "9");
            y = y.Replace("一", "1").Replace("二", "2").Replace("三", "3").Replace("四", "4").Replace("五", "5").Replace("六", "6").Replace("七", "7").Replace("八", "8").Replace("九", "9");
            if (x == "") { return 1; }
            if (y == "") { return -1; }
            return string.Compare(x, y);
        }
    }
}
