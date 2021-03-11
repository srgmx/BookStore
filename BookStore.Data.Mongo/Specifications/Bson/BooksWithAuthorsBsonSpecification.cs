using BookStore.Data.Mongo.Constants;
using BookStore.Data.Mongo.Specifications.Bson;
using BookStore.Domain;
using MongoDB.Bson;
using PluralizeService.Core;

namespace BookStore.Data.Mongo
{
    public class BooksWithAuthorsBsonSpecification : BaseBsonSpecification, IBsonSpecification
    {
        protected override BsonDocument GetSpecification()
        {
            var authorCollection = typeof(Author).Name;
            var authorsKey = PluralizationProvider.Pluralize(authorCollection.ToLower());

            return new BsonDocument
            {
                {
                    MongoKeyWords.LookUp,
                    new BsonDocument
                    {
                        { MongoLookUpKeyWords.From, authorCollection },
                        { MongoLookUpKeyWords.LocalField, $"{authorsKey}.{MongoKeyWords.Id}" },
                        { MongoLookUpKeyWords.ForeignField, MongoKeyWords.Id },
                        { MongoLookUpKeyWords.As, authorsKey }
                    }
                }
            };
        }
    }
}
