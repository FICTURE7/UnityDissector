using System;
using System.Collections.Generic;
using System.IO;

namespace Unity3DDisassembler.IO
{
    public class FileReader
    {
        /// <summary>
        /// A class that is used to read data types from
        /// a buffer
        /// </summary>
        /// <param name="buffer">Buffer from where data will be read</param>
        public FileReader(byte[] buffer)
        {
            BaseStream = new MemoryStream(buffer);
        }

        /// <summary>
        /// A class that is used to read data types from
        /// a buffer
        /// </summary>
        /// <param name="stream">Stream that contains the buffer</param>
        public FileReader(Stream stream)
        {
            BaseStream = stream;
        }

        public Stream BaseStream { get; set; }

        public byte ReadByte()
        {
            return (byte)BaseStream.ReadByte();
        }

        public short ReadInt16()
        {
            var bytes = ReadByteArray(2);
            Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }

        public ushort ReadUInt16()
        {
            var bytes = ReadByteArray(2);
            Array.Reverse(bytes);
            return BitConverter.ToUInt16(bytes, 0);
        }

        public int ReadInt32()
        {
            var bytes = ReadByteArray(4);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public uint ReadUInt32()
        {
            var bytes = ReadByteArray(4);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        public long ReadInt64()
        {
            var bytes = ReadByteArray(8);
            Array.Reverse(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

        public ulong ReadUInt64()
        {
            var bytes = ReadByteArray(8);
            Array.Reverse(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }

        public float ReadSingle()
        {
            var bytes = ReadByteArray(8);
            Array.Reverse(bytes);
            return BitConverter.ToSingle(bytes, 0);
        }

        public string ReadString()
        {
            List<byte> list = new List<byte>();
            byte chars = (byte)BaseStream.ReadByte();
            while (chars != 0x00)//Check for null terminator
            {
                list.Add(chars);
                chars = (byte)BaseStream.ReadByte();
            }
            return System.Text.Encoding.ASCII.GetString(list.ToArray());
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
