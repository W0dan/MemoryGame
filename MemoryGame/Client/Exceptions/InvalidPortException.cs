using System;

namespace MemoryGame.Client.Exceptions
{
    public class InvalidPortException : Exception
    {
        public InvalidPortException()
            : base("Invalid port")
        { }
    }
}