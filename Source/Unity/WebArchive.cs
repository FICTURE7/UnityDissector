using System;
using System.Collections.Generic;
using System.IO;
using Unity3DDisassembler.Compression.LZMA;
using Unity3DDisassembler.IO;

namespace Unity3DDisassembler.Unity
{
    public class WebArchive
    {
        public WebArchive() { throw new NotImplementedException(); }

        public WebArchive(string path)
        {
            this.FilePath = path;
            this.FileName = Path.GetFileName(path);
        }

        public long CompressedSize { get; private set; }
        public long UncompressedSize { get; private set; }
        public string FilePath { get; private set; }
        public string FileName { get; private set; }
        public int FileCount { get { return Files.Length; } }
        public WebArchiveFile this[int index] { get { return Files[index]; } } //fancy stuff

        private WebArchiveFile[] Files { get; set; }
        private bool Opened { get; set; }

        public void OpenNew() // this stuff can generate a malformed .unity3d file that oblitarates your hard disk
        {
            var filesPath = GetFiles(Path.GetDirectoryName(FilePath));
            var files = new WebArchiveFile[filesPath.Length];
            var headerSize = 5; //int32

            //read files
            for (int i = 0; i < filesPath.Length; i++)
            {
                files[i] = new WebArchiveFile();
                files[i].Name = Path.GetFileName(filesPath[i]);
                files[i].Bytes = File.ReadAllBytes(filesPath[i]);
                files[i].Size = files[i].Bytes.Length;
                headerSize += files[i].Name.Length + 9;
            }

            files[0].Offset = headerSize;

            for (int i = 1; i < files.Length; i++)
                files[i].Offset = files[i - 1].Offset + files[i - 1].Size;
            var uncompressedSize = headerSize;
            for (int i = 0; i < files.Length; i++)
                uncompressedSize += files[i].Bytes.Length;

            Files = files;
            UncompressedSize = uncompressedSize;
            Opened = true;
        }

        public void Open()
        {
            var file = File.ReadAllBytes(FilePath);
            var fileReader = new FileReader(file);

            //read compressed header
            var signature = fileReader.ReadString();
            var buildVersion = fileReader.ReadInt32();
            var webPlayerVersion = fileReader.ReadString();
            var unityEngineVersion = fileReader.ReadString();
            var compressedFileSize = fileReader.ReadInt32();
            var compressedHeaderSize = fileReader.ReadInt32();
            fileReader.SkipBytes(8);
            var compressedBodySize = fileReader.ReadInt32();
            var uncompressedSize = fileReader.ReadInt32();
            var compressedFileSize2 = fileReader.ReadInt32();
            var uncompressedHeaderSize = fileReader.ReadInt32();

            if (fileReader.ReadByte() != 0x00) //end of header
                throw new InvalidDataException(string.Format("Expected 0x00 at: {0}.", fileReader.Position.ToString("X2")));
            fileReader.Position = compressedHeaderSize;

            //read compressed body
            var fileBody = fileReader.ReadByteArray(compressedBodySize);
            var decompressedFile = LzmaUtils.Decompress(fileBody);
            File.WriteAllBytes(@"C:\Users\Ramda_000\Documents\GitHub\Unity3D-Disassembler\Source\bin\Debug\decompressed.txt", decompressedFile);

            //read decompressed header
            fileReader = new FileReader(decompressedFile);
            var files = new WebArchiveFile[fileReader.ReadInt32()];
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = new WebArchiveFile();
                files[i].Name = fileReader.ReadString();
                files[i].Offset = fileReader.ReadInt32();
                files[i].Size = fileReader.ReadInt32();
            }

            //read decompressed body
            for (int i = 0; i < files.Length; i++)
            {
                fileReader.Position = files[i].Offset;
                files[i].Bytes = fileReader.ReadByteArray(files[i].Size);
            }

            Files = files;
            CompressedSize = compressedFileSize;
            UncompressedSize = uncompressedSize;
            Opened = true;
        }

        public void Close(bool write)
        {
            if (!write)
            {
                
                return;
            }

            var fileWriter = new FileWriter();
            var headerSize = 5;

            fileWriter.WriteInt32(FileCount);
            for (int i = 0; i < FileCount; i++)
            {
                fileWriter.WriteString(this[i].Name);
                fileWriter.WriteInt32(this[i].Offset);
                fileWriter.WriteInt32(this[i].Size);
                headerSize += this[i].Name.Length + 9;
            }
            fileWriter.WriteByte(0x00);

            for (int i = 0; i < FileCount; i++)
            {
                fileWriter.Position = this[i].Offset;
                fileWriter.WriteByteArray(this[i].Bytes);
            }

            var uncompressed = ((MemoryStream)fileWriter.BaseStream).ToArray();
            var compressedBody = LzmaUtils.Compress(uncompressed);

            fileWriter = new FileWriter();
            fileWriter.WriteString("UnityWeb");
            fileWriter.WriteInt32(3);
            fileWriter.WriteString("3.x.x");
            fileWriter.WriteString("4.6.1f1");
            fileWriter.WriteInt32(compressedBody.Length + 60);
            fileWriter.WriteInt32(60);
            fileWriter.WriteInt32(1);
            fileWriter.WriteInt32(1);
            fileWriter.WriteInt32(compressedBody.Length);
            fileWriter.WriteInt32(uncompressed.Length);
            fileWriter.WriteInt32(compressedBody.Length + 60);
            fileWriter.WriteInt32(headerSize);
            fileWriter.WriteByte(0x00);
            fileWriter.WriteByteArray(compressedBody);
            fileWriter.WriteByte(0x00);

            File.WriteAllBytes(FilePath, ((MemoryStream)fileWriter.BaseStream).ToArray());

            this.CompressedSize = fileWriter.BaseStream.Length;
        }

        public void Extract()
        {
            if (!Opened)
                throw new InvalidOperationException("Open() must be called first to open the file.");

            var directory = string.Format("{0}\\{1}", Environment.CurrentDirectory, FileName.Replace(".unity3d", ""));
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            for (int i = 0; i < FileCount; i++)
            {
                var filePath = string.Format("{0}\\{1}", directory, this[i].Name);
                if (!Directory.Exists(filePath)) Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                File.WriteAllBytes(filePath, this[i].Bytes);
            }
        }

        public void ExtractTo(string path)
        {
            if (!Opened)
                throw new InvalidOperationException("Open() must be called first to open the file.");

            var directory = string.Format("{0}\\{1}", path, FileName.Replace(".unity3d", ""));
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            for (int i = 0; i < FileCount; i++)
            {
                var filePath = string.Format("{0}\\{1}", directory, this[i].Name);
                if (!Directory.Exists(filePath)) Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                File.WriteAllBytes(filePath, this[i].Bytes);
            }
        }

        private string[] GetFiles(string directory)
        {
            //not fully implemented
            var filesPath = new List<string>();
            var subDirectories = Directory.GetDirectories(directory);

            for (int i = 0; i < subDirectories.Length; i++)
            {
                var files = Directory.GetFiles(subDirectories[i]);
                filesPath.AddRange(files);
            }
            filesPath.AddRange(Directory.GetFiles(directory));
            return filesPath.ToArray();
        }
    }
}
