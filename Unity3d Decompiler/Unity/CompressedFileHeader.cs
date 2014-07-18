using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity3dDecompiler.Unity
{
    class CompressedFileHeader
    {
        public string Signature { get; set; }

        public int BuildVerison { get; set; }
    }
}
