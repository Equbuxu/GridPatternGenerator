using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternGenerator.ClipboardUtils
{
    public class ClipboardEntry
    {
        public uint id;
        public byte[] data;

        public ClipboardEntry(uint id, byte[] data)
        {
            this.id = id;
            this.data = data;
        }
    }
}
