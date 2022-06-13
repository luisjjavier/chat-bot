using System.Linq.Expressions;

namespace ChatBot.Core.Boundaries.Persistence
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Table { get; }

        Task<IList<T>> GetAllAsync();

        Task InsertAsync(T entity);

        Task UpdateAsync(T entity);

        Task UpdateAsync(params T[] entities);

        Task DeleteAsync(T entity);

        Task<T> FirstAsNoTracking(Expression<Func<T, bool>> predicate = null,
            params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> WhereAsNoTracking(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);
    }
}
