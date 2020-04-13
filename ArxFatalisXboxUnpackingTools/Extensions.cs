using System.IO;

namespace ArxFatalisXboxUnpackingTools
{
    public static class Extensions
    {
        public static string ReadCString(this BinaryReader br, int limit = int.MaxValue)
        {
            string output = "";
            char c;
            while (output.Length < limit &&  (c = br.ReadChar()) != 0)
            {
                output += c;
            }
            return output;
        }

        public static void WriteCString(this BinaryWriter bw, string str, int totalLength = -1)
        {
            if(totalLength < 0)
            {
                totalLength = str.Length + 1; //+1 for 0 termination
            }

            for(int i =0; i < totalLength; i++)
            {
                if(i < str.Length) {
                    bw.Write(str[i]);
                }
                else
                {
                    bw.Write('\0');
                }
            }
        }

        public static string ReadFixedLengthString(this BinaryReader br, int length)
        {
            return new string(br.ReadChars(length));
        }

        public static void WriteFixedLengthString(this BinaryWriter bw, string str, int length = -1)
        {
            if(length < 0)
            {
                length = str.Length;
            }

            for (int i = 0; i < length; i++)
            {
                if (i < str.Length)
                {
                    bw.Write(str[i]);
                }
                else
                {
                    bw.Write('\0');
                }
            }
        }

        public static bool ContainsIgnoreCase(this string me, string what)
        {
            return System.Globalization.CultureInfo.InvariantCulture.CompareInfo.IndexOf(me, what, System.Globalization.CompareOptions.IgnoreCase) >= 0;
        }
    }
}
