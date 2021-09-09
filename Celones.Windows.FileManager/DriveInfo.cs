namespace Celones.Windows.FileManager
{
    public class DriveInfo
    {
        private readonly Interop.FMS_GETDRIVEINFOW _info;

        internal DriveInfo(Interop.FMS_GETDRIVEINFOW info)
        {
            _info = info;
        }

        public long AvailableFreeSpace => _info.dwFreeSpace;
        public string CurrentDirectory => _info.szPath;
        public string NetworkShare => _info.szShare;
        public long TotalSize => _info.dwTotalSpace;
        public string VolumeLabel => _info.szVolume;
    }
}
