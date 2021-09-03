using Vanara.PInvoke;
using System;
using System.Runtime.InteropServices;
using System.Linq;

namespace Celones.Windows.FileManager.SampleExtension
{
    public class SampleExtension : FileManagerExtension
    {
        private static readonly SampleExtension s_extension = new();

        [UnmanagedCallersOnly(EntryPoint = "FMExtensionProcW")]
        public static int EntryPoint(IntPtr hWnd, IntPtr wEvent, IntPtr lParam)
            => s_extension.ExtensionProc(hWnd, wEvent, lParam);

        private readonly HINSTANCE _instance;
        private uint _menuDelta;
        private bool _toggled = false;

        public SampleExtension()
        {
            var name = typeof(SampleExtension).Namespace + ".dll";
            _instance = Kernel32.EnumProcessModules(Kernel32.GetCurrentProcess())
                .First(module => Kernel32.GetModuleFileName(module).EndsWith(name));
        }

        protected override bool OnLoad(LoadEventArgs e)
        {
            e.MenuName = "AddonSampleMenu";
            e.MenuHandle = User32.GetSubMenu(User32.LoadMenu(_instance, 101), 0);
            _menuDelta = e.MenuDelta;

            return base.OnLoad(e);
        }

        protected override bool OnMenuInitialize(MenuInitializeEventArgs e)
        {
            User32.CheckMenuItem(
                hMenu: e.Menu,
                uIDCheckItem: _menuDelta + 8,
                uCheck: _toggled
                ? User32.MenuFlags.MF_BYCOMMAND | User32.MenuFlags.MF_CHECKED
                : User32.MenuFlags.MF_BYCOMMAND | User32.MenuFlags.MF_UNCHECKED);

            return base.OnMenuInitialize(e);
        }

        protected override bool OnToolbarLoad(ToolbarLoadEventArgs e)
        {
            e.Buttons.Add(new ToolbarButton { CommandId = 1 });
            e.BitmappedCount = 1;
            e.BitmapId = 102;
            return base.OnToolbarLoad(e);
        }
    }
}
