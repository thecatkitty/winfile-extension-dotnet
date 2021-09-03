using System;
using Vanara.PInvoke;

namespace Celones.Windows.FileManager
{
    public class HelpStringEventArgs : EventArgs
    {
        public HelpStringEventArgs(int commandId, HMENU menu)
        {
            CommandId = commandId;
            Menu = menu;
        }

        public int CommandId { get; }
        public HMENU Menu { get; }
        public string Help { get; set; }
    }
}
