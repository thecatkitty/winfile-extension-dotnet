using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Celones.Windows.FileManager.SampleExtension
{
    class Interop
    {
        [DllImport("ext-ms-win-ntuser-menu-l1-1-3")]
        public extern static IntPtr LoadMenuW(IntPtr hInstance, IntPtr lpMenuName);
    }
}
