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
        IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _entities;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _entities = _context.Set<TEntity>();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity) 
        {
            await _entities.AddAsync(entity);

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

        public virtual void RemoveAsync(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var entityInDb = await GetByIdAsync(entity.Id);
            _context.Entry(entityInDb).State = EntityState.Detached;
            _entities.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            return entity;
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