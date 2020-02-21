using System;

namespace Philter.Model
{
    public class ReplacementStoreDisabledException : PhilterException
    {
        public ReplacementStoreDisabledException(string message) : base(message)
        {

        }

        public ReplacementStoreDisabledException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}
