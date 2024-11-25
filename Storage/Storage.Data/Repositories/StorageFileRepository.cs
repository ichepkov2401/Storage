using Microsoft.EntityFrameworkCore;
using Storage.Data.Contexts;
using Storage.Data.Entity;
using System.Linq.Expressions;

namespace Storage.Data.Repositories
{
    public class StorageFileRepository : IStorageRepository
    {
        private static StorageFileContext context = null;

        public StorageFileRepository(string connection) 
        {
            if (context == null)
                context = new StorageFileContext(connection);
        }


        async Task IStorageRepository.Add<TEntity>(TEntity entity)
        {
            List<TEntity> set = context.Set<TEntity>();
            set.Add(entity);
            entity.CreatedDate = DateTime.UtcNow;
            context.SaveChanges();
        }

        async Task IStorageRepository.Delete<TEntity>(TEntity entity)
        {
            entity.DeletedDate = DateTime.UtcNow;
            context.SaveChanges();
        }

        async Task<IQueryable<TEntity>> IStorageRepository.Get<TEntity>(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int? skip, int? take, ICollection<Expression<Func<TEntity, object>>> includes)
        {
            return Prepare(filter, orderBy, skip, take, includes);
        }

        async Task<TEntity> IStorageRepository.GetOne<TEntity>(Expression<Func<TEntity, bool>> filter, ICollection<Expression<Func<TEntity, object>>> includes)
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

        async Task IStorageRepository.Update<TEntity>(TEntity entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;
            context.SaveChanges();
        }

        private IQueryable<TEntity> Prepare<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            ICollection<Expression<Func<TEntity, object>>> includes = null) where TEntity : class, IBaseEntity
        {
            IQueryable<TEntity> set = context.Set<TEntity>().AsQueryable();
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
