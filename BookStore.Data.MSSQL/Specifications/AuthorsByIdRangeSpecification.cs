using BookStore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Data.MSSQL.Specifications
{
    public class AuthorsByIdRangeSpecification : BaseSpecification<Author>
    {
        public AuthorsByIdRangeSpecification(IEnumerable<Guid> ids) :
            base(b => ids.Contains(b.Id))
        {
        }
    }
}
