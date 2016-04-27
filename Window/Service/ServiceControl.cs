using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Cocon90.Lib.Util.Window.Service
{
    /// <summary>
    /// 服务控制类，依靠ServiceHelper类，进行安装和卸载服务
    /// </summary>
    public class ServiceControl
    {

        public string ServiceDiscription { get; set; }
        public string ServiceName { get; set; }
        public string ServicePath { get; set; }
        /// <summary>
        /// 服务名称，服务主程序全路径，服务描述
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="servicePath"></param>
        /// <param name="serviceDiscription"></param>
        public ServiceControl(string serviceName, string servicePath, string serviceDiscription)
        {
            this.ServiceName = serviceName;
            this.ServicePath = servicePath;
            this.ServiceDiscription = serviceDiscription;
        }
        /// <summary>
        /// 服务名称，服务主程序全路径
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="servicePath"></param>
        public ServiceControl(string serviceName, string servicePath)
            : this(serviceName, servicePath, "无描述信息！")
        { }
        public bool IsServiceExisted()
        {
            return IsServiceExisted(ServiceName);
        }
        public bool IsServiceExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName == serviceName)
                {
                    if (s.Status != ServiceControllerStatus.Running)
                    {
                        try
                        {
                            s.Start();
                        }
                        catch { }
                    }
                    return true;
                }
            }
            return false;
        }
        public bool IsServiceExistedAndRunning()
        {
            return IsServiceExistedAndRunning(ServiceName);
        }
        public bool IsServiceExistedAndRunning(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName == serviceName)
                {
                    if (s.Status == ServiceControllerStatus.Running)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 停止并卸载
        /// </summary>
        public bool Stop()
        {
            try
            {
                ServiceHelper.StopService(ServiceName, TimeSpan.FromSeconds(30));
                ServiceHelper.Uninstall(ServiceName);
                return true;
            }
            catch { return false; }
        }
        /// <summary>
        /// 安装并启动
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            try
            {
                ServiceHelper.Uninstall(ServiceName);
                ServiceHelper.Install(ServiceName, ServiceName, ServicePath, ServiceDiscription, ServiceStartType.Auto);
                return ServiceHelper.StartService(ServiceName, TimeSpan.FromSeconds(30));
            }
            catch { return false; }
        }

        /// <summary>
        /// 如果未安装，则安装，如果未运行，则安装并启动，操作成功则返回True，失败则返回False。
        /// </summary>
        /// <returns></returns>
        public bool IntelligenceRun()
        {
            try
            {
                if (!IsServiceExisted())
                {
                    ServiceHelper.Install(ServiceName, ServiceName, ServicePath, ServiceDiscription, ServiceStartType.Auto);
                }
                if (!ServiceHelper.StartService(ServiceName, TimeSpan.FromSeconds(30)))
                {
                    return Start();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
