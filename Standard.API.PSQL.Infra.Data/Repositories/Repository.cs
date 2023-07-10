﻿using Microsoft.EntityFrameworkCore;
using Standard.API.PSQL.Domain.Entities;
using Standard.API.PSQL.Domain.Repository;
using Standard.API.PSQL.Infra.Data.Helpers;
using System.Linq.Expressions;

namespace Standard.API.PSQL.Infra.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbSet<TEntity> _entities;

        public Repository(DbSet<TEntity> entities) => _entities = entities;

        public async Task<TEntity> AddAsync(TEntity entity) => (await _entities.AddAsync(entity)).Entity;
        public async Task<TEntity> UpdateAsync(TEntity entity) => await Task.Run(() => { return _entities.Update(entity).Entity; });
        public async Task<TEntity> GetByIdAsync(Guid id, IEnumerable<string> entitiesToInclude) => await _entities.Include(entitiesToInclude).FirstOrDefaultAsync(e => e.Id == id);

        public async Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<Expression<Func<TEntity, bool>>> predicates,
                                                            IEnumerable<string> entitiesToInclude) => await _entities.Filter(predicates)
                                                                                                                     .Include(entitiesToInclude)
                                                                                                                     .ToListAsync();
        public async Task<IEnumerable<TEntity>> GetAllAsync(int skip, int limit, IEnumerable<Expression<Func<TEntity, bool>>> predicates,
                                                                                 IEnumerable<string> entitiesToInclude) => await _entities.Filter(predicates)
                                                                                                                                          .Skip(skip)
                                                                                                                                          .Take(limit)
                                                                                                                                          .Include(entitiesToInclude)
                                                                                                                                          .ToListAsync();
        public void Delete(TEntity entity) => _entities.Remove(entity);
        public async Task DeleteAsync(TEntity entity) => await Task.Run(() => { Delete(entity); });
        public async virtual Task<int> Count(IEnumerable<Expression<Func<TEntity, bool>>>? predicates = null) => await _entities.Filter(predicates).CountAsync();
        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate) => await _entities.AnyAsync(predicate);
    }
}