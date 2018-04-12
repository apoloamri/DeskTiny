using DTCore.DTSystem.Diagnostics;
using System;

namespace DTCore.DTSystem
{
    public class DTException : Exception
    {
        public DTException(string message)
        {
            DTDebug.WriteLine("System Exception:", message);

            throw new Exception(message);
        }
    }
}
