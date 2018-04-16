using Tenderfoot.DTSystem.Diagnostics;
using System;

namespace Tenderfoot.DTSystem
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
