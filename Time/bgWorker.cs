using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Time
{
    /// <summary>
    /// BackgroundWorker类的辅助类
    /// </summary>
    public class bgWorker
    {
        public class ArgumentEntry
        {
            private object objectResult;
            /// <summary>
            /// 存放对像的类Object，不为NULL
            /// </summary>
            public object ObjectResult
            {
                get { return objectResult; }
                set { objectResult = value; }
            }
            private int intResult;
            /// <summary>
            /// 存放对像的类Int 默认为0
            /// </summary>
            public int IntResult
            {
                get { return intResult; }
                set { intResult = value; }
            }
            private bool boolResult;
            /// <summary>
            /// 存放对像的类Bool 默认为False
            /// </summary>
            public bool BoolResult
            {
                get { return boolResult; }
                set { boolResult = value; }
            }
            private List<object> objectList = new List<object>();
            /// <summary>
            /// 存放对像的集体，不为NULL。
            /// </summary>
            public List<object> ObjectList
            {
                get { return objectList; }
                set { objectList = value; }
            }
            /// <summary>
            /// IList类型对像，注意，默认为NULL
            /// </summary>
            public IList IList { get; set; }
            

        }
        public delegate void ArgumentDelegate(ArgumentEntry argument);
        /// <summary>
        /// 执行新BackgroundWorker实体的RunWorkerAsync方法，doWork内的代码是在多线程中运行的。workCompleted中的代码将会在当前线程中运行。
        /// </summary>
        /// <param name="doWork">要使用背景线程做的事情，做完事后，把结果交给doWork的参数ArgumentEntry对像。</param>
        /// <param name="workCompleted">把结果以ArgumentEntry对像为参数传出来。当前线程去执行workCompleted。</param>
        public static void runAsync(ArgumentDelegate doWork, ArgumentDelegate workCompleted)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (ss, ee) => { ArgumentEntry ae = new ArgumentEntry(); if (doWork != null) { doWork(ae); } ee.Result = ae; };
            bw.RunWorkerCompleted += (ss, ee) => { var ae = ee.Result as ArgumentEntry; if (workCompleted != null) { workCompleted(ae); } };
            bw.RunWorkerAsync();
        }
        /// <summary>
        /// 执行新BackgroundWorker实体的RunWorkerAsync方法，doWork内的代码是在多线程中运行的。workCompleted中的代码将会在当前线程中运行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="doWork"></param>
        /// <param name="workCompleted"></param>
        public static void runAsync<T>(Func<T> doWork, Action<T> workCompleted)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (ss, ee) => { ee.Result = doWork(); };
            bw.RunWorkerCompleted += (ss, ee) => { var ae = (T)ee.Result; if (workCompleted != null) { workCompleted(ae); } };
            bw.RunWorkerAsync();
        }
    }
}
