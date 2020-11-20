using BookStore.Data.Contracts;
using BookStore.Data.Specifications;
using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Infrastructure
{
    public abstract class GenericRepository<TEntity> :
        IGenericRepository<TEntity>, 
        IDisposable where TEntity : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _entities;
        private bool _isDisposed;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _entities = _context.Set<TEntity>();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity) 
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification = null)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public virtual async Task<TEntity> FindAsync(ISpecification<TEntity> specification = null)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _entities.FindAsync(id);
        }

        public virtual async Task<bool> RemoveAsync(TEntity entity)
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var entityInDb = await GetByIdAsync(entity.Id);
            _context.Entry(entityInDb).State = EntityState.Detached;
            _entities.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _isDisposed = true;
            }
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            if (specification == null)
            {
                return _entities;
            }

            return SpecificationEvaluator<TEntity>
                .GetQuery(_entities.AsQueryable(), specification);
        }
    }
}