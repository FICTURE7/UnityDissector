using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity3dDecompiler.Unity;
using SevenZip.Compression;
using SevenZip.Compression.LZMA;
using System.IO;

namespace Unity3dDecompiler
{
    class Program
    {
        static void main()
        {
            byte[] DecompressedFile = null;
            byte[] UnityFile = FileToRead(@"C:\Users\Ramda_000\Downloads\UberStrikeHeader4.5.0.282.unity3d");
            DataStream reader;

            int TimesFailed = 0;
            int NumFileDetected = 0;

            string[] unityFileInfo = ParseUnityFileHeader(UnityFile);

            if (unityFileInfo[0] == "UnityWeb")
            {
                Console.WriteLine("Decompressing .unity3d file...");
                for (int i = 0; i < 100; i++)
                {
                    try
                    {
                        byte[] CompressedFile = new byte[UnityFile.Length - i];
                        Buffer.BlockCopy(UnityFile, i, CompressedFile, 0, CompressedFile.Length);

                        DecompressedFile = SevenZipHelper.Decompress(CompressedFile);
                        Console.WriteLine("At offset {0} successfully decompressed .unity3d file.", i.ToString("X"));
                        Console.WriteLine("Output in \"DecompressedFile.txt\"");
                        File.WriteAllBytes(@"C:\Users\Ramda_000\Documents\Visual Studio 2012\Projects\Unity3d Decompiler\Unity3d Decompiler\bin\Debug\Decompressed.txt", DecompressedFile);
                        Console.WriteLine("Tried {0}", TimesFailed);
                        byte[]Mod = FileToRead(@"C:\Users\Ramda_000\Desktop\Server\Uberstrike\Data.txt");
                        byte[]CompressedMod = SevenZipHelper.Compress(Mod);
                        File.WriteAllBytes(@"C:\Users\Ramda_000\Documents\Visual Studio 2012\Projects\Unity3d Decompiler\Unity3d Decompiler\bin\Debug\ComMod.txt", CompressedMod);
                        break;
                    }
                    catch
                    {
                        TimesFailed = TimesFailed + 1;
                    }
                }
            }
            else
            {
                main();
            }
            Stream DecompressedFileStream = new MemoryStream(DecompressedFile);
            reader = new DataStream(DecompressedFileStream);
            NumFileDetected = reader.ReadInt();

            Console.WriteLine("{0} files were detected", NumFileDetected);
            Console.WriteLine("Decompressed file size {0} Bytes", DecompressedFile.Length); 

            Console.Read();
        }

        static byte[] FileToRead(string filePath)
        {
            byte[] Read = File.ReadAllBytes(filePath);
            return Read;
        }

        static string[] ParseUnityFileHeader(byte[] UnityFile)
        {
            string[] FileInfo = new string[6];

            if (UnityFile != null)
            {
                Stream UnityFileStream = new MemoryStream(UnityFile);
                DataStream FileReader = new DataStream(UnityFileStream);

                //Signature UnityWeb for web archive (.unity3d)
                FileInfo[0] = FileReader.ReadString();
                FileReader.SkipBytes(4);
                //File version
                FileInfo[1] = (string)FileReader.ReadByte().ToString();
                //Unity Engine version
                FileInfo[2] = FileReader.ReadString();
                //Full Unity Engine version
                FileInfo[3] = FileReader.ReadString();
                //File size when compressed
                FileInfo[4] = FileReader.ReadInt().ToString();
                //Begin of compressed data/end of header
                FileInfo[5] = FileReader.ReadInt().ToString();
                return FileInfo;
            }
            else
            {
                Console.WriteLine("UnityFile is null, connot parse data");
                return FileInfo;
            }
        }

        static void ReplaceAssemblyCSharp(byte[] DecompressedFile)
        {
            byte[] ModdedAssembly = FileToRead(@"C:\Users\Ramda_000\Documents\Visual Studio 2012\Projects\Unity3d Decompiler\Unity3d Decompiler\bin\Debug\Assembly-CSharp.dll");
            byte[] OriginalAssembly = FileToRead(@"C:\Users\Ramda_000\Desktop\Server\Uberstrike\Assembly-CSharp.dll");

            Stream DecompressedFileStream = new MemoryStream(DecompressedFile);
            DataStream FileReader = new DataStream(DecompressedFileStream);
            FileReader.SkipBytes(2011704);
            FileReader.WriteByteArray(ModdedAssembly);
            FileReader.SkipBytes(OriginalAssembly.Length - ModdedAssembly.Length);
            FileReader.SkipBytes(DecompressedFile.Length - (2011704 + ModdedAssembly.Length));
            byte[] data = ReadFully(DecompressedFileStream);

            File.WriteAllBytes(@"C:\Users\Ramda_000\Documents\Visual Studio 2012\Projects\Unity3d Decompiler\Unity3d Decompiler\bin\Debug\Modded.txt", data);
        }

        static string GetBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        static void Main()
        {
            Unpacker unpacker = new Unpacker(@"C:\Users\Ramda_000\Downloads\UberStrikeHeader4.6.0rc3.unity3d");
            ConsoleIO.WriteInfo("File Path: " + unpacker.FilePath);
            ConsoleIO.WriteInfo("File Name: " + unpacker.FileName);
            ConsoleIO.WriteInfo("File Extension: " + unpacker.FileExtension);
            ConsoleIO.WriteInfo("File Size: " + unpacker.FileSize + " bytes");
            unpacker.Unpack();
            //byte[] shizz;
            //shizz = SevenZipHelper.Compress(FileToRead(@"C:\Users\Ramda_000\Documents\Visual Studio 2012\Projects\Unity3d Decompiler\Unity3d Decompiler\bin\Debug\Modded.txt"));
            //File.WriteAllBytes(@"C:\Users\Ramda_000\Documents\Visual Studio 2012\Projects\Unity3d Decompiler\Unity3d Decompiler\bin\Debug\Modded Compressed.txt", shizz);
            Console.Read();
        }
    }
}
