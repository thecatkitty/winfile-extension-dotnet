using System;
using System.Runtime.InteropServices;

namespace Celones.Windows.FileManager
{
    public abstract class FileManagerExtension
    {
        public event EventHandler<LoadEventArgs> Load;
        public event EventHandler Unload;
        public event EventHandler<MenuInitializeEventArgs> MenuInitialize;
        public event EventHandler<ToolbarLoadEventArgs> ToolbarLoad;

        private IntPtr _buttons;

        public int ExtensionProc(IntPtr hWnd, IntPtr wEvent, IntPtr lParam)
        {
            switch ((int)wEvent)
            {
                case Interop.FMEVENT_LOAD:
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

                case Interop.FMEVENT_UNLOAD:
                    return OnUnload() ? 0 : -1;

                case Interop.FMEVENT_INITMENU:
                    return OnMenuInitialize(new MenuInitializeEventArgs(lParam)) ? 0 : -1;

                case Interop.FMEVENT_TOOLBARLOAD:
                {
                    var load = Marshal.PtrToStructure<Interop.FMS_TOOLBARLOAD>(lParam);
                    var e = new ToolbarLoadEventArgs();
                    if (!OnToolbarLoad(e)) return 0;

                    if (_buttons == IntPtr.Zero)
                    {
                        _buttons = Marshal.AllocHGlobal(e.Buttons.Count * Marshal.SizeOf<Interop.EXT_BUTTON>());
                    }

                    for (var i = 0; i < e.Buttons.Count; i++)
                    {
                        Interop.EXT_BUTTON button;
                        button.idCommand = e.Buttons[i].CommandId;
                        button.idsHelp = e.Buttons[i].HelpId;
                        button.fsStyle = (ushort)e.Buttons[i].Style;
                        Marshal.StructureToPtr(button, _buttons + i * Marshal.SizeOf(button), true);
                    }

                    load.dwSize = (uint)Marshal.SizeOf(load);
                    load.lpButtons = _buttons;
                    load.cButtons = (ushort)e.Buttons.Count;
                    load.cBitmaps = e.BitmappedCount;
                    load.idBitmap = e.BitmapHandle.IsNull ? e.BitmapId : (ushort)0;
                    load.hBitmap = (IntPtr)e.BitmapHandle;
                    Marshal.StructureToPtr(load, lParam, true);
                    return 1;
                }

                default:
                    return 0;
            }
        }

        ~FileManagerExtension()
        {
            if (_buttons == IntPtr.Zero) return;
            Marshal.FreeHGlobal(_buttons);
            _buttons = IntPtr.Zero;
        }

        protected virtual bool OnLoad(LoadEventArgs e)
        {
            Load?.Invoke(this, e);
            return true;
        }

        protected virtual bool OnUnload()
        {
            Unload?.Invoke(this, EventArgs.Empty);
            return true;
        }

        protected virtual bool OnMenuInitialize(MenuInitializeEventArgs e)
        {
            MenuInitialize?.Invoke(this, e);
            return true;
        }

        protected virtual bool OnToolbarLoad(ToolbarLoadEventArgs e)
        {
            ToolbarLoad?.Invoke(this, e);
            return e.Buttons.Count > 0;
        }
    }
}
