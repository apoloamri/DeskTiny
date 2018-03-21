using DTCore.Tools;
using System;
using System.IO;
using System.Text;
using SystemDiagnostics = System.Diagnostics;

namespace DTCore.System.Diagnostics
{
    public static class Debug
    {
        public static void WriteLine(string title, string details)
        {
            SystemDiagnostics.Debug.WriteLine(GenerateText(title, details));
        }

        public static void WriteLog(string path, string title, string details)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(GenerateText(title, details));

            Directory.CreateDirectory(Path.GetFullPath(path));

            File.AppendAllText(path, stringBuilder.ToString());

            stringBuilder.Clear();
        }

        private static string GenerateText(string title, string details)
        {
            return
                $"------------------------------{Environment.NewLine}" +
                $"[{title}]:{Environment.NewLine}" +
                $"{details}{Environment.NewLine}" +
                $"------------------------------{Environment.NewLine}";
        }
    }
}
