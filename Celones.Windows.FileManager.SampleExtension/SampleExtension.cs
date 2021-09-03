using Vanara.PInvoke;
using System;
using System.Runtime.InteropServices;
using System.Linq;

namespace Celones.Windows.FileManager.SampleExtension
{
    public class SampleExtension : FileManagerExtension
    {
        private static SampleExtension _extension = new();

        [UnmanagedCallersOnly(EntryPoint = "FMExtensionProcW")]
        public static int EntryPoint(IntPtr hWnd, IntPtr wEvent, IntPtr lParam)
            => _extension.ExtensionProc(hWnd, wEvent, lParam);

        private readonly HINSTANCE _hInstance;

        public SampleExtension()
        {
            var name = typeof(SampleExtension).Namespace + ".dll";
            _hInstance = Kernel32.EnumProcessModules(Kernel32.GetCurrentProcess())
                .First(module => Kernel32.GetModuleFileName(module).EndsWith(name));
        }

        protected override bool OnLoad(LoadEventArgs e)
        {
            e.MenuName = "AddonSampleMenu";
            e.MenuHandle = User32.GetSubMenu(User32.LoadMenu(_hInstance, 101), 0);

            base.OnLoad(e);
            return true;
        }

        protected override bool OnUnload()
        {
            base.OnUnload();
            return true;
        }
    }
}
