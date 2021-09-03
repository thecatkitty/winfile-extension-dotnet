using System;

namespace Celones.Windows.FileManager
{
    public class LoadEventArgs : EventArgs
    {
        public LoadEventArgs(uint menuDelta)
        {
            MenuDelta = menuDelta;
        }

        public uint MenuDelta { get; }
        public string MenuName { get; set; }
        public IntPtr MenuHandle { get; set; }
    }
}
    