using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity3dDecompiler.Unity
{
    public class DecompressedFileHeader
    {
        public int FileCount { get; set; }

        public FileObject[] Files { get; set; }

        //Still has no use.
    }
}
