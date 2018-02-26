using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data
{
   public interface IRepository<TEntity> : IDisposable where TEntity : DomainObjectBase
   {
      IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

      IEnumerable<TEntity> GetAll();

      TEntity GetOne(string id);

      List<TEntity> ExecuteQuery(string query);
   }
}
