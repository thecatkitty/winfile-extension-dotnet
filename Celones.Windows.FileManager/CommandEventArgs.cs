using System;

namespace Celones.Windows.FileManager
{
    public class CommandEventArgs : FileManagerEventArgs
    {
        public CommandEventArgs(FileManagerHost host, int commandId) : base(host)
        {
            CommandId = commandId;
            ReturnValue = 0;
        }

        public int CommandId { get; }
        public int ReturnValue { get; set; }
    }
}
