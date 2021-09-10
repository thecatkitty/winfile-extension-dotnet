using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Vanara.PInvoke;
using static Celones.Windows.FileManager.Interop;

namespace Celones.Windows.FileManager
{
    public class FileManagerHost
    {
        public HWND Window { get; }

        public FileManagerHost(HWND hWnd)
        {
            Window = hWnd;
        }

        public FocusTarget Focus => (FocusTarget)User32.SendMessage(Window, FM_GETFOCUS);
        public int SelectionCountShortNames => (int)User32.SendMessage(Window, FM_GETSELCOUNT);
        public int SelectionCount => (int)User32.SendMessage(Window, FM_GETSELCOUNTLFN);

        public DriveInfo SelectedDrive
        {
            get
            {
                var buffer = Marshal.AllocHGlobal(Marshal.SizeOf<FMS_GETDRIVEINFOW>());
                User32.SendMessage(Window, FM_GETDRIVEINFO, IntPtr.Zero, (long)buffer);

                var info = Marshal.PtrToStructure<FMS_GETDRIVEINFOW>(buffer);
                Marshal.FreeHGlobal(buffer);
                return new DriveInfo(info);
            }
        }

        public IEnumerable<FileInfo> SelectedFiles
        {
            get
            {
                var count = SelectionCount;
                for (var index = 0; index < count; index++)
                {
                    yield return GetSelectedFile(index);
                }
            }
        }

        public FileInfo GetSelectedFileShortNames(int index)
        {
            var buffer = Marshal.AllocHGlobal(Marshal.SizeOf<FMS_GETFILESELW>());
            User32.SendMessage(Window, FM_GETFILESEL, (IntPtr)index, (long)buffer);

            var info = Marshal.PtrToStructure<FMS_GETFILESELW>(buffer);
            Marshal.FreeHGlobal(buffer);
            return new FileInfo(info);
        }

        public FileInfo GetSelectedFile(int index)
        {
            var buffer = Marshal.AllocHGlobal(Marshal.SizeOf<FMS_GETFILESELW>());
            User32.SendMessage(Window, FM_GETFILESELLFN, (IntPtr)index, (long)buffer);

            var info = Marshal.PtrToStructure<FMS_GETFILESELW>(buffer);
            Marshal.FreeHGlobal(buffer);
            return new FileInfo(info);
        }

        public void Refresh(bool all = false) => User32.SendMessage(Window, FM_REFRESH_WINDOWS, IntPtr.Zero, all ? 1 : 0);

        public void ReloadExtensions() => User32.SendMessage(Window, FM_RELOAD_EXTENSIONS);
    }
}
