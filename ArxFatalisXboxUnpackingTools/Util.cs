using System.IO;

namespace ArxFatalisXboxUnpackingTools
{
    public static class Util
    {
        public static bool IsTextFile(Stream s)
        {
            BinaryReader br = new BinaryReader(s, System.Text.Encoding.ASCII);
            //BinaryReader br = new BinaryReader(s);

            s.Position = 0;

            int textCharCount = 0;
            for (long i = 0; i < s.Length; i++)
            {
                char c = br.ReadChar();
                if (c >= 32 && c <= 126 || c == 10 || c == 13) //printable or cr,lf
                {
                    textCharCount++;
                }
            }

            if (((float)textCharCount) / s.Length > 0.40)
            {
                return true; //if we have more than 40% text characters say its a text file, should also handle utf16 and the like
            }

            return false;
        }
    }
}
