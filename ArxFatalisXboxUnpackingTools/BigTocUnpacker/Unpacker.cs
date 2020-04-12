using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ArxFatalisXboxUnpackingTools.BigTocUnpacker
{

    public class Unpacker
    {
        struct MyFileData
        {
            public uint hash;
            public uint length;
            public uint offset;

            public MyFileData(uint hash, uint length, uint offset)
            {
                this.hash = hash;
                this.length = length;
                this.offset = offset;
            }
        }

        private FileInfo bigFile;
        private FileInfo tocFile;
        private DirectoryInfo outputDir;

        private FileStream toc, big;
        private BinaryReader tocReader;

        public Unpacker(FileInfo bigFile, FileInfo tocFile)
        {
            this.bigFile = bigFile;
            this.tocFile = tocFile;

            string filename = Path.GetFileNameWithoutExtension(this.bigFile.FullName);
            outputDir = new DirectoryInfo(Path.Combine(bigFile.Directory.FullName, filename + "_ext"));
        }

        public void Unpack()
        {
            if (outputDir.Exists)
            {
                //make sure output dir is empty
                outputDir.Delete(true);
                while (outputDir.Exists)
                {
                    Thread.Sleep(100);
                    outputDir.Refresh();
                }
            }
            outputDir.Create();

            using (toc = new FileStream(tocFile.FullName, FileMode.Open, FileAccess.Read))
            using (tocReader = new BinaryReader(toc))
            using (big = new FileStream(bigFile.FullName, FileMode.Open, FileAccess.Read))
            {
                uint fileCount = tocReader.ReadUInt32(); //read filecount
                                                         //fileCount = (uint)((toc.Length - 4) / 12); //alternatively calculate filecount from filesize
                List<MyFileData> files = new List<MyFileData>();
                for (long i = 0; i < fileCount; i++)
                {
                    uint garbageChecksumMaybe = tocReader.ReadUInt32();
                    uint fileLength = tocReader.ReadUInt32();
                    uint fileOffset = tocReader.ReadUInt32();
                    files.Add(new MyFileData(garbageChecksumMaybe, fileLength, fileOffset));
                }

                Parallel.ForEach(files, (f) =>
                {
                    FileInfo output = UnpackFile(f);
                    output = ExtensionFinder.FindExtensionAndRename(output);
                    Console.WriteLine("Extracting " + output.Name);
                });
            }
        }

        FileInfo UnpackFile(MyFileData f)
        {
            string checksumHex = f.hash.ToString("X8");

            string filename = "file_" + checksumHex;
            var outFile = new FileInfo(Path.Combine(this.outputDir.FullName, filename));

            byte[] buffer = new byte[1024 * 4];//4KB
            using (var o = new FileStream(outFile.FullName, FileMode.Create, FileAccess.Write))
            {
                lock (big)
                {
                    big.Position = f.offset;
                    int toRead = (int)f.length;
                    while (toRead > 0)
                    {
                        int read = big.Read(buffer, 0, Math.Min(buffer.Length, toRead));
                        toRead -= read;
                        o.Write(buffer, 0, read);
                    }
                }
            }

            return outFile;
        }
    }
}
