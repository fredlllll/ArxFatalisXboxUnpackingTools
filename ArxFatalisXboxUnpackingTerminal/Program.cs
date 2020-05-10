using ArxFatalisXboxUnpackingTerminal.Programs;
using System;

namespace ArxFatalisXboxUnpackingTerminal
{
    class Program
    {
        static void wr(string msg)
        {
            Console.WriteLine(msg);
        }

        static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                wr("What you wanna do?");
                wr("0) exit");
                wr("1) unpack big and toc file");
                wr("2) convert xm1 to obj");


                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    IProgram toRun = null;
                    switch (choice)
                    {
                        case 0:
                            running = false;
                            break;
                        case 1:
                            toRun = new UnpackBigTocProgram();
                            break;
                        case 2:
                            toRun = new ConvertXM1Program();
                            break;
                        default:
                            wr("not a valid choice, try again next time");
                            break;
                    }
                    if (toRun != null)
                    {
                        toRun.Run();
                    }
                }
                else
                {
                    wr("Thats not a number, try again next time");
                }
            }
        }
    }
}
