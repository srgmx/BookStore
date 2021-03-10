using BookCoreLibrary.Domain.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace BookStore.Domain
{
    public class Book : BaseEntity
    {
        public Book()
        {
            Authors = new List<Author>();
        }

        public string Name { get; set; }

        public string Articul { get; set; }

        public int AvailableQuantity { get; set; }

        public int ReservedQuantity { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        public DateTime PublishedAt { get; set; }

        public List<Author> Authors { get; set; }
    }
}
