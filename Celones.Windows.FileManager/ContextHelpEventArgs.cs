using System;
using Vanara.PInvoke;

namespace Celones.Windows.FileManager
{
    public class ContextHelpEventArgs : EventArgs
    {
        public ContextHelpEventArgs(HWND window, ushort commandId)
        {
            Window = window;
            CommandId = commandId;
        }

        public HWND Window { get; }
        public ushort CommandId { get; }
    }
}
