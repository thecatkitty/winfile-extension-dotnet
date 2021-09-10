using System;
using Vanara.PInvoke;

namespace Celones.Windows.FileManager
{
    public class MenuInitializeEventArgs : FileManagerEventArgs
    {
        public MenuInitializeEventArgs(FileManagerHost host, HMENU menu) : base(host)
        {
            Menu = menu;
        }

        public HMENU Menu { get; }
    }
}
