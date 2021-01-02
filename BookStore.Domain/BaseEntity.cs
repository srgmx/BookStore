using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BookStore.Domain
{
    public class BaseEntity
    {
        [BsonId]
        public Guid Id { get; set; }
    }
}
