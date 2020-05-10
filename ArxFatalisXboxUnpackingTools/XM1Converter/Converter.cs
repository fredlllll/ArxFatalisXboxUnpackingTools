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

            public void Write(BinaryWriter writer)
            {
                writer.WriteCString(identifier, 4);
                writer.Write(unknown1);
                writer.Write(unknown2);
                writer.Write(unknown3);
            }

            public static int Size
            {
                get { return 16; }
            }

            public void ReadFrom(BinaryReader reader)
            {
                identifier = reader.ReadCString(4); // file identifier XM1
                unknown1 = reader.ReadUInt32(); //no idea, 0
                unknown2 = reader.ReadCString(4); // string "001"
                unknown3 = reader.ReadUInt32(); //no idea, 0
            }
        }

        public void Convert(FileInfo xm1File)
        {
            var outDir = Path.Combine(xm1File.Directory.FullName,Path.GetFileNameWithoutExtension(xm1File.FullName));
            Directory.CreateDirectory(outDir);

            List<XM1Object> objects = new List<XM1Object>();
            var header = new XM1Header();

            int i = 0;
            using (var f = new FileStream(xm1File.FullName, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(f))
            {
                header.ReadFrom(reader);
                while (f.Position < f.Length)
                {
                    var start = f.Position;

                    var obj = ObjectSerializer.ReadObject(reader);
                    Console.WriteLine("found " + obj.type);

                    var outfile = Path.Combine(outDir, "xm1_obj_" + i);
                    using (var o = new FileStream(outfile, FileMode.Create, FileAccess.Write))
                    using (var owriter = new BinaryWriter(o))
                    {
                        f.Position = start;
                        owriter.Write(reader.ReadBytes((int)obj.length)); //do a binary copy
                    }

                    objects.Add(obj);
                    i++;
                }
            }

            IFormatProvider format = System.Globalization.CultureInfo.InvariantCulture;
            string floatFormat = "0.00000";

            i = 0;
            foreach (var obj in objects)
            {
                if (obj is XM1MeshObject mesh)
                {
                    FileInfo objFile = new FileInfo(Path.Combine(outDir, Path.GetFileNameWithoutExtension(xm1File.FullName) + "_" + i + ".obj"));
                    i++;

                    if (mesh.vertices.Count == 0)
                    {
                        continue; //skip empty objects
                    }

                    using (FileStream fs = new FileStream(objFile.FullName, FileMode.Create, FileAccess.Write))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine("o obj");
                        sw.WriteLine("# vertices");
                        foreach (var v in mesh.vertices)
                        {
                            sw.Write("v ");
                            sw.Write(v.x.ToString(floatFormat, format) + " ");
                            sw.Write(v.y.ToString(floatFormat, format) + " ");
                            sw.WriteLine(v.z.ToString(floatFormat, format));
                        }
                        sw.WriteLine("# smooth shading off");
                        sw.WriteLine("s off");
                        sw.WriteLine("# triangles");
                        foreach (var t in mesh.triangles)
                        {
                            sw.Write("f ");
                            sw.Write((t.a + 1).ToString(format) + " ");
                            sw.Write((t.b + 1).ToString(format) + " ");
                            sw.WriteLine((t.c + 1).ToString(format));
                        }
                    }
                }
            }
        }
    }
}
