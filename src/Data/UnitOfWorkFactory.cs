using System;
using System.Data.Entity.Infrastructure;

namespace TwoNil.Data
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly DbContextFactory _contextFactory;
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public UnitOfWorkFactory()
        {
            //TODO deze repository moet weg, onderstaande gebruiken, maar dan heb DI nodig...
            throw new NotImplementedException();
        }

        public UnitOfWorkFactory(DbContextFactory contextFactory, IDbConnectionFactory dbConnectionFactory)
        {
            _contextFactory = contextFactory;
            _dbConnectionFactory = dbConnectionFactory;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(_contextFactory, _dbConnectionFactory);
        }
    }
}
