using System;
using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Database
{
   public class TransactionManager : SqliteRepository, IDisposable
   {
      private readonly List<Transaction> _transactions;

      internal TransactionManager(string databaseFilePath)
         : base(databaseFilePath)
      {
         _transactions = new List<Transaction>();
      }

      private void AddTransaction(DomainObjectBase domainObject, Action<DomainObjectBase> operation)
      {
         Assert(domainObject);

         var transaction = new Transaction
         {
            DomainObject = domainObject,
            Operation = operation
         };

         _transactions.Add(transaction);
      }

      public void RegisterInsert(DomainObjectBase domainObject)
      {
         RegisterInsert(new [] { domainObject });
      }

      public void RegisterInsert(IEnumerable<DomainObjectBase> domainObjects)
      {
         foreach (var domainObject in domainObjects)
         {
            AddTransaction(domainObject, Insert);
         }
      }

      public void RegisterUpdate(DomainObjectBase domainObject)
      {
         RegisterUpdate(new[] { domainObject });
      }

      public void RegisterUpdate(IEnumerable<DomainObjectBase> domainObjects)
      {
         foreach (var domainObject in domainObjects)
         {
            AddTransaction(domainObject, Update);
         }
      }

      public void RegisterDelete(DomainObjectBase domainObject)
      {
         RegisterDelete(new[] { domainObject });
      }

      public void RegisterDelete(IEnumerable<DomainObjectBase> domainObjects)
      {
         foreach (var domainObject in domainObjects)
         {
            AddTransaction(domainObject, Delete);
         }
      }

      public void Save()
      {
         // Do not connect until the actual save is requested.
         Connect();

         try
         {
            StartTransaction();

            foreach (var transaction in _transactions)
            {
               transaction.Operation.Invoke(transaction.DomainObject);
            }

            CommitTransaction();

            _transactions.Clear();
         }
         catch (Exception)
         {
            RollbackTransaction();
            throw;
         }
      }

      public IEnumerable<Transaction> GetTransactions()
      {
         return _transactions;
      }

      private void Insert(DomainObjectBase domainObject)
      {
         domainObject.LastModified = GetLastModified();
         Connection.Insert(domainObject);
      }

      private void Update(DomainObjectBase domainObject)
      {
         domainObject.LastModified = GetLastModified();
         Connection.Update(domainObject);
      }

      private void Delete(DomainObjectBase domainObject)
      {
         Connection.Delete(domainObject);
      }

      private string GetLastModified()
      {
         return Format(DateTime.UtcNow);
      }
      
      private void Assert(DomainObjectBase domainObject)
      {
         if (domainObject == null)
         {
            throw new ArgumentNullException(nameof(domainObject));
         }
      }

      private void StartTransaction()
      {
         Connection.BeginTransaction();
      }

      private void CommitTransaction()
      {
         Connection.Commit();
      }

      private void RollbackTransaction()
      {
         Connection.Rollback();
      }
   }
}
