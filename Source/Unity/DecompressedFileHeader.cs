namespace Unity3DDisassembler.Unity
{
    public class DecompressedFileHeader
    {
        public int FileCount { get; set; }

        public byte[] Bytes { get; set; }

        public FileObject[] Files { get; set; }
    }
}
