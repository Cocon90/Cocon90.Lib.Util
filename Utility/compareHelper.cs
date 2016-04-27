using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Utility
{
    /// <summary>
    /// 比较两具元素是的辅助类
    /// </summary>
    public class compareHelper
    {
        /// <summary>
        /// 比较两个数组的元素是否一一对应且相同。
        /// </summary>
        public static bool arrayCompare(Array arr1, Array arr2)
        {
            if (arr1 == null && arr2 == null) return true;
            if (arr1 != null && arr2 == null || arr1 == null && arr2 != null) return false;
            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr2.GetValue(i) != arr1.GetValue(i)) return false;
            }
            return true;
        }
    }
}
