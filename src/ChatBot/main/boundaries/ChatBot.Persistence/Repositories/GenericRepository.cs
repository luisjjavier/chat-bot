using ChatBot.Core.Boundaries.Persistence;
using Microsoft.EntityFrameworkCore;
using Triplex.Validations;

namespace ChatBot.Persistence.Repositories
{
    public  class GenericRepository <T>: IRepository<T> where T : class
    {
        private readonly ChatDbContext _chatDbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ChatDbContext chatDbContext)
        {
            _chatDbContext = chatDbContext;
            _dbSet = _chatDbContext.Set<T>();
        }

        public IQueryable<T> Table => _dbSet;
        public async Task<IList<T>> GetAllAsync() 
            => await _dbSet.AsNoTracking().ToListAsync();

        public async Task InsertAsync(T entity)
        {
            Arguments.NotNull(entity, nameof(entity));

            await _dbSet.AddAsync(entity);
            await _chatDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(params T[] entities)
        {
            Arguments.NotNull(entities, nameof(entities));

            foreach (var entity in entities)
            {
                _dbSet.Attach(entity);
                _chatDbContext.Entry(entity).State = EntityState.Modified;
            }

            await _chatDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            Arguments.NotNull(entity, nameof(entity));

            _dbSet.Remove(entity);
            await _chatDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _chatDbContext.Entry(entity).State = EntityState.Modified;

            await _chatDbContext.SaveChangesAsync();
        }
    }
}
