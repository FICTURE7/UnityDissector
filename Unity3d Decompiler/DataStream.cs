using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace Unity3dDecompiler
{
    public class DataStream
    {
        public Stream MainStream { get; set; }

        public DataStream(Stream stream)
        {
            MainStream = stream;
        }

        public int ReadInt()
        {
            var bytes = new byte[4];
            MainStream.Read(bytes, 0, 4);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public byte ReadByte()
        {
            return (byte)MainStream.ReadByte();
        }

        public byte[] ReadByteArray(int count)
        {
            byte[] bytes = new byte[count];

            for (int i = 0; i < count; i++)
            {
                bytes[i] = ReadByte();
            }

            return bytes;
        }

        public void WriteByteArray(byte[] data)
        {
            MainStream.Write(data, 0, data.Length);
        }

        public string ReadString()
        {
            List<byte> list = new List<byte>();
            byte null_teminator = (byte)MainStream.ReadByte();
            while (null_teminator != 0x00)
            {
                list.Add(null_teminator);
                null_teminator = (byte)MainStream.ReadByte();
            }
            return Encoding.ASCII.GetString(list.ToArray());
        }

        public void SkipBytes(int count)
        {
            MainStream.Read(new byte[count], 0, count);
        }
    }
}
