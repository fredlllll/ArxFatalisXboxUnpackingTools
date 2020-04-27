using System.IO;

namespace ArxFatalisXboxUnpackingTools.XM1Converter
{
    public class XM1MeshTriangleEntry
    {
        public ushort a, b, c;

        public void ReadFrom(BinaryReader reader)
        {
            a = reader.ReadUInt16();
            b = reader.ReadUInt16();
            c = reader.ReadUInt16();
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(a);
            writer.Write(b);
            writer.Write(c);
        }
    }
}

