using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TwoNil.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Adds the provided entity to the context
        /// </summary>
        void Add(T entity);

        /// <summary>
        /// Adds the provided entities to the context
        /// </summary>
        void Add(IEnumerable<T> entities);

        /// <summary>
        /// Attaches the entity to the current context and marks it as modified
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Attaches the entities to the current context and marks them as modified
        /// </summary>
        void Update(IEnumerable<T> entities);

        /// <summary>
        /// Removes the provided entity from the context
        /// </summary>
        void Remove(T entity);

        /// <summary>
        /// Removes the provided entities from the context
        /// </summary>
        void Remove(IEnumerable<T> entities);

        Task<T> Find(params object[] keyParams);

        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
    }
}
