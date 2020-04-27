using System.IO;

namespace ArxFatalisXboxUnpackingTools.XM1Converter
{
    public class XM1MeshVertex
    {
        public float x, y, z;
        public byte r, g, b, a;
        public uint flags; //TODO: no idea what this actually does

        float ReadWeird(BinaryReader reader)
        {
            return reader.ReadInt32() / 1000000.0f;
        }

        void WriteWeird(BinaryWriter writer, float f)
        {

        }

        public void ReadFrom(BinaryReader reader)
        {
            //x = reader.ReadSingle();
            //y = reader.ReadSingle();
            //z = reader.ReadSingle();

            x = ReadWeird(reader);
            y = ReadWeird(reader);
            z = ReadWeird(reader);

            r = reader.ReadByte();
            g = reader.ReadByte();
            b = reader.ReadByte();
            a = reader.ReadByte();

            flags = reader.ReadUInt32();
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);

            writer.Write(r);
            writer.Write(g);
            writer.Write(b);
            writer.Write(a);

            writer.Write(flags);
        }
    }
}
