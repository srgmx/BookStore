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

        public virtual async Task<TEntity> GetAsync(ISpecification<TEntity> specification = null)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _entities.FindAsync(id);
        }

        public virtual void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            // Step 1: Get existing entry
            var entityInDb = await GetByIdAsync(entity.Id);

            // Step 2: Set modification
            var entityToReturn = SetModification(entityInDb, entity);

            return entityToReturn;
        }

        /// <summary>
        /// Sets an entity update logic, maps properties for update.
        /// Must return result updated entity.
        /// </summary>
        /// <param name="entityInDb">Entity is existed in the database.</param>
        /// <param name="entity">Entity contains properties with updated values.</param>
        /// <returns>Result entity with updated properties.</returns>
        protected virtual TEntity SetModification(TEntity entityInDb, TEntity entity)
        {
            _context.Entry(entityInDb).State = EntityState.Detached;
            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        protected IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
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
