using System;

namespace BookStore.Data.Mongo
{
    public class MongoTransactionResponse
    {
        public string Mesage { get; set; }

        public bool IsSuccess { get; set; }

        public Exception Exception { get; set; }
    }
}
