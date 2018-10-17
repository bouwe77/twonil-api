using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TwoNil.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public virtual void Add(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                Add(entity);
        }

        public virtual void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public async Task<T> Find(params object[] keyParams)
        {
            return await _context.Set<T>().FindAsync(keyParams);
        }

        public virtual void Update(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                Update(entity);
        }

        public virtual void Update(T entity)
        {
            var currentState = _context.Entry(entity).State;
            if (currentState != EntityState.Added && currentState != EntityState.Deleted)
            {
                _context.Set<T>().Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
        }

        public virtual void Remove(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                Remove(entity);
        }

        public virtual void Remove(T entity)
        {
            var currentState = _context.Entry(entity).State;
            if (currentState == EntityState.Detached)
                _context.Set<T>().Attach(entity);

            _context.Set<T>().Remove(entity);
        }

        public virtual IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return query;
        }
    }
}
