using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity3DDisassembler.IO;
using Unity3DDisassembler.Unity;

namespace Unity3DDisassembler
{
    public class Assembler
    {
        ///////////////////////////////////////////////////////////
        ///////////////// STILL BETA STUFFZ HERE //////////////////
        ///////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of Assembler class.
        /// </summary>
        /// <param name="directory">Directory containing files to be assemble.</param>
        public Assembler(string directory)
        {
            CompressedFile = new Unity.CompressedFile(); //get the file that will be compressed.
            DecompressedFile = new Unity.DecompressedFile();
            DecompressedFile.Header.Files = new FileObject[Directory.GetFiles(directory).Length];
            for (int i = 0; i < Directory.GetFiles(directory).Length; i++)
            {
                DecompressedFile.Header.Files[i] = new FileObject();
                DecompressedFile.Header.Files[i].Name = GetFilesName(directory)[i];
                DecompressedFile.Header.Files[i].Bytes = File.ReadAllBytes(directory + @"\" + DecompressedFile.Header.Files[i].Name);
                DecompressedFile.Header.Files[i].Size = DecompressedFile.Header.Files[i].Bytes.Length;
            }
        }

        public CompressedFile CompressedFile { get; set; }
        public DecompressedFile DecompressedFile { get; set; }
        public List<FileObject> Files { get; set; }

        /// <summary>
        /// Start assembling the files into a .unity3d webarchive.
        /// </summary>
        public void Assemble()
        {
            DecompressedFile.WriteFile();
            byte[] compressedbody = Compression.LZMA.LzmaUtils.Compress(((MemoryStream)DecompressedFile.FileWriter.BaseStream).ToArray());
            CompressedFile.Header.BuildVerison = 3;
            CompressedFile.Header.WebPlayerVersion = "3.x.x";
            CompressedFile.Header.UnityEngineVersion = "4.5.1f3";
            CompressedFile.Header.CompressedFileSize = compressedbody.Length;
            CompressedFile.Header.CompressedFileHeaderSize = 60;
            CompressedFile.Header.CompressedFileSizeWithoutHeader = compressedbody.Length - CompressedFile.Header.CompressedFileHeaderSize;
            CompressedFile.Header.DecompressedFileSize = ((MemoryStream)DecompressedFile.FileWriter.BaseStream).ToArray().Length;
            CompressedFile.Header.CompressedFileSize2 = compressedbody.Length;
            CompressedFile.Header.DecompressedFileHeaderSize = DecompressedFile.Header.Files[0].Offset;

            CompressedFile.WriteFile(compressedbody);
        }

        public void WriteFile(string fileName)
        {
            File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + fileName, ((MemoryStream)CompressedFile.FileWriter.BaseStream).ToArray());
        }

        /// <summary>
        /// Get the name of the files in the specified directory.
        /// </summary>
        /// <param name="directory">Directory containing files.</param>
        /// <returns>Files name</returns>
        public string[] GetFilesName(string directory)
        {
            string[] filesPath = Directory.GetFiles(directory); //Still have to complete stuff and things here cause... Man, Brain.exe is not responding.
            string[] FileNames = new string[filesPath.Length];
            for (int i = 0; i < filesPath.Length; i++)
            {
                string[] split = filesPath[i].Split('\\');
                FileNames[i] = split[split.Length - 1].Replace('!', '\\');
            }
            return FileNames;
        }
    }
}
