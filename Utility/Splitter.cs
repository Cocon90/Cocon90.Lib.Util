using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Utility
{
    /// <summary>
    /// 通用分支器
    /// </summary>
    public class Splitter<T>
    {
        /// <summary>
        /// 分支选择过滤器
        /// </summary>
        public Func<Splitter<T>, T> Filter { get; set; }
        public Splitter(Func<Splitter<T>, T> filter)
        {
            this.Filter = filter;
        }

        Dictionary<object, T> splitterDictionary = new Dictionary<object, T>();
        /// <summary>
        /// 分支器字典
        /// </summary>
        public Dictionary<object, T> SplitterDictionary { get { return splitterDictionary; } set { splitterDictionary = value; } }

        public static implicit operator T(Splitter<T> common)
        {
            if (common.Filter != null)
            {
                return common.Filter(common);
            }
            return default(T);
        }

    }
}
