using System;

namespace BookStore.Data.Mongo
{
    public class MongoTransactionAbortException : Exception
    {
        public MongoTransactionAbortException()
        {
        }

        public MongoTransactionAbortException(string message)
            : base(message)
        {
        }

        public MongoTransactionAbortException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
