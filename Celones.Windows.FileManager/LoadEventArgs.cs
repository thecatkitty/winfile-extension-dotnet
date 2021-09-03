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
        public Vanara.PInvoke.HMENU MenuHandle { get; set; }
    }
}
    