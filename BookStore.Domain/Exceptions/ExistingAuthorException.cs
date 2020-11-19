using System;

namespace BookStore.Domain.Exceptions
{
    public class ExistingAuthorException : Exception
    {
        public ExistingAuthorException()
        {
        }

        public ExistingAuthorException(string message)
            : base(message)
        {
        }

        public ExistingAuthorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
