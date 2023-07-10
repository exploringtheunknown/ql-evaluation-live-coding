using System.Linq.Expressions;

namespace Standard.API.PSQL.Domain.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(Guid id, IEnumerable<string> entitiesToInclude = null);
        Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<Expression<Func<TEntity, bool>>> predicates = null, IEnumerable<string> entitiesToInclude = null);
        Task<IEnumerable<TEntity>> GetAllAsync(int skip, int limit, IEnumerable<Expression<Func<TEntity, bool>>> predicates = null, IEnumerable<string> entitiesToInclude = null);
        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<int> Count(IEnumerable<Expression<Func<TEntity, bool>>> predicates = null);
        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
