using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Tenderfoot.TfSystem.Diagnostics
{
    public static class TfDebug
    {
        public static void WriteLine(string title, string details)
        {
            Debug.WriteLine(GenerateText(title, details));
        }

        public static void WriteLog(Exception details)
        {
            var errorDetails =
                $"Stack trace: {details.StackTrace}{Environment.NewLine}" +
                $"Source: {details.Source}{Environment.NewLine}" +
                $"Message: {details.Message}";

            WriteLog(
                TfSettings.Logs.System, 
                $"System Exception Details - {DateTime.Now}", 
                errorDetails);
        }

        public static void WriteLog(string path, string title, string details)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(GenerateText(title, details));

            Directory.CreateDirectory(Path.GetDirectoryName(path));

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
