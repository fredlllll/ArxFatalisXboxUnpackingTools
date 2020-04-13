using ArxFatalisXboxUnpackingTools.XM1Converter;
using System;
using System.IO;

namespace ArxFatalisXboxUnpackingTerminal.Programs
{
    public class ConvertXM1Program : IProgram
    {
        public bool Run()
        {
            Console.WriteLine("Enter Path to xm1 file:");
            string path = Console.ReadLine();

            var converter = new Converter();
            converter.Convert(new FileInfo(path));

            return true;
        }
    }
}
