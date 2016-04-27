using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Utility
{
    /// <summary>
    /// 缓存辅助 T为缓存数据的类型
    /// </summary>
    public class CacheHelper<T>
    {
        /// <summary>
        /// 传入间隔时间
        /// </summary>
        public int IntervalSeconds { get; set; }
        /// <summary>
        /// 构建一个缓存辅助类
        /// </summary>
        /// <param name="intervalSeconds"></param>
        public CacheHelper(int intervalSeconds)
        {
            this.IntervalSeconds = intervalSeconds;
        }
        Dictionary<string, CacheModel<T>> dics = new Dictionary<string, CacheModel<T>>();
        /// <summary>
        /// 缓冲数据字典
        /// </summary>
        public Dictionary<string, CacheModel<T>> CacheData { get { return dics; } set { dics = value; } }
        /// <summary>
        /// 更新数据到缓存,如果没有，则创建
        /// </summary>
        /// <param name="data"></param>
        public void UpdateCache(string key, T data)
        {
            lock (dics)
            {
                if (dics.ContainsKey(key))
                {
                    dics[key] = new CacheModel<T> { Data = data };
                }
                else { dics.Add(key, new CacheModel<T> { Data = data }); }
            }
        }
        /// <summary>
        /// 数据取缓存，如果有缓冲中有数据且时间小于IntervalSeconds则取缓冲，否则通过generateDataMethod的返回值取得数据并存起来。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="generateDataMethod"></param>
        /// <returns></returns>
        public T ReadCache(string key, GenerateDataDelegate generateDataMethod)
        {
            lock (dics)
            {
                if (dics.ContainsKey(key) && (DateTime.Now - dics[key].UpdateTime).TotalSeconds < IntervalSeconds)
                {
                    return dics[key].Data;
                }
                else
                {
                    var data = generateDataMethod();
                    UpdateCache(key, data);
                    return data;
                }
            }
        }
        /// <summary>
        /// 返回数据的委托
        /// </summary>
        /// <returns></returns>
        public delegate T GenerateDataDelegate();
        /// <summary>
        /// 缓存数据实体
        /// </summary>
        public class CacheModel<T>
        {
            public CacheModel() { this.CacheId = Guid.NewGuid(); UpdateTime = DateTime.Now; }
            /// <summary>
            /// 缓存实体的唯一标记
            /// </summary>
            public Guid CacheId { get; set; }
            /// <summary>
            /// 缓存的数据
            /// </summary>
            public T Data { get; set; }
            /// <summary>
            /// 最近一次更新日期
            /// </summary>
            public DateTime UpdateTime { get; set; }
        }
    }

}
