using GigaComic.Core.Entities;
using GigaComic.Core.Interfaces;
using GigaComic.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GigaComic.Services
{
    public abstract class BaseService<T> where T : class, IEntity
    {
        public BaseService(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public AppDbContext DbContext { get; init; }

        private DbSet<T> DbSet => DbContext.Set<T>();
        private bool isWaitingSave;

        public virtual void Add(T item)
        {
            DbSet.Add(item);

            if (!isWaitingSave)
            {
                SaveChanges();
            }
        }

        public virtual IQueryable<T> Get()
        {
            return DbSet;
        }

        public virtual T GetSingle(Expression<Func<T, bool>> filter)
        {
            return Get().Single(filter);
        }

        public virtual T? Get(params object[] primaryKey)
        {
            return DbSet.Find(primaryKey);
        }

        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> filter)
        {
            return await Get().SingleAsync(filter);
        }

        public virtual IList<T> Get(Expression<Func<T, bool>> filter)
        {
            return Get().Where(filter).ToList();
        }

        public virtual int Count(Expression<Func<T, bool>> filter)
        {
            if (filter != null)
            {
                return Get().Where(filter).Count();
            }
            return Get().Count();
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            if (filter != null)
            {
                return await Get().Where(filter).CountAsync();
            }
            return await Get().CountAsync();
        }

        public virtual void Update(T item)
        {
            DbSet.Update(item);

            if (!isWaitingSave)
            {
                SaveChanges();
            }
        }

        public virtual void UpdateRange(params T[] items)
        {
            DbSet.UpdateRange(items);

            if (!isWaitingSave)
            {
                SaveChanges();
            }
        }

        public void Remove(params long[] primaryKey)
        {
            var item = Get(primaryKey);
            if (item != null)
            {
                Remove(item);
            }
        }

        public virtual void Remove(T item)
        {
            DbSet.Remove(item);
            if (!isWaitingSave)
            {
                SaveChanges();
            }
        }

        public virtual void Remove(Expression<Func<T, bool>> filter)
        {
            DbSet.RemoveRange(DbSet.Where(filter));
            if (!isWaitingSave)
            {
                SaveChanges();
            }
        }

        public virtual void RemoveRange(params T[] items)
        {
            DbSet.RemoveRange(items);
            if (!isWaitingSave)
            {
                SaveChanges();
            }
        }

        public virtual void SaveChanges()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in DbContext.ChangeTracker.Entries())
            {
                if (entry.Entity is BaseRecord record)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            record.CreatedAt = now;
                            record.UpdatedAt = now;
                            break;

                        case EntityState.Modified:
                            record.UpdatedAt = now;
                            break;
                    }
                }
            }

            DbContext.SaveChanges();
            isWaitingSave = false;
        }
    }

}
