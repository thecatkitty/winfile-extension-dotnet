using System;

namespace Celones.Windows.FileManager
{
    public class FileManagerEventArgs : EventArgs
    {
        public FileManagerEventArgs(FileManagerHost host)
        {
            Host = host;
        }

        public FileManagerHost Host { get; }
    }
}
