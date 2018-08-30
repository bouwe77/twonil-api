using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Repositories
{
   public class ReadRepository<TEntity> : SqliteRepository, IRepository<TEntity> where TEntity : DomainObjectBase, new()
   {
      private string _gameId;

      internal ReadRepository(string databaseFilePath, string gameId)
         : base(databaseFilePath)
      {
         _gameId = gameId;
         Connect();
      }

      public List<TEntity> ExecuteQuery(string query)
      {
         var entities = Connection.Query<TEntity>(query);
         AddGameId(entities);
         return entities;
      }

      public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
      {
         var entities = Connection.Table<TEntity>().AsQueryable().Where(predicate.Compile()).ToList();
         AddGameId(entities);
         return entities;
      }

      public IEnumerable<TEntity> GetAll()
      {
         var entities = Connection.Table<TEntity>().ToList();
         AddGameId(entities);
         return entities;
      }

      public TEntity GetOne(string id)
      {
         var entity = Find(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
         AddGameId(entity);
         return entity;
      }

      private void AddGameId(IEnumerable<TEntity> entities)
      {
         foreach (var entity in entities)
         {
            AddGameId(entity);
         }
      }

      private void AddGameId(TEntity entity)
      {
         entity.GameId = _gameId;
      }
   }
}
