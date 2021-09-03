using System;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace Celones.Windows.FileManager
{
    internal class Interop
    {
        public const int WM_USER = 0x0400;

        public const int MENU_TEXT_LEN = 40;

        public const int FMMENU_FIRST = 1;
        public const int FMMENU_LAST = 99;

        public const int FMEVENT_LOAD = 100;
        public const int FMEVENT_UNLOAD = 101;
        public const int FMEVENT_INITMENU = 102;
        public const int FMEVENT_USER_REFRESH = 103;
        public const int FMEVENT_SELCHANGE = 104;
        public const int FMEVENT_TOOLBARLOAD = 105;
        public const int FMEVENT_HELPSTRING = 106;
        public const int FMEVENT_HELPMENUITEM = 107;

        public const int FMFOCUS_DIR = 1;
        public const int FMFOCUS_TREE = 2;
        public const int FMFOCUS_DRIVES = 3;
        public const int FMFOCUS_SEARCH = 4;

        public const int FM_GETFOCUS = (WM_USER + 0x0200);
        public const int FM_GETDRIVEINFOA = (WM_USER + 0x0201);
        public const int FM_GETSELCOUNT = (WM_USER + 0x0202);
        public const int FM_GETSELCOUNTLFN = (WM_USER + 0x0203);
        public const int FM_GETFILESELA = (WM_USER + 0x0204);
        public const int FM_GETFILESELLFNA = (WM_USER + 0x0205);
        public const int FM_REFRESH_WINDOWS = (WM_USER + 0x0206);
        public const int FM_RELOAD_EXTENSIONS = (WM_USER + 0x0207);
        public const int FM_GETDRIVEINFOW = (WM_USER + 0x0211);
        public const int FM_GETFILESELW = (WM_USER + 0x0214);
        public const int FM_GETFILESELLFNW = (WM_USER + 0x0215);

        public const int FM_GETDRIVEINFO = FM_GETDRIVEINFOW;
        public const int FM_GETFILESEL = FM_GETFILESELW;
        public const int FM_GETFILESELLFN = FM_GETFILESELLFNW;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct FMS_GETFILESELW
        {
            public System.Runtime.InteropServices.ComTypes.FILETIME ftTime;
            public uint dwSize;
            public byte bAttr;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string szName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct FMS_GETDRIVEINFOW
        {
            public uint dwTotalSpace;
            public uint dwFreeSpace;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string szPath;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)] public string szVolume;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string szShare;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct FMS_LOADW
        {
            public uint dwSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MENU_TEXT_LEN)] public string szMenuName;
            public IntPtr hMenu;
            public uint wMenuDelta;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct EXT_BUTTON
        {
            public ushort idCommand;
            public ushort idsHelp;
            public ushort fsStyle;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FMS_TOOLBARLOAD
        {
            public uint dwSize;
            public IntPtr lpButtons;
            public ushort cButtons;
            public ushort cBitmaps;
            public ushort idBitmap;
            public IntPtr hBitmap;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct FMS_HELPSTRINGW
        {
            public int idCommand;
            public IntPtr hMenu;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string szHelp;
        }

        public delegate uint FM_EXT_PROC(IntPtr hWnd, IntPtr wEvent, IntPtr lParam);
        public delegate uint FM_UNDELETE_PROC(IntPtr hWnd, [MarshalAs(UnmanagedType.LPTStr)] string lpStr);
    }
}
