using BookStore.Data.Mongo.Constants;
using BookStore.Data.Mongo.Specifications.Bson;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace BookStore.Data.Mongo
{
    public class BooksByIdRangeBsonSpecification : BaseBsonSpecification, IBsonSpecification
    {
        private readonly IEnumerable<Guid> _bookIds;

        public BooksByIdRangeBsonSpecification(IEnumerable<Guid> bookIds)
        {
            _bookIds = bookIds;
        }

        protected override BsonDocument GetSpecification()
        {
            var rangeBsonDoc = new BsonDocument(MongoKeyWords.In, new BsonArray(_bookIds));
            var idFilterBsonDoc = new BsonDocument(MongoKeyWords.Id, rangeBsonDoc);
            var matchBsonDoc = new BsonDocument(MongoKeyWords.Match, idFilterBsonDoc);

            return matchBsonDoc;
        }
    }
}
