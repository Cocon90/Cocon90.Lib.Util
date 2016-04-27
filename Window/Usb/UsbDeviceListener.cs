using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Cocon90.Lib.Util.Window.Usb
{
    /// <summary>
    /// USB存储设备监听类
    /// </summary>
    public class UsbDeviceListener
    {
        /// <summary>
        /// 实例化一个存储设备监听类
        /// </summary>
        public UsbDeviceListener()
        {
            backgroundworker.DoWork += backgroundworker_DoWork;
            backgroundworker.RunWorkerCompleted += backgroundworker_RunWorkerCompleted;
        }
        void backgroundworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as RemovableStatusModel;
            if (OnUsbRemovableStatusChanged != null)
            {
                OnUsbRemovableStatusChanged(result);
            }
        }
        /// <summary>
        /// 弹出驱动器，指定驱动器的盘符，如H盘，就输入"H" 返回是否弹出成功
        /// </summary>
        public bool PopupDevice(string driveLetter)
        {
            string filename = @"\\.\" + driveLetter;
            //打开设备，得到设备的句柄handle.
            IntPtr handle = CreateFile(filename, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, 0x3, 0, IntPtr.Zero);

            // 向目标设备发送设备控制码，也就是发送命令。IOCTL_STORAGE_EJECT_MEDIA  弹出U盘。
            uint byteReturned;
            bool result = DeviceIoControl(handle, IOCTL_STORAGE_EJECT_MEDIA, IntPtr.Zero, 0, IntPtr.Zero, 0, out byteReturned, IntPtr.Zero);
            return result;
        }
        /// <summary>
        /// 在Form对像的protected override void WndProc(ref Message m)方法中，加入的方法，用来起到检测USB作用。
        /// </summary>
        /// <param name="m"></param>
        public void WndProc(ref Message m)
        {
            if (m.Msg == WM_DEVICECHANGE)
            {
                switch (m.WParam.ToInt32())
                {
                    case DBT_DEVICEARRIVAL:
                        var dr = ds.Add();//通知存到记录中
                        if (dr == null) break;
                        if (OnUsbRemovableStatusChanged != null)
                        {
                            backgroundworker.RunWorkerAsync(dr);
                        }
                        break;
                    case DBT_DEVICEREMOVECOMPLETE:
                        var removedDrive = ds.Remove();//通知存到记录中
                        if (removedDrive == null) break;
                        if (OnUsbRemovableStatusChanged != null)
                        { backgroundworker.RunWorkerAsync(removedDrive); }
                        break;
                    default:
                        break;
                }
            }
            m.Result = IntPtr.Zero;
        }

        void backgroundworker_DoWork(object sender, DoWorkEventArgs e)
        {
            var result = e.Argument as RemovableStatusModel;
            if (result != null)
            {
                if (result.IsConnectionToPc)//若是插上到PC端
                {
                    result.SerialNumber = new USBSerialNumber().getSerialNumberFromDriveLetter(result.DeviceName.TrimEnd('\\'));
                }
                else
                {//否则从表中去查
                    result.SerialNumber = ds.GetSerialNumberByDeviceName(result.DeviceName);
                    ds.DeleteDeviceFormList(result);
                }

            }
            e.Result = result;
        }
        DeviceSteck ds = new DeviceSteck();

        string panfu = "";
        private const int WM_DEVICECHANGE = 0x219;//U盘插入后，OS的底层会自动检测到，然后向应用程序发送“硬件设备状态改变“的消息
        private const int DBT_DEVICEARRIVAL = 0x8000;  //就是用来表示U盘可用的。一个设备或媒体已被插入一块，现在可用。
        private const int DBT_CONFIGCHANGECANCELED = 0x0019;  //要求更改当前的配置（或取消停靠码头）已被取消。
        private const int DBT_CONFIGCHANGED = 0x0018;  //当前的配置发生了变化，由于码头或取消固定。
        private const int DBT_CUSTOMEVENT = 0x8006; //自定义的事件发生。 的Windows NT 4.0和Windows 95：此值不支持。
        private const int DBT_DEVICEQUERYREMOVE = 0x8001;  //审批要求删除一个设备或媒体作品。任何应用程序也不能否认这一要求，并取消删除。
        private const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;  //请求删除一个设备或媒体片已被取消。
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;  //一个设备或媒体片已被删除。
        private const int DBT_DEVICEREMOVEPENDING = 0x8003;  //一个设备或媒体一块即将被删除。不能否认的。
        private const int DBT_DEVICETYPESPECIFIC = 0x8005;  //一个设备特定事件发生。
        private const int DBT_DEVNODES_CHANGED = 0x0007;  //一种设备已被添加到或从系统中删除。
        private const int DBT_QUERYCHANGECONFIG = 0x0017;  //许可是要求改变目前的配置（码头或取消固定）。
        private const int DBT_USERDEFINED = 0xFFFF;  //此消息的含义是用户定义的
        private const uint GENERIC_READ = 0x80000000;
        private const int GENERIC_WRITE = 0x40000000;
        private const int FILE_SHARE_READ = 0x1;
        private const int FILE_SHARE_WRITE = 0x2;
        private const int IOCTL_STORAGE_EJECT_MEDIA = 0x2d4808;
        /// <summary>
        /// USB可移动存储设备改变委托
        /// </summary>
        /// <param name="status"></param>
        public delegate void UsbRemovableStatusChanged(RemovableStatusModel status);
        /// <summary>
        /// 事件 USB可移动存储设备改变时发生
        /// </summary>
        public event UsbRemovableStatusChanged OnUsbRemovableStatusChanged;
        private BackgroundWorker backgroundworker = new BackgroundWorker();


        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr CreateFile(
         string lpFileName,
         uint dwDesireAccess,
         uint dwShareMode,
         IntPtr SecurityAttributes,
         uint dwCreationDisposition,
         uint dwFlagsAndAttributes,
         IntPtr hTemplateFile);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped
        );
    }
    /// <summary>
    /// 取得驱动器硬件序列号信息的类。
    /// </summary>
    public  class USBSerialNumber
    {

        string _serialNumber;
        string _driveLetter;
        /// <summary>
        /// 获取驱动器硬件序列号，转入盘符，比如I:\盘就传"I"
        /// </summary>
        /// <param name="driveLetter"></param>
        /// <returns></returns>
        public string getSerialNumberFromDriveLetter(string driveLetter)
        {
            this._driveLetter = driveLetter.ToUpper();

            if (!this._driveLetter.Contains(":"))
            {
                this._driveLetter += ":";
            }

            matchDriveLetterWithSerial();

            return this._serialNumber;
        }

        private void matchDriveLetterWithSerial()
        {

            string[] diskArray;
            string driveNumber;
            string driveLetter;

            ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDiskToPartition");
            foreach (ManagementObject dm in searcher1.Get())
            {
                diskArray = null;
                driveLetter = getValueInQuotes(dm["Dependent"].ToString());
                diskArray = getValueInQuotes(dm["Antecedent"].ToString()).Split(',');
                driveNumber = diskArray[0].Remove(0, 6).Trim();
                if (driveLetter == this._driveLetter)
                {
                    /* This is where we get the drive serial */
                    ManagementObjectSearcher disks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                    foreach (ManagementObject disk in disks.Get())
                    {

                        if (disk["Name"].ToString() == ("\\\\.\\PHYSICALDRIVE" + driveNumber) & disk["InterfaceType"].ToString() == "USB")
                        {
                            this._serialNumber = parseSerialFromDeviceID(disk["PNPDeviceID"].ToString());
                        }
                    }
                }
            }
        }

        private string parseSerialFromDeviceID(string deviceId)
        {
            string[] splitDeviceId = deviceId.Split('\\');
            string[] serialArray;
            string serial;
            int arrayLen = splitDeviceId.Length - 1;

            serialArray = splitDeviceId[arrayLen].Split('&');
            serial = serialArray[0];

            return serial;
        }

        private string getValueInQuotes(string inValue)
        {
            string parsedValue = "";

            int posFoundStart = 0;
            int posFoundEnd = 0;

            posFoundStart = inValue.IndexOf("\"");
            posFoundEnd = inValue.IndexOf("\"", posFoundStart + 1);

            parsedValue = inValue.Substring(posFoundStart + 1, (posFoundEnd - posFoundStart) - 1);

            return parsedValue;
        }

    }
    /// <summary>
    /// 用来记忆插入的设备和拔出的设备的一个类
    /// </summary>
    class DeviceSteck
    {
        List<RemovableStatusModel> ListAll = new List<RemovableStatusModel>();

        /// <summary>
        /// 插入设备时调用此方法。
        /// </summary>
        /// <param name="usb"></param>
        public RemovableStatusModel Add()
        {
            var drivers = System.IO.DriveInfo.GetDrives();
            foreach (var item in drivers)
            {
                if (item.DriveType == DriveType.Removable)
                {
                    if (!ListAll.Exists(u => u.DeviceName == item.Name))
                    {
                        var addModel = new RemovableStatusModel { DeviceInfo = item, DateTime = DateTime.Now, DeviceName = item.Name, IsConnectionToPc = true };
                        ListAll.Add(addModel);
                        return addModel;
                    }
                }
            }
            return null;
        }
        /// <summary>
        ///  拔出设备时调用此方法，可以返回被拔掉的设备的信息
        /// </summary>
        /// <returns></returns>
        public RemovableStatusModel Remove()
        {
            var drivers = System.IO.DriveInfo.GetDrives();
            foreach (var item in ListAll)
            {
                if (!drivers.ToList().Exists((u) => u.Name == item.DeviceName))
                {

                    return new RemovableStatusModel { DateTime = item.DateTime, DeviceInfo = item.DeviceInfo, DeviceName = item.DeviceName, SerialNumber = item.SerialNumber, IsConnectionToPc = false };
                }
            }
            return null;
        }
        /// <summary>
        /// 从列表中移除
        /// </summary>
        /// <param name="item"></param>
        public void DeleteDeviceFormList(RemovableStatusModel item)
        {
            try
            {
                ListAll.RemoveAt(ListAll.FindIndex((u) => u.DeviceName == item.DeviceName));
            }
            catch { }
        }
        /// <summary>
        /// 通过名称，取得设备信息。
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public string GetSerialNumberByDeviceName(string serialNumber)
        {
            var m = ListAll.Find(rsm => rsm.DeviceName == serialNumber);
            if (m != null)
            {
                return m.SerialNumber;
            }
            return "";
        }
    }
    public class RemovableStatusModel
    {
        /// <summary>
        /// True:连接到PC，False:从PC上弹出。
        /// </summary>
        public bool IsConnectionToPc { get; set; }
        /// <summary>
        /// 驱动器设备信息
        /// </summary>
        public DriveInfo DeviceInfo { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 设备状态发现时间
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// 设备序列号
        /// </summary>
        public string SerialNumber { get; set; }
    }
}
