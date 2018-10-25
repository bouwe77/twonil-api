using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Repositories
{
    public class MemoryRepository<TEntity> : IGenericRepository<TEntity> where TEntity : DomainObjectBase
    {
        protected IEnumerable<TEntity> Entities { get; set; }
        protected InMemoryData InMemoryData;

        public MemoryRepository()
        {
            InMemoryData = new InMemoryData();
        }

        public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            return Entities.AsQueryable();
        }

        public Task<TEntity> Find(params object[] keyParams) => throw NotSupported();
        public void Add(TEntity entity) => throw NotSupported();
        public void Update(TEntity entity) => throw NotSupported();
        public void Remove(TEntity entity) => throw NotSupported();
        public void Add(IEnumerable<TEntity> entities) => throw NotSupported();
        public void Update(IEnumerable<TEntity> entities) => throw NotSupported();
        public void Remove(IEnumerable<TEntity> entities) => throw NotSupported();

        private NotSupportedException NotSupported()
        {
            return new NotSupportedException("In-memory repositories do not support this operation");
        }
    }
}
