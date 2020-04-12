using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ArxFatalisXboxUnpackingTools.XM1Converter
{
    public class Converter
    {
        public FileInfo Convert(FileInfo xm1File)
        {
            //xm1 header: 16 bytes, first 3 are XM1, then most likely 3 unidentified int32
            //after that follow objects, each object has 4 chars identifying it, then 4 bytes telling the length of the object in bytes, including identifier and length

            return xm1File; //TODO:
        }
    }
}
