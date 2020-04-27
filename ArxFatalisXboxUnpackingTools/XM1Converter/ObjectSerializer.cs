using System;
using System.IO;

namespace ArxFatalisXboxUnpackingTools.XM1Converter
{
    public static class ObjectSerializer
    {
        public static XM1Object ReadObject(BinaryReader reader)
        {
            long start = reader.BaseStream.Position;
            string type = XM1Object.PeekType(reader);
            XM1Object retval = null;
            switch (type)
            {
                case "MESH":
                    var mesh = new XM1MeshObject();
                    mesh.ReadFrom(reader);
                    retval = mesh;
                    break;
                default: //we only know mesh atm, but in case another type comes by we will do this
                    retval = new XM1Object();
                    retval.ReadFrom(reader);
                    Console.WriteLine("found unknown object: " + retval.type);
                    break;
            }

            long end = start + retval.length;
            if (reader.BaseStream.Position != end)
            {
                reader.BaseStream.Position = end;
                Console.WriteLine("messed up reading on object: " + retval.type);
            }

            return retval;
        }

        public static void WriteObject(BinaryWriter writer, XM1Object obj)
        {
            obj.WriteTo(writer); //not much else to do here
        }
    }
}
