namespace Unity3DDisassembler.Unity
{
    public class CompressedFileHeader
    {
        public string Signature { get; set; }

        public int BuildVerison { get; set; }

        public string WebPlayerVersion { get; set; }

        public string UnityEngineVersion { get; set; }

        public int CompressedFileSize { get; set; }

        public int CompressedFileHeaderSize { get; set; }

        public int CompressedFileSizeWithoutHeader { get; set; } //I know pretty huge variable name. If you have a better name let me know. :)

        public int DecompressedFileSize { get; set; }

        public int CompressedFileSize2 { get; set; }

        public int DecompressedFileHeaderSize { get; set; } //Again. The huge variable name and don't forget if you have a better name let me know. :/

        public byte[] Bytes { get; set; }
    }
}
