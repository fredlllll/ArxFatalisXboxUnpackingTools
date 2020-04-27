using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ArxFatalisXboxUnpackingTools.XM1Converter
{
    public class XM1Object
    {
        public string type;
        public uint length;

        public virtual void ReadFrom(BinaryReader reader)
        {
            type = reader.ReadFixedLengthString(4);
            length = reader.ReadUInt32();
        }

        public virtual void WriteTo(BinaryWriter writer)
        {
            writer.WriteFixedLengthString(type, 4);
            writer.Write(length);
        }

        public static string PeekType(BinaryReader reader)
        {
            long start = reader.BaseStream.Position;
            string type = reader.ReadFixedLengthString(4);
            reader.BaseStream.Position = start;
            return type;
        }
    }
}
