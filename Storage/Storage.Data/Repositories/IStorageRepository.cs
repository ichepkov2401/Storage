using Microsoft.EntityFrameworkCore;
using Storage.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Data.Repositories
{
    public interface IStorageRepository
    {
        public Task<IQueryable<TEntity>> Get<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            ICollection<Expression<Func<TEntity, object>>> includes = null) where TEntity : class, IBaseEntity;

        public Task<TEntity> GetOne<TEntity>(
            Expression<Func<TEntity, bool>> filter,
            ICollection<Expression<Func<TEntity, object>>> includes = null) where TEntity : class, IBaseEntity;

        public Task Add<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;

        public Task Update<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;

        public Task Delete<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
    }
}
