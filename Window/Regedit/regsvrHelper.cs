using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Window.Regedit
{
    /// <summary>
    /// dll或ocx注册工具
    /// </summary>
    public class regsvrHelper
    {
        /// <summary>
        /// 注册结果
        /// </summary>
        public enum RegResult
        {
            /// <summary>
            /// 0 已经注册
            /// </summary>
            HasReg = 0,
            /// <summary>
            /// 1 注册成功
            /// </summary>
            RegSuccess = 1,
            /// <summary>
            /// 2 注册失败
            /// </summary>
            RegFail = -1,
            /// <summary>
            /// 预注册的.OCX或dll文件不存在
            /// </summary>
            RegFileNotExist = -2
        }
        /// <summary>
        /// 注册控件，clsidGuid:传入注册的CLSID编码(Guid值)用为组件的唯一编号。ocxNote:备注 。ocxFilePath文件路径 
        /// </summary>
        /// <param name="ocxFilePath"></param>
        /// <returns>[0]--已经注册过;[1]--注册成功；[-1]--注册失败；[-2]--预注册的.OCX文件不存在</returns>
        public static RegResult RegControl(Guid clsidGuid, string ocxNote, string ocxFilePath)
        {
            try
            {
                if (File.Exists(ocxFilePath))
                {
                    var dirPath = Path.GetDirectoryName(ocxFilePath);
                    var fileName = Path.GetFileName(ocxFilePath);
                    return (RegResult)RegOcxFile("CLSID\\{" + clsidGuid.ToString().ToUpper() + "}", "", ocxNote, dirPath, fileName);
                }
                else return RegResult.RegFileNotExist;
            }
            catch { return RegResult.RegFail; }
        }

        private static int RegOcxFile(string RegKey, string GetValue, string UseValue, string FileDir, string FileName)
        {
            try
            {
                RegistryKey RegRoot = Registry.ClassesRoot;
                RegistryKey RegName = RegRoot.OpenSubKey(RegKey);
                string Dfile = Environment.GetFolderPath(System.Environment.SpecialFolder.Windows) + "\\" + FileName.ToString();
                string Sfile = FileDir.ToString() + "\\" + FileName.ToString();
                string RegOcxFile = Environment.GetFolderPath(System.Environment.SpecialFolder.Windows) + "\\" + FileName.ToString();

                if ((object)RegName == null || !File.Exists(RegOcxFile))
                {
                    if (!File.Exists(Sfile))
                    {
                        return -2;
                    }

                    File.Copy(Sfile, Dfile, true);
                    RunCmd(" \"" + Dfile + "\"  /s");

                    return 1;
                }
                else
                {
                    if (!(RegName.GetValue(GetValue.ToString()).ToString() == UseValue.ToString()) || !File.Exists(RegOcxFile))
                    {
                        File.Copy(Sfile, Dfile, true);
                        //						RunCmd ("/Q/C  regsvr32  \""+Dfile+"\"  /s") ;		
                        RunCmd(" \"" + Dfile + "\"  /s");
                        return 1;
                    }
                    return 0;
                }
            }
            catch
            {
                return -1;
            }

        }
        /// <summary>
        /// Process类调用外部程序regsvr32.exe程序
        /// 加入参数 "/c " + 要执行的命令来执行一个dos命令
        /// （/c代表执行参数指定的命令后关闭cmd.exe /k参数则不关闭cmd.exe）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private static string RunCmd(string command)
        {
            Process p = new Process();

            //Process類有一個StartInfo屬性，這個是ProcessStartInfo類，包括了一些屬性和方法，下面我們用到了他的幾個屬性：

            p.StartInfo.FileName = "regsvr32.exe";      //設定程序名
            p.StartInfo.Arguments = command;            //設定程式執行參數
            p.StartInfo.UseShellExecute = false;        //關閉Shell的使用
            p.StartInfo.RedirectStandardInput = true;   //重定向標準輸入
            p.StartInfo.RedirectStandardOutput = true;  //重定向標準輸出
            p.StartInfo.RedirectStandardError = true;   //重定向錯誤輸出
            p.StartInfo.CreateNoWindow = true;          //設置不顯示窗口

            p.Start();   //啟動

            //p.StandardInput.WriteLine(command);       //也可以用這種方式輸入要執行的命令
            //p.StandardInput.WriteLine("exit");        //不過要記得加上Exit要不然下一行程式執行的時候會當機

            string result = p.StandardOutput.ReadToEnd();        //從輸出流取得命令執行結果
            return result;

        }

    }
}
