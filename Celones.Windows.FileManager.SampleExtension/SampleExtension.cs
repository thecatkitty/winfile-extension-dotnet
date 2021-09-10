using Vanara.PInvoke;
using System;
using System.Runtime.InteropServices;
using System.Linq;
// ReSharper disable InconsistentNaming

namespace Celones.Windows.FileManager.SampleExtension
{
    public static class SampleExtension
    {
        private const int IDM_FIRSTBUTTON = 1;
        private const int IDM_TESTMENU = 2;
        private const int IDM_TOGGLE = 8;

        private static readonly FileManagerExtension s_extension;
        private static readonly HINSTANCE s_instance;
        private static uint s_menuDelta;
        private static bool s_toggled;

        [UnmanagedCallersOnly(EntryPoint = "FMExtensionProcW")]
        public static int EntryPoint(IntPtr hWnd, IntPtr wEvent, IntPtr lParam)
            => s_extension.ExtensionProc(hWnd, wEvent, lParam);

        static SampleExtension()
        {
            var name = typeof(SampleExtension).Namespace + ".dll";
            s_instance = Kernel32.EnumProcessModules(Kernel32.GetCurrentProcess())
                .First(module => Kernel32.GetModuleFileName(module).EndsWith(name));

            s_extension = new();
            s_extension.Load += LoadEventHandler;
            s_extension.MenuInitialize += MenuInitializeEventHandler;
            s_extension.ToolbarLoad += ToolbarLoadEventHandler;
            s_extension.HelpString += HelpStringEventHandler;
            s_extension.ContextHelp += ContextHelpEventHandler;
            s_extension.Command += CommandEventHandler;
        }

        private static void LoadEventHandler(object sender, LoadEventArgs e)
        {
            e.MenuName = "AddonSampleMenu";
            e.MenuHandle = User32.GetSubMenu(User32.LoadMenu(s_instance, 101), 0);
            s_menuDelta = e.MenuDelta;
        }

        private static void MenuInitializeEventHandler(object sender, MenuInitializeEventArgs e)
        {
            User32.CheckMenuItem(
                hMenu: e.Menu,
                uIDCheckItem: s_menuDelta + IDM_TOGGLE,
                uCheck: s_toggled
                ? User32.MenuFlags.MF_BYCOMMAND | User32.MenuFlags.MF_CHECKED
                : User32.MenuFlags.MF_BYCOMMAND | User32.MenuFlags.MF_UNCHECKED);
        }

        private static void ToolbarLoadEventHandler(object sender, ToolbarLoadEventArgs e)
        {
            e.Buttons.Add(new ToolbarButton { CommandId = IDM_FIRSTBUTTON });
            e.BitmappedCount = 1;
            e.BitmapId = 102;
        }

        private static void HelpStringEventHandler(object sender, HelpStringEventArgs e)
        {
            e.Help = e.CommandId switch
            {
                -1 => "The mouse is over the menu item (Extension).",
                _ => $"Tooltip for item {e.CommandId}"
            };
        }

        private static void ContextHelpEventHandler(object sender, ContextHelpEventArgs e)
        {
            User32.MessageBox(e.Host.Window, $"Help for {e.CommandId}", "WinHelp call", User32.MB_FLAGS.MB_OK);
            // User32.WinHelp(e.Window, "ExtHelp.hlp", User32.HelpCmd.HELP_CONTEXT, (IntPtr)e.CommandId);
        }

        private static void CommandEventHandler(object sender, CommandEventArgs e)
        {
            switch (e.CommandId)
            {
                case IDM_FIRSTBUTTON:
                {
                    User32.MessageBox(e.Host.Window, $"Focus is on {e.Host.Focus} area.", "Test-Plugin", User32.MB_FLAGS.MB_OK);

                    if (e.Host.Focus == FocusTarget.Directory)
                    {
                        foreach (var file in e.Host.SelectedFiles)
                        {
                            User32.MessageBox(e.Host.Window, file.FullName, "Selected file", User32.MB_FLAGS.MB_OK);
                        }
                    }
                    else if (e.Host.Focus == FocusTarget.Drives)
                    {
                        User32.MessageBox(e.Host.Window, e.Host.SelectedDrive.CurrentDirectory, "Current directory", User32.MB_FLAGS.MB_OK);
                    }

                    break;
                }

                case IDM_TESTMENU:
                {
                    User32.MessageBox(e.Host.Window, "Hi test!", "IDM_TESTMENU", User32.MB_FLAGS.MB_OK);
                    break;
                }

                case IDM_TOGGLE:
                {
                    User32.MessageBox(e.Host.Window, s_toggled ? "Hi On" : "Hi Off", "IDM_TOGGLE", User32.MB_FLAGS.MB_OK);
                    s_toggled = !s_toggled;
                    break;
                }

                default:
                {
                    User32.MessageBox(e.Host.Window, $"Unrecognized idm: {e.CommandId}", "Error", User32.MB_FLAGS.MB_OK);
                    e.ReturnValue = -1;
                    break;
                }
            }
        }
    }
}
