using Microsoft.EntityFrameworkCore;
using Storage.Data.Entity;
using System.Formats.Tar;
using System.Linq.Expressions;

namespace Storage.Data.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        private readonly StorageDbContext _context;

        public StorageRepository(StorageDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<TEntity>> Get<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            ICollection<Expression<Func<TEntity, object>>> includes = null) where TEntity : class, IBaseEntity
        {
            return Prepare(filter, orderBy, skip, take, includes);
        }

        public async Task<TEntity> GetOne<TEntity>(
            Expression<Func<TEntity, bool>> filter,
            ICollection<Expression<Func<TEntity, object>>> includes = null) where TEntity : class, IBaseEntity
        {
            IQueryable<TEntity> set = Prepare(filter, null, null, null, includes);
            if (set.Count() > 1)
            {
                throw new AggregateException("Found more than one elements");
            }

            if (set.Count() == 1)
            {
                return set.Single();
            }

            return null;
        }

        public async Task Add<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            DbSet<TEntity> set = _context.Set<TEntity>();
            set.Add(entity);
            entity.CreatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task Update<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        { 
            entity.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task Delete<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            entity.DeletedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        private IQueryable<TEntity> Prepare<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            ICollection<Expression<Func<TEntity, object>>> includes = null) where TEntity : class, IBaseEntity
        {
            IQueryable<TEntity> set = _context.Set<TEntity>();
            set = set.Where(x => !x.DeletedDate.HasValue);
            if (filter != null)
            {
                set = set.Where(filter);
            }
            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    set = set.Include(includeProperty);
                }
            }
            if (orderBy != null)
            {
                set = orderBy(set);
            }
            if (skip.HasValue)
            {
                set = set.Skip(skip.Value);
            }
            if (take.HasValue)
            {
                set = set.Take(take.Value);
            }
            return set;
        }
    }
}
