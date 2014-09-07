using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace Unity3DDisassembler.IO
{
    public class DataStream
    {
        public Stream MainStream { get; set; }

        public DataStream(Stream stream)
        {
            MainStream = stream;
        }

        public DataStream(byte[] data)
        {
            Stream stream = new MemoryStream(data);
            MainStream = stream;
            stream = null;
        }

        public int ReadInt()
        {
            var bytes = ReadByteArray(4);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public void WriteInt(int data)
        {
            byte[] bytes = BitConverter.GetBytes(data);
            Array.Reverse(bytes);
            WriteByteArray(bytes);
        }

        public byte ReadByte()
        {
            return (byte)MainStream.ReadByte();
        }

        public void WriteByte(byte data)
        {
            MainStream.WriteByte(data);
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

        public void WriteString(string data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            byte[] null_terminator = { 0x00 };
            byte[] final = ConcatBytes(bytes, null_terminator);
            MainStream.Write(final, 0, final.Length);
        }

        public void SkipBytes(int count)
        {
            MainStream.Position = MainStream.Position + count;
        }

        public void Goto(int offset)
        {
            MainStream.Position = offset;
        }

        private static byte[] ConcatBytes(params byte[][] bytes)
        {
            List<byte> result = new List<byte>();
            foreach (byte[] array in bytes)
                result.AddRange(array);
            return result.ToArray();
        }
    }
}
