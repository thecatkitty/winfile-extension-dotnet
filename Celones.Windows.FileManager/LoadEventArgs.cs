using System;

namespace Celones.Windows.FileManager
{
    public class LoadEventArgs : FileManagerEventArgs
    {
        public LoadEventArgs(FileManagerHost host, uint menuDelta) : base(host)
        {
            MenuDelta = menuDelta;
        }

        public uint MenuDelta { get; }
        public string MenuName { get; set; }
        public Vanara.PInvoke.HMENU MenuHandle { get; set; }
    }
}
    