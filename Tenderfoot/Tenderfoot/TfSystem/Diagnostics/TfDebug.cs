﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Tenderfoot.Database;
using Tenderfoot.Net;
using Tenderfoot.Tools.Extensions;

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
            SaveToDB(GenerateText(title, details));

            //StringBuilder stringBuilder = new StringBuilder();

            //stringBuilder.Append(GenerateText(title, details));
            //var message = stringBuilder.ToString();
            
            //Directory.CreateDirectory(Path.GetDirectoryName(path)); 
            //File.AppendAllText(path, message);
            
            //stringBuilder.Clear();
        }

        private static string GenerateText(string title, string details)
        {
            return
                $"------------------------------{Environment.NewLine}" +
                $"[{title}]:{Environment.NewLine}" +
                $"{details}{Environment.NewLine}" +
                $"------------------------------{Environment.NewLine}";
        }

        private static void SaveToDB(string message)
        {
            var systemLogs = Schemas.SystemLogs;
            systemLogs.Entity.message = message;
            systemLogs.Insert();
        }
    }
}
