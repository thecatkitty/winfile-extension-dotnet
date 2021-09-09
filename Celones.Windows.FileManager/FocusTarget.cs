namespace Celones.Windows.FileManager
{
    public enum FocusTarget
    {
        Directory = Interop.FMFOCUS_DIR,
        Tree = Interop.FMFOCUS_TREE,
        Drives = Interop.FMFOCUS_DRIVES,
        Search = Interop.FMFOCUS_SEARCH
    }
}