using System;
using Vanara.PInvoke;

namespace Celones.Windows.FileManager
{
    public class ContextHelpEventArgs : FileManagerEventArgs
    {
        public ContextHelpEventArgs(FileManagerHost host, ushort commandId) : base(host)
        {
            CommandId = commandId;
        }

        public ushort CommandId { get; }
    }
}
