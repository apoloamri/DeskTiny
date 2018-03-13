using DTCore.System.Diagnostics;
using System;

namespace DTCore.System
{
    public class CustomException : Exception
    {
        public CustomException(string message)
        {
            Debug.WriteLine("System Exception:", message);

            throw new Exception(message);
        }
    }
}
