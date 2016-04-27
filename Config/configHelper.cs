using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Config
{
    /// <summary>
    /// App.Config文件操作辅助类
    /// </summary>
    public class configHelper
    {
        /// <summary>
        /// 获取指定路径为configFilePath的Config文件的AppSetting里面的指定键值对应的value值，如果configFilePath为NULL，则返回当前程序的Config文件对应的值。
        /// </summary>
        public static string GetAppSettingValue(string key, string defaultValue, string configFilePath = null)
        {
            try
            {
                Configuration config = null;
                if ((configFilePath + "").Trim() != "")
                {
                    var map = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
                    config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                }
                else
                {
                    config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
                string value = config.AppSettings.Settings[key].Value;
                return value;
            }
            catch { return defaultValue; }
        }
        public static bool SetAppSettingValue(string key, string value, string configFilePath = null)
        {
            try
            {
                Configuration config = null;
                if ((configFilePath + "").Trim() != "")
                {
                    var map = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
                    config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                }
                else
                {
                    config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
                if (config.AppSettings.Settings[key] != null)
                { config.AppSettings.Settings[key].Value = value; }
                else { config.AppSettings.Settings.Add(key, value); }
                config.Save();
                return true;
            }
            catch { return false; }
        }
    }
}
