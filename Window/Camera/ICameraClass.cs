using System;
namespace Cocon90.Lib.Util.Window.Camera
{
    public interface ICameraClass
    {
        bool SaveImage(string path);
        bool Start();
        bool StartRecord(string path);
        bool Stop();
        bool StopRecord();
    }
}
