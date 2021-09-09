using System;

namespace Celones.Windows.FileManager
{
    public class CommandEventArgs : EventArgs
    {
        public CommandEventArgs(Vanara.PInvoke.HWND hWnd, int commandId)
        {
            CommandId = commandId;
            Window = hWnd;
            ReturnValue = 0;
        }

        public int CommandId { get; }
        public Vanara.PInvoke.HWND Window { get; }
        public int ReturnValue { get; set; }
    }
}
