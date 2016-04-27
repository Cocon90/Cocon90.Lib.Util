using Cocon90.Lib.Util.Ini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cocon90.Lib.Util.Controls
{
    /// <summary>
    /// 带记忆功能的Combox
    /// </summary>
    public class MemCombox : System.Windows.Forms.ComboBox
    {
        List<string> memoryList = new List<string>();
        IniHelper ini = null;
        /// <summary>
        ///  实例化一个带记忆功能的Combox
        /// </summary>
        public MemCombox()
        {
            this.LostFocus += (ss, ee) =>
            {
                if (this.Text.Trim() != "" && (!this.Items.Contains(this.Text)))
                {
                    this.Items.Add(this.Text); memoryList.Add(this.Text);
                }
                if (ini != null)
                {
                    //将最新的MaxMemCount条记录存起来
                    var startIndex = (memoryList.Count - aMaxMemCount) < 0 ? 0 : (memoryList.Count - aMaxMemCount);
                    if (aMaxMemCount > 0)
                    {
                        for (int i = startIndex; i < memoryList.Count; i++)
                        {
                            var txt = memoryList[i];
                            if ((txt + "").Trim() != "") { ini.Write("MemoryList", "key_" + (i - startIndex), txt); }
                        }
                    }
                }
            };
        }
        string inifile = Guid.NewGuid().ToString("N");
        /// <summary>
        /// 记忆文件的存放名称，系统将存放于程序目录下的"memory"文件夹下
        /// </summary>
        public string aIniFileName
        {
            get { return inifile; }
            set
            {
                inifile = value;
                ini = new IniHelper(Application.StartupPath + "\\memory\\" + aIniFileName + "_mem" + ".ini");
                memoryList.Clear();
                this.Items.Clear();
                for (int i = 0; i < aMaxMemCount; i++)
                {
                    string val = ini.Read("MemoryList", "key_" + i) + "";
                    if (val.Trim() == "") { break; }
                    else { memoryList.Add(val); }
                }
                memoryList.ForEach((im) => { this.Items.Add(im); });
            }
        }
        int maxmemcount = 200;
        /// <summary>
        /// 最大记忆的条数
        /// </summary>
        public int aMaxMemCount { get { return maxmemcount; } set { maxmemcount = value; aIniFileName = inifile; } }
    }


}
