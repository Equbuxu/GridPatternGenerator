using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternGenerator
{
    public static class MemoryStreamExt
    {
        public static void WriteInt(this MemoryStream stream, int value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }
        public static void WriteUInt(this MemoryStream stream, uint value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }
        public static void WriteUShort(this MemoryStream stream, ushort value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }
    }
}
