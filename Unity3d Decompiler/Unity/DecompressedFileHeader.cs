using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity3dDecompiler.Unity
{
    public class DecompressedFileHeader
    {
        //Still not implemented and has no use
        public int FileCount { get; set; }
        public FileObject[] Files { get; set; }
    }
}
