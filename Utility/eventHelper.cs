using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Utility
{
    /// <summary>
    /// 事件委托辅助类
    /// </summary>
    public class eventHelper
    {
        /// <summary>
        /// 通知事件。传入Null时不通知。
        /// </summary>
        /// <param name="action"></param>
        public static void notifyEvent(Action action)
        {
            if (action != null) { action(); }
        }

    }
}
