using Microsoft.EntityFrameworkCore;
using Storage.Data.Entity;
using System.Formats.Tar;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Storage.Data.Repositories
{
    public class StorageRepository 
    {
        private readonly StorageDbContext _context;

        public StorageRepository(StorageDbContext context)
        {
            _context = context;
        }

        private IQueryable<TEntity> Prepare<TEntity>(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            ICollection<Expression<Func<TEntity, object>>> includes = null) where TEntity : class
        {
            IQueryable<TEntity> set = _context.Set<TEntity>();
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
