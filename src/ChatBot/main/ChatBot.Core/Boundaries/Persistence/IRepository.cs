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
    }
}
