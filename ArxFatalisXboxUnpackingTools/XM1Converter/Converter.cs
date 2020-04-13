using System;
using System.Collections.Generic;
using System.IO;

namespace ArxFatalisXboxUnpackingTools.XM1Converter
{
    public class Converter
    {
        class XM1Header
        {
            public string identifier;
            public uint unknown1;
            public string unknown2;
            public uint unknown3;

            public XM1Header(BinaryReader reader)
            {
                identifier = reader.ReadCString(4); // file identifier XM1
                unknown1 = reader.ReadUInt32(); //no idea, 0
                unknown2 = reader.ReadCString(4); // string "001"
                unknown3 = reader.ReadUInt32(); //no idea, 0
            }
        }

        class XM1Object
        {
            public string type;
            public uint length;
            public byte[] body;

            public XM1Object(BinaryReader reader)
            {
                type = reader.ReadCString(4); // type, e.g. MESH
                Console.WriteLine("Found " + type);
                length = reader.ReadUInt32();

                int bodyLength = (int)(length - 8);
                body = new byte[bodyLength];
                for (int i = 0; i < bodyLength; i++)
                {
                    body[i] = reader.ReadByte(); //TODO: optimize this to read in chunks or so
                }
            }
        }

        public FileInfo Convert(FileInfo xm1File)
        {
            var dir = xm1File.Directory.FullName;

            using (var f = new FileStream(xm1File.FullName, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(f))
            {
                var header = new XM1Header(reader);
                List<XM1Object> objects = new List<XM1Object>();
                int i = 0;
                while (f.Position < f.Length)
                {
                    var obj = new XM1Object(reader);

                    var outfile = Path.Combine(dir, "xm1_obj_" + i);
                    using (var o = new FileStream(outfile, FileMode.Create, FileAccess.Write))
                    using (var owriter = new BinaryWriter(o))
                    {
                        owriter.WriteFixedLengthString(obj.type, 4);
                        owriter.Write(obj.length);
                        owriter.BaseStream.Write(obj.body, 0, obj.body.Length);
                    }

                    objects.Add(obj);
                    i++;
                }
            }

            //xm1 header: 16 bytes, first 3 are XM1, then most likely 3 unidentified int32
            //after that follow objects, each object has 4 chars identifying it, then 4 bytes telling the length of the object in bytes, including identifier and length

            return xm1File; //TODO:
        }
    }
}
