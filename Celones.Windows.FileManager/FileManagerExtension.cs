using System;
using System.Runtime.InteropServices;
using static Celones.Windows.FileManager.Interop;

namespace Celones.Windows.FileManager
{
    public abstract class FileManagerExtension
    {
        public event EventHandler<LoadEventArgs> Load;
        public event EventHandler Unload;
        public event EventHandler<MenuInitializeEventArgs> MenuInitialize;
        public event EventHandler<ToolbarLoadEventArgs> ToolbarLoad;
        public event EventHandler UserRefresh;
        public event EventHandler SelectionChanged;
        public event EventHandler<HelpStringEventArgs> HelpString;
        public event EventHandler<ContextHelpEventArgs> ContextHelp;
        public event EventHandler<CommandEventArgs> Command;

        protected FileManagerHost Host { get; private set; }

        private IntPtr _buttons;

        public int ExtensionProc(IntPtr hWnd, IntPtr wEvent, IntPtr lParam)
        {
            Host = new FileManagerHost(hWnd);
            switch ((int)wEvent)
            {
                case FMEVENT_LOAD:
                {
                    var (e, load) = EventArgsConverters.GetLoadEventArgs(lParam);
                    if (!OnLoad(e)) return 0;

                    EventArgsConverters.Update(ref load, e);
                    EventArgsConverters.Set(lParam, load);
                    return 1;
                }

                case FMEVENT_UNLOAD:
                    return OnUnload() ? 0 : -1;

                case FMEVENT_INITMENU:
                    return OnMenuInitialize(EventArgsConverters.GetMenuInitializeEventArgs(lParam)) ? 0 : -1;

                case FMEVENT_TOOLBARLOAD:
                {
                    var (e, load) = EventArgsConverters.GetToolbarLoadEventArgs(lParam);
                    if (!OnToolbarLoad(e)) return 0;

                    if (_buttons == IntPtr.Zero)
                    {
                        _buttons = Marshal.AllocHGlobal(e.Buttons.Count * Marshal.SizeOf<EXT_BUTTON>());
                    }
                    load.lpButtons = _buttons;

                    EventArgsConverters.Update(ref load, e);
                    EventArgsConverters.Set(lParam, load);
                    return 1;
                }

                case FMEVENT_USER_REFRESH:
                    return OnUserRefresh() ? 0 : -1;

                case FMEVENT_SELCHANGE:
                    return OnSelectionChanged() ? 0 : -1;

                case FMEVENT_HELPSTRING:
                {
                    var (e, help) = EventArgsConverters.GetHelpStringEventArgs(lParam);
                    if (!OnHelpString(e)) return -1;

                    EventArgsConverters.Update(ref help, e);
                    EventArgsConverters.Set(lParam, help);
                    return 0;
                }

                case FMEVENT_HELPMENUITEM:
                    return OnContextHelp(new ContextHelpEventArgs(hWnd, (ushort)lParam)) ? 0 : -1;

                default:
                    return OnCommand(new CommandEventArgs(hWnd, (int)wEvent));
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

        protected virtual bool OnUserRefresh()
        {
            UserRefresh?.Invoke(this, EventArgs.Empty);
            return true;
        }

        protected virtual bool OnSelectionChanged()
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }

        protected virtual bool OnHelpString(HelpStringEventArgs e)
        {
            HelpString?.Invoke(this, e);
            return true;
        }

        protected virtual bool OnContextHelp(ContextHelpEventArgs e)
        {
            ContextHelp?.Invoke(this, e);
            return true;
        }

        protected virtual int OnCommand(CommandEventArgs e)
        {
            Command?.Invoke(this, e);
            return e.ReturnValue;
        }
    }
}
