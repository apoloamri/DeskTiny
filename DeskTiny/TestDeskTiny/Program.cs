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
                Start();

                //DatabaseTest.TestAll();

                End();

                Console.WriteLine(
                    $"Press any key to redo test.{Environment.NewLine}" +
                    $"Press ESC to exit the test.{Environment.NewLine}");

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
