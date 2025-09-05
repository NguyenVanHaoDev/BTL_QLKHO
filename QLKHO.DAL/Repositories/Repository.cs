using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace QLKHO.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public virtual T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void Update(T entity)
        {
            try
            {
                var entry = _context.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }
                entry.State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                // Nếu có lỗi với entity tracking, thử cách khác
                try
                {
                    // Detach entity hiện tại nếu có
                    var existingEntry = _context.Entry(entity);
                    if (existingEntry.State != EntityState.Detached)
                    {
                        existingEntry.State = EntityState.Detached;
                    }
                    
                    // Attach lại và set modified
                    _dbSet.Attach(entity);
                    _context.Entry(entity).State = EntityState.Modified;
                }
                catch
                {
                    // Nếu vẫn lỗi, throw exception gốc (giữ nguyên stack trace)
                    throw;
                }
            }
        }

        public virtual void UpdateSafe(T entity)
        {
            // Cách tiếp cận an toàn hơn cho update
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            
            // Chỉ đánh dấu entity là modified, không set state trực tiếp
            entry.State = EntityState.Modified;
        }

        public virtual void Remove(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public virtual void RemoveById(object id)
        {
            T entity = _dbSet.Find(id);
            if (entity != null)
            {
                Remove(entity);
            }
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public virtual int Count()
        {
            return _dbSet.Count();
        }

        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Count(predicate);
        }

        public virtual IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
}
