using System;

namespace Philter.Model
{
    public class PhilterException : Exception
    {
        public PhilterException(string message) : base(message)
        {

        }

        public PhilterException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}
