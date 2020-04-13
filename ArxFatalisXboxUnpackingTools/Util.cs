using System.IO;

namespace ArxFatalisXboxUnpackingTools
{
    public static class Util
    {
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
    }
}
