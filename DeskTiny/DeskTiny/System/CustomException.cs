using DeskTiny.System.Diagnostics;
using System;

namespace DeskTiny.System
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
