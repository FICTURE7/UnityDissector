using System;
using System.IO;

namespace Unity3DDisassembler.Compression.LZMA
{
    public static class LzmaUtils
    {
        //static int dictionary = 1 << 23;
        private static int dictionary = 524288; /*1 << 23*/

        //Peter Bromberg's helper code. Big thanks for that.
        private static bool eos = false;

        private static CoderPropID[] propIDs = 
		{
            CoderPropID.DictionarySize,
            CoderPropID.PosStateBits,
			CoderPropID.LitContextBits,
			CoderPropID.LitPosBits,
			CoderPropID.Algorithm,
			CoderPropID.NumFastBytes,
			CoderPropID.MatchFinder,
			CoderPropID.EndMarker,
		};

        // these are the default properties, keeping it simple for now:
        private static object[] properties = 
        {
			(Int32)(dictionary),
			(Int32)(2),
			(Int32)(3),
			(Int32)(0),
			(Int32)(2),
			(Int32)(64),
			"bt4",
			eos
        };

        /// <summary>
        /// Compresses the inputBytes in Lzma
        /// </summary>
        /// <param name="inputBytes">bytes to compress</param>
        /// <returns>Compressed bytes</returns>
        public static byte[] Compress(byte[] inputBytes)
        {
            if (inputBytes != null)
            {
                MemoryStream inStream = new MemoryStream(inputBytes);
                MemoryStream outStream = new MemoryStream();
                Unity3DDisassembler.Compression.LZMA.Encoder encoder = new Unity3DDisassembler.Compression.LZMA.Encoder();
                encoder.SetCoderProperties(propIDs, properties);
                encoder.WriteCoderProperties(outStream);
                long fileSize = inStream.Length;
                for (int i = 0; i < 8; i++)
                    outStream.WriteByte((Byte)(fileSize >> (8 * i)));
                encoder.Code(inStream, outStream, -1, -1, null);

                //Clear Memory
                inStream.Dispose();
                encoder = null;

                return outStream.ToArray();
            }
            else
            {
                throw new ArgumentNullException("inputBytes", "inputBytes was null. :[");
            }
        }

        /// <summary>
        /// Decompresses the inputBytes from Lzma
        /// </summary>
        /// <param name="inputBytes">bytes to decompress</param>
        /// <returns>Decompressed bytes</returns>
        public static byte[] Decompress(byte[] inputBytes)
        {
            if (inputBytes != null)
            {
                MemoryStream newInStream = new MemoryStream(inputBytes);

                Unity3DDisassembler.Compression.LZMA.Decoder decoder = new Unity3DDisassembler.Compression.LZMA.Decoder();

                newInStream.Seek(0, 0);
                MemoryStream newOutStream = new MemoryStream();

                byte[] properties2 = new byte[5];
                if (newInStream.Read(properties2, 0, 5) != 5)
                    throw (new Exception("input .lzma is too short"));
                long outSize = 0;
                for (int i = 0; i < 8; i++)
                {
                    int v = newInStream.ReadByte();
                    if (v < 0)
                        throw (new Exception("Can't Read 1"));
                    outSize |= ((long)(byte)v) << (8 * i);
                }
                decoder.SetDecoderProperties(properties2);
                long compressedSize = newInStream.Length - newInStream.Position;
                decoder.Code(newInStream, newOutStream, compressedSize, outSize, null);

                //Clear Memory
                properties2 = null;
                newInStream.Dispose();
                decoder = null;

                return newOutStream.ToArray();
            }
            else
            {
                throw new ArgumentNullException("inputBytes", "inputBytes was null. :[");
            }
        }
    }
}
