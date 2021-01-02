using BookStore.Data.Abstraction;
using BookStore.Domain;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookStore.Data.Mongo
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly BookStoreDbContext _context;
        private readonly IMongoCollection<TEntity> _collection;

        public GenericRepository(BookStoreDbContext context)
        {
            _context = context;
            _collection = _context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> criteria = null)
        {
            if (criteria == null)
            {
                return await _collection.AsQueryable().ToListAsync();
            }

            var cursor = await _collection.FindAsync(criteria);
            var entities = await cursor.ToListAsync();

            return entities;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> criteria)
        {
            var cursor = await _collection.FindAsync(criteria);
            var entity = await cursor.FirstOrDefaultAsync();

            return entity;
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            var entityCursor = await _collection.FindAsync(e => e.Id == id);
            var entity = await entityCursor.SingleOrDefaultAsync();

            return entity;
        }

        public virtual Task<TEntity> AddAsync(TEntity entity)
        {
            _context.AddCommand(async () => {
                await _collection.InsertOneAsync(_context.Session, entity);
            });

            return Task.FromResult(entity);
        }

        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.AddCommand(async () =>
            {
                var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
                var entityInDb = await _collection.ReplaceOneAsync(_context.Session, filter, entity);
            });

            return Task.FromResult(entity);
        }

        public async Task RemoveAsync(TEntity entity)
        {
            await RemoveAsync(entity.Id);
        }

        public Task RemoveAsync(Guid id)
        {
            _context.AddCommand(async () =>
            {
                var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
                await _collection.DeleteOneAsync(_context.Session, filter);
            });

            return Task.CompletedTask;
        }
    }
}
