using System;
using System.Runtime.InteropServices;

namespace Celones.Windows.FileManager
{
    public abstract class FileManagerExtension
    {
        public int ExtensionProc(IntPtr hWnd, IntPtr wEvent, IntPtr lParam)
        {
            if ((int)wEvent == Interop.FMEVENT_LOAD)
            {
                var load = Marshal.PtrToStructure<Interop.FMS_LOADW>(lParam);
                var e = new LoadEventArgs(load.wMenuDelta);
                if (!OnLoad(e)) return 0;

                load.dwSize = (uint)Marshal.SizeOf(load);
                load.szMenuName = e.MenuName;
                load.hMenu = (IntPtr)e.MenuHandle;
                Marshal.StructureToPtr(load, lParam, true);
                return 1;
            }

            return 0;
        }

        protected virtual bool OnLoad(LoadEventArgs e) => false;
    }
}
