using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Repositories
{
   public class MemoryRepository<TEntity> : IRepository<TEntity> where TEntity : DomainObjectBase
   {
      protected IEnumerable<TEntity> Entities { get; set; }
      protected InMemoryData InMemoryData;

      public MemoryRepository()
      {
         InMemoryData = new InMemoryData();
      }

      public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
      {
         return Entities.AsQueryable().Where(predicate.Compile()).ToList();
      }

      public IEnumerable<TEntity> GetAll()
      {
         return Entities;
      }

      public TEntity GetOne(string id)
      {
         return Entities.SingleOrDefault(entity => entity.Id == id);
      }
      
      public void Dispose()
      {
         // Nothing to dispose.
      }

      public List<TEntity> ExecuteQuery(string query)
      {
         throw new NotImplementedException("Querying is not supported on in-memory collections");
      }
   }
}
