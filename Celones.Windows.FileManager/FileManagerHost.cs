using System;
using Vanara.PInvoke;
using static Celones.Windows.FileManager.Interop;

namespace Celones.Windows.FileManager
{
    public class FileManagerHost
    {
        private readonly HWND _hWnd;

        public FileManagerHost(HWND hWnd)
        {
            _hWnd = hWnd;
        }

        public FocusTarget Focus => (FocusTarget)User32.SendMessage(_hWnd, FM_GETFOCUS);
        public int SelectionCountShortNames => (int)User32.SendMessage(_hWnd, FM_GETSELCOUNT);
        public int SelectionCount => (int)User32.SendMessage(_hWnd, FM_GETSELCOUNTLFN);

        public void Refresh(bool all = false) => User32.SendMessage(_hWnd, FM_REFRESH_WINDOWS, IntPtr.Zero, all ? 1 : 0);

        public void ReloadExtensions() => User32.SendMessage(_hWnd, FM_RELOAD_EXTENSIONS);
    }
}
