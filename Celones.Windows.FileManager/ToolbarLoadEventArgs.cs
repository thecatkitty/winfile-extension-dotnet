using System;
using System.Collections.Generic;

namespace Celones.Windows.FileManager
{
    public enum ToolbarButtonStyle
    {
        Normal = 0,
        Separator = 1,
        Toggle = 2
    }

    public class ToolbarButton
    {
        public ushort CommandId { get; set; }
        public ushort HelpId { get; set; }
        public ToolbarButtonStyle Style { get; set; }
    }

    public class ToolbarLoadEventArgs : EventArgs
    {
        public IList<ToolbarButton> Buttons { get; internal set; } = new List<ToolbarButton>();
        public ushort BitmappedCount { get; set; }
        public Vanara.PInvoke.HBITMAP BitmapHandle { get; set; }
        public ushort BitmapId { get; set; }
    }
}
