using System;
namespace Cocon90.Lib.Util.Server
{
    /// <summary>
    /// 表示一个服务。
    /// </summary>
    public interface IBaseServer
    {
        /// <summary>
        /// 获取或设置 服务是否正在运行。
        /// </summary>
        bool IsRunning { get; set; }
        /// <summary>
        /// 启动服务
        /// </summary>
        void Start();
        /// <summary>
        /// 停止服务
        /// </summary>
        void Stop();
    }
}
