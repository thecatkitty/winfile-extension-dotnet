using System;

namespace Celones.Windows.FileManager
{
    public class CommandEventArgs : EventArgs
    {
        public CommandEventArgs(Vanara.PInvoke.HWND hWnd, int command)
        {
            Command = command;
            Window = hWnd;
            ReturnValue = 0;
        }

        public int Command { get; }
        public Vanara.PInvoke.HWND Window { get; }
        public int ReturnValue { get; set; }
    }
}
