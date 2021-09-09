using Vanara.PInvoke;
using System;
using System.Runtime.InteropServices;
using System.Linq;
// ReSharper disable InconsistentNaming

namespace Celones.Windows.FileManager.SampleExtension
{
    public class SampleExtension : FileManagerExtension
    {
        private const int IDM_FIRSTBUTTON = 1;
        private const int IDM_TESTMENU = 2;
        private const int IDM_TOGGLE = 8;

        private static readonly SampleExtension s_extension = new();

        [UnmanagedCallersOnly(EntryPoint = "FMExtensionProcW")]
        public static int EntryPoint(IntPtr hWnd, IntPtr wEvent, IntPtr lParam)
            => s_extension.ExtensionProc(hWnd, wEvent, lParam);

        private readonly HINSTANCE _instance;
        private uint _menuDelta;
        private bool _toggled;

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

        protected override bool OnHelpString(HelpStringEventArgs e)
        {
            e.Help = e.CommandId switch
            {
                -1 => "The mouse is over the menu item (Extension).",
                _ => $"Tooltip for item {e.CommandId}"
            };
            return base.OnHelpString(e);
        }

        protected override bool OnContextHelp(ContextHelpEventArgs e)
        {
            User32.MessageBox(e.Window, $"Help for {e.CommandId}", "WinHelp call", User32.MB_FLAGS.MB_OK);
            // User32.WinHelp(e.Window, "ExtHelp.hlp", User32.HelpCmd.HELP_CONTEXT, (IntPtr)e.CommandId);
            return base.OnContextHelp(e);
        }

        protected override int OnCommand(CommandEventArgs e)
        {
            switch (e.CommandId)
            {
                case IDM_FIRSTBUTTON:
                {
                    User32.MessageBox(e.Window, $"Focus is on {Focus} area.", "Test-Plugin", User32.MB_FLAGS.MB_OK);
                    break;
                }

                case IDM_TESTMENU:
                {
                    User32.MessageBox(e.Window, "Hi test!", "IDM_TESTMENU", User32.MB_FLAGS.MB_OK);
                    break;
                }

                case IDM_TOGGLE:
                {
                    User32.MessageBox(e.Window, _toggled ? "Hi On" : "Hi Off", "IDM_TOGGLE", User32.MB_FLAGS.MB_OK);
                    _toggled = !_toggled;
                    break;
                }

                default:
                {
                    User32.MessageBox(e.Window, $"Unrecognized idm: {e.CommandId}", "Error", User32.MB_FLAGS.MB_OK);
                    _toggled = !_toggled;
                    return base.OnCommand(e);
                }
            }

            return 0;
        }
    }
}
