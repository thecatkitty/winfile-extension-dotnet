using System;
using System.IO;
using Vanara.Extensions;

namespace Celones.Windows.FileManager
{
    public class FileInfo
    {
        private readonly Interop.FMS_GETFILESELW _info;

        internal FileInfo(Interop.FMS_GETFILESELW info)
        {
            _info = info;
        }

        public FileAttributes Attributes => (FileAttributes)_info.bAttr;
        public DateTime CreationTime => _info.ftTime.ToDateTime();
        public string FullName => _info.szName;
        public long Length => _info.dwSize;
    }
}
