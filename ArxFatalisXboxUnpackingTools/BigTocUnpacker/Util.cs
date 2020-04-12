using System.IO;

namespace ArxFatalisXboxUnpackingTools.BigTocUnpacker
{
    public static class Util
    {
        public static string ReadCString(BinaryReader br)
        {
            char c = '\0';
            string output = "";
            while ((c = br.ReadChar()) != 0)
            {
                output += c;
            }
            return output;
        }

        public static bool IsTextFile(Stream s)
        {
            s.Position = 0;

            int controlCharCount = 0;
            for (long i = 0; i < s.Length; i++)
            {
                int b = s.ReadByte();
                if (b < 32 && b != '\r' && b != '\n' && b != ' ' && b != '\t')
                {
                    controlCharCount++;
                }
            }

            if (((float)controlCharCount) / s.Length < 0.05f)
            {
                return true; //if we have less than 5% control characters say its a text file
            }

            return false;
        }

        public static bool ContainsIgnoreCase(this string me, string what)
        {
            return System.Globalization.CultureInfo.InvariantCulture.CompareInfo.IndexOf(me, what, System.Globalization.CompareOptions.IgnoreCase) >= 0;
        }
    }
}
