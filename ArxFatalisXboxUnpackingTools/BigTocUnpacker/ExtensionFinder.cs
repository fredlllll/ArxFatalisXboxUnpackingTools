using System.IO;

namespace ArxFatalisXboxUnpackingTools.BigTocUnpacker
{
    public static class ExtensionFinder
    {
        static string FindExtension(FileInfo file)
        {
            using (var f = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(f, System.Text.ASCIIEncoding.ASCII))
            {
                try
                {
                    f.Position = 0;
                    string first2 = new string(reader.ReadChars(2));
                    switch (first2)
                    {
                        case "BM":
                            return "bmp";
                    }
                }
                catch { }

                try
                {
                    f.Position = 0;
                    string first3 = new string(reader.ReadChars(3));
                    switch (first3)
                    {
                        case "DDS":
                            return "dds";
                        case "KFA":
                            return "cin";
                        case "FTL":
                            return "ftl";
                        case "XM1":
                            return "xm1";
                    }
                }
                catch { }

                try
                {
                    f.Position = 0;
                    string first4 = new string(reader.ReadChars(4));
                    switch (first4)
                    {
                        case "RIFF":
                            return "wav";
                    }
                }
                catch { }

                try
                {
                    f.Position = 0;
                    string jfif = new string(reader.ReadChars(4));
                    if (jfif.Equals("JFIF"))
                    {
                        return "jpg";
                    }
                }
                catch { }

                try
                {
                    f.Position = 0;
                    string cstring = Util.ReadCString(reader);
                    if (cstring.ContainsIgnoreCase("levels\\level"))
                    {
                        return "fts"; //fts levels contain their dir path as first cstring
                    }
                    else if (cstring.Equals("Theo Animation File"))
                    {
                        return "tea";
                    }
                }
                catch { }

                try
                {
                    f.Position = 4;
                    string cstring = Util.ReadCString(reader);
                    if (cstring.Equals("DANAE_FILE"))
                    {
                        return "dlf";
                    }
                    else if (cstring.Equals("DANAE_LLH_FILE"))
                    {
                        return "llf";
                    }
                }
                catch { }

                try
                {
                    f.Position = 2;
                    byte[] bytes = reader.ReadBytes(3);
                    if (bytes[0] == 216 && bytes[1] == 69 && bytes[2] == 193)
                    {
                        return "llf"; //it seems these 3 bytes are the same for all llfs
                    }
                }
                catch { }

                if (f.Length > 0 && Util.IsTextFile(f))
                {
                    return "txt"; //give it txt if it doesnt contain control chars
                }

                return null;
            }
        }

        public static FileInfo FindExtensionAndRename(FileInfo file)
        {
            //check if we can find out filext
            string extension = FindExtension(file);

            if (extension != null)
            {
                string filename = file.Name + "." + extension;
                string output = Path.Combine(file.DirectoryName, filename);
                file.MoveTo(output);
                return new FileInfo(output);
            }
            return file;
        }
    }
}
