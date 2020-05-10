using ArxFatalisXboxUnpackingTools.BigTocUnpacker;
using System;
using System.IO;

namespace ArxFatalisXboxUnpackingTerminal.Programs
{
    public class UnpackBigTocProgram : IProgram
    {
        public bool Run()
        {
            Console.WriteLine("Enter Path to big or toc file:");
            string bigOrToc = Console.ReadLine();
            if (bigOrToc.Length == 0)
            {
                Console.WriteLine("you have to enter something dude");
                return false;
            }
            string baseFilename = Path.Combine(Path.GetDirectoryName(bigOrToc), Path.GetFileNameWithoutExtension(bigOrToc));

            FileInfo big = new FileInfo(baseFilename + ".big");
            FileInfo toc = new FileInfo(baseFilename + ".toc");

            if (big.Exists && toc.Exists)
            {
                var unpacker = new Unpacker(big, toc);
                unpacker.Unpack();
                return true;
            }

            return false;
        }
    }
}
