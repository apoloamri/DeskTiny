using System;
using diagnostics = System.Diagnostics;

namespace DTCore.System.Diagnostics
{
    public static class Debug
    {
        public static void WriteLine(string title, string details)
        {
            diagnostics.Debug.WriteLine(
                $"------------------------------{Environment.NewLine}" + 
                $"[{title}]:{Environment.NewLine}" +
                $"{details}{Environment.NewLine}" +
                $"------------------------------{Environment.NewLine}");
        }
    }
}
