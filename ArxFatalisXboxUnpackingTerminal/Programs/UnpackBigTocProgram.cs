﻿using ArxFatalisXboxUnpackingTools.BigTocUnpacker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ArxFatalisXboxUnpackingTerminal.Programs
{
    public class UnpackBigTocProgram : IProgram
    {
        public bool Run()
        {
            Console.WriteLine("Enter Path to big or toc file:");
            string bigOrToc = Console.ReadLine();
            string baseFilename = Path.Combine(Path.GetDirectoryName(bigOrToc), Path.GetFileNameWithoutExtension(bigOrToc));

            FileInfo big = new FileInfo(baseFilename + ".big");
            FileInfo toc = new FileInfo(baseFilename + ".toc");

            if (big != null && toc != null)
            {
                var unpacker = new Unpacker(big, toc);
                unpacker.Unpack();
                return true;
            }

            return false;
        }
    }
}
