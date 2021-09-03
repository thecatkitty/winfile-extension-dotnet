using System;
using System.Runtime.InteropServices;

namespace Celones.Windows.FileManager.SampleExtension
{
    public class SampleExtension : FileManagerExtension
    {
        private static SampleExtension _extension = new();

        [UnmanagedCallersOnly(EntryPoint = "FMExtensionProcW")]
        public static int EntryPoint(IntPtr hWnd, IntPtr wEvent, IntPtr lParam)
            => _extension.ExtensionProc(hWnd, wEvent, lParam);

        protected override bool OnLoad(LoadEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show(
                text: "Hello World!",
                caption: "Windows File Manager managed extension",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Exclamation);
            return true;
        }
    }
}
