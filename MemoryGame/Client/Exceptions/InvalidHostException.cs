using System;

namespace MemoryGame.Client.Exceptions
{
    public class InvalidHostException : Exception
    {
        public InvalidHostException()
            : base("Invalid host")
        { }
    }
}