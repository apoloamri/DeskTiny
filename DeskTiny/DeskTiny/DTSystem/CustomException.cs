using DTCore.DTSystem.Diagnostics;
using System;

namespace DTCore.DTSystem
{
    public class CustomException : Exception
    {
        public CustomException(string message)
        {
            DTDebug.WriteLine("System Exception:", message);

            throw new Exception(message);
        }
    }
}
