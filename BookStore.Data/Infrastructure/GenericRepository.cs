using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookStore.Data.Contract;
using BookStore.Data.Persistance;
using BookStore.Data.Specifications;


namespace BookStore.Data.Infrastructure
{
    public class GenericRepository<TEntity> : 
        IGenericRepository<TEntity>, IDisposable where TEntity : class
    {
        private readonly BookStoreDbContext _context;
        private DbSet<TEntity> _entities;
        private bool isDisposed;

        public GenericRepository(BookStoreDbContext context)
        {
            _context = context;
            _entities = _context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(ISpecification<TEntity> specification)
        {
            return await this.ApplySpecification(specification).ToListAsync();
        }

        public async Task<TEntity> FindAsync(ISpecification<TEntity> specification)
        {
            return await this.ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<TEntity> FindByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<TEntity> RemoveAsync(TEntity entity)
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
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
            if (!isDisposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                isDisposed = true;
            }
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity>
                .GetQuery(_entities.AsQueryable(), specification);
        }
    }
}