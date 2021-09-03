using System;
using Vanara.PInvoke;

namespace Celones.Windows.FileManager
{
    public class MenuInitializeEventArgs : EventArgs
    {
        public MenuInitializeEventArgs(HMENU menu)
        {
            Menu = menu;
        }

        public HMENU Menu { get; }
    }
}
