using System;
using System.IO;

namespace ArxFatalisXboxUnpackingTools.XM1Converter
{
    public class XM1MeshVertex
    {
        public float x, y, z;
        public byte r, g, b, a;
        public uint flags; //TODO: no idea what this actually does

        float ReadInt16Div1000(BinaryReader reader)
        {
            return reader.ReadInt16() / 1000.0f;
        }


        void WriteWeird(BinaryWriter writer, float f)
        {

        }

        public void ReadFrom(BinaryReader reader)
        {
            //x = reader.ReadSingle();
            //y = reader.ReadSingle();
            //z = reader.ReadSingle();

            x = ReadInt16Div1000(reader);
            y = ReadInt16Div1000(reader);
            z = ReadInt16Div1000(reader);

            reader.ReadInt16();//skip unknown shit
            reader.ReadInt16();
            reader.ReadInt16();

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
