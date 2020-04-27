using System.Collections.Generic;
using System.IO;

namespace ArxFatalisXboxUnpackingTools.XM1Converter
{
    public class XM1MeshObject : XM1Object
    {
        uint unknown1;
        uint unknown2;
        uint unknown3PossiblyConnectedToHeaderSize;
        uint unknown4;

        public uint vertexCount;
        public uint triangleCount;

        List<uint> unknown5 = new List<uint>();

        public List<XM1MeshVertex> vertices = new List<XM1MeshVertex>();
        public List<XM1MeshTriangleEntry> triangles = new List<XM1MeshTriangleEntry>();

        public override void ReadFrom(BinaryReader reader)
        {
            base.ReadFrom(reader);
            unknown1 = reader.ReadUInt32();
            unknown2 = reader.ReadUInt32();
            unknown3PossiblyConnectedToHeaderSize = reader.ReadUInt32();
            unknown4 = reader.ReadUInt32();

            vertexCount = reader.ReadUInt32();
            triangleCount = reader.ReadUInt32();


            if (vertexCount == 0)
            {
                return;
            }
            //safe to assume the file is longer if the vertex count is > 0
            //TODO: read rest of header, dont know how to determine size though, for now we will search to a sequence which seems to always be the same and go from there
            while (true)
            {
                uint tmp = reader.ReadUInt32();
                unknown5.Add(tmp);
                if (tmp == 100926727) //0x07050406
                {
                    //exit loop
                    unknown5.Add(reader.ReadUInt32());
                    break;
                }
            }


            for (int i = 0; i < vertexCount; i++)
            {
                var vert = new XM1MeshVertex();
                vert.ReadFrom(reader);
                vertices.Add(vert);
            }

            for (int i = 0; i < triangleCount; i++)
            {
                var tri = new XM1MeshTriangleEntry();
                tri.ReadFrom(reader);
                triangles.Add(tri);
            }
        }

        public override void WriteTo(BinaryWriter writer)
        {
            base.WriteTo(writer);

            writer.Write(unknown1);
            writer.Write(unknown2);
            writer.Write(unknown3PossiblyConnectedToHeaderSize);
            writer.Write(unknown4);

            writer.Write(vertexCount);
            writer.Write(triangleCount);
            if(vertexCount == 0)
            {
                return;
            }

            foreach(var tmp in unknown5)
            {
                writer.Write(tmp);
            }

            foreach(var vert in vertices)
            {
                vert.WriteTo(writer);
            }

            foreach(var tri in triangles)
            {
                tri.WriteTo(writer);
            }
        }
    }
}
