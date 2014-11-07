using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Unity3DDisassembler.IO
{
    public class FileWriter
    {
        public FileWriter(byte[] buffer)
        {
            BaseStream = new MemoryStream(buffer);
        }

        public FileWriter()
        {
            BaseStream = new MemoryStream();
        }

        public FileWriter(Stream stream)
        {
            BaseStream = stream;
        }

        public Stream BaseStream { get; set; }

        public void WriteByte(byte data)
        {
            BaseStream.WriteByte(data);
        }

        public void WriteInt16(short data)
        {
            var bytes = BitConverter.GetBytes(data);
            Array.Reverse(bytes);
            WriteByteArray(bytes);
        }

        public void WriteUInt16(ushort data)
        {
            var bytes = BitConverter.GetBytes(data);
            Array.Reverse(bytes);
            WriteByteArray(bytes);
        }

        public void WriteInt32(int data)
        {
            var bytes = BitConverter.GetBytes(data);
            Array.Reverse(bytes);
            WriteByteArray(bytes);
        }

        public void WriteUInt32(uint data)
        {
            var bytes = BitConverter.GetBytes(data);
            Array.Reverse(bytes);
            WriteByteArray(bytes);
        }

        public void WriteInt64(ulong data)
        {
            var bytes = BitConverter.GetBytes(data);
            Array.Reverse(bytes);
            WriteByteArray(bytes);
        }

        public void WriteUInt64(ulong data)
        {
            var bytes = BitConverter.GetBytes(data);
            Array.Reverse(bytes);
            WriteByteArray(bytes);
        }

        public void WriteSingle(float data)
        {
            var bytes = BitConverter.GetBytes(data);
            Array.Reverse(bytes);
            WriteByteArray(bytes);
        }

        public void WriteString(string data)
        {
            var bytes = Encoding.ASCII.GetBytes(data);
            WriteByteArray(bytes);
            WriteByte(0x00);
        }

        public void WriteByteArray(byte[] data)
        {
            BaseStream.Write(data, 0, data.Length);
        }

        public void SkipBytes(int count)
        {
            BaseStream.Position = BaseStream.Position + count;
        }

        public void Goto(int offset)
        {
            BaseStream.Position = offset;
        }
    }
}
