using System;
using System.Runtime.InteropServices;
using static Celones.Windows.FileManager.Interop;

namespace Celones.Windows.FileManager
{
    static class EventArgsConverters
    {
        public static (LoadEventArgs, FMS_LOADW) GetLoadEventArgs(FileManagerHost host, IntPtr ptr)
        {
            var load = Marshal.PtrToStructure<FMS_LOADW>(ptr);
            var e = new LoadEventArgs(host, load.wMenuDelta);
            return (e, load);
        }

        public static MenuInitializeEventArgs GetMenuInitializeEventArgs(FileManagerHost host, IntPtr ptr) => new(host, ptr);

        public static (ToolbarLoadEventArgs, FMS_TOOLBARLOAD) GetToolbarLoadEventArgs(FileManagerHost host, IntPtr ptr)
        {
            var load = Marshal.PtrToStructure<FMS_TOOLBARLOAD>(ptr);
            var e = new ToolbarLoadEventArgs(host);
            return (e, load);
        }

        public static (HelpStringEventArgs, FMS_HELPSTRINGW) GetHelpStringEventArgs(FileManagerHost host, IntPtr ptr)
        {
            var help = Marshal.PtrToStructure<FMS_HELPSTRINGW>(ptr);
            var e = new HelpStringEventArgs(host, help.idCommand, help.hMenu);
            return (e, help);
        }

        public static void Update(ref FMS_LOADW load, LoadEventArgs e)
        {
            load.dwSize = (uint)Marshal.SizeOf(load);
            load.szMenuName = e.MenuName;
            load.hMenu = (IntPtr)e.MenuHandle;
        }

        public static void Update(ref FMS_TOOLBARLOAD load, ToolbarLoadEventArgs e)
        {
            var buttonPointer = load.lpButtons;
            foreach (var button in e.Buttons)
            {
                Marshal.StructureToPtr(Convert(button), buttonPointer, true);
                buttonPointer += Marshal.SizeOf<EXT_BUTTON>();
            }

            load.dwSize = (uint)Marshal.SizeOf(load);
            load.cButtons = (ushort)e.Buttons.Count;
            load.cBitmaps = e.BitmappedCount;
            load.idBitmap = e.BitmapHandle.IsNull ? e.BitmapId : (ushort)0;
            load.hBitmap = (IntPtr)e.BitmapHandle;
        }

        public static void Update(ref FMS_HELPSTRINGW help, HelpStringEventArgs e)
        {
            help.szHelp = e.Help;
        }

        public static void Set<T>(IntPtr ptr, T data) => Marshal.StructureToPtr(data, ptr, true);

        private static EXT_BUTTON Convert(ToolbarButton button) => new()
        {
            idCommand = button.CommandId,
            idsHelp = button.HelpId,
            fsStyle = (ushort)button.Style
        };
    }
}
