using System;

namespace BookStore.Domain.Exceptions
{
    public class InvalidAuthorsException : Exception
    {
        public InvalidAuthorsException()
        {
        }

        public InvalidAuthorsException(string message)
            : base(message)
        {
        }

        public InvalidAuthorsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}