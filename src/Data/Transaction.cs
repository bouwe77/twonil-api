using System;
using System.Data.Entity;

namespace TwoNil.Data
{
    internal class Transaction : ITransaction, IDisposable
    {
        private readonly DbContextTransaction _dbContextTransaction;

        public Transaction(DbContextTransaction dbContextTransaction)
        {
            _dbContextTransaction = dbContextTransaction;
        }

        public void Commit()
        {
            _dbContextTransaction.Commit();
        }

        public void Rollback()
        {
            _dbContextTransaction.Commit();
        }

        public void Dispose()
        {
            _dbContextTransaction.Dispose();
        }
    }
}
