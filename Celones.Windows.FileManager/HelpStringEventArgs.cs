using System;
using Vanara.PInvoke;

namespace Celones.Windows.FileManager
{
    public class HelpStringEventArgs : FileManagerEventArgs
    {
        public HelpStringEventArgs(FileManagerHost host, int commandId, HMENU menu) : base(host)
        {
            CommandId = commandId;
            Menu = menu;
        }

        public int CommandId { get; }
        public HMENU Menu { get; }
        public string Help { get; set; }
    }
}
