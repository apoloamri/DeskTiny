using System;
using System.Diagnostics;

namespace DeskTiny.Tools
{
    public static class Diagnostics
    {
        public static void WriteDebug(string title, string details)
        {
            Debug.WriteLine(
                $"------------------------------" + Environment.NewLine +
                $"[{title}]:" + Environment.NewLine +
                details + Environment.NewLine +
                $"------------------------------" + Environment.NewLine + Environment.NewLine);
        }
    }
}
