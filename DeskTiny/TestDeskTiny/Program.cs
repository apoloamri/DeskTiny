using System;
using System.Diagnostics;

namespace TestDeskTiny
{
    class Program
    {
        static Stopwatch timer = new Stopwatch();

        static void Main(string[] args)
        {
            do
            {
                while (!Console.KeyAvailable)
                {
                    Start();

                    DatabaseTest.TestAll();

                    End();

                    Console.WriteLine($"Press any key to redo test.{Environment.NewLine}");
                    Console.ReadKey();
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        static void Start()
        {
            timer.Reset();
            timer.Start();
        }

        static void End()
        {
            timer.Stop();
            Console.WriteLine($"Total elapsed time: {timer.ElapsedMilliseconds}");
        }
    }
}
