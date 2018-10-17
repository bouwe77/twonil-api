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

        public void Add(TEntity entity)
        {
            throw new NotSupportedException("Adding is not supported for in-memory repositories");
        }

        public void Update(TEntity entity)
        {
            throw new NotSupportedException("Updating is not supported for in-memory repositories");
        }

        public void Remove(TEntity entity)
        {
            throw new NotSupportedException("Removing is not supported for in-memory repositories");
        }

        public Task<TEntity> Find(params object[] keyParams)
        {
            //TODO Deze nog implemeteren?
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            return Entities.AsQueryable();
        }
    }
}
