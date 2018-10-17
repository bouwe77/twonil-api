using System.Data.Entity.Infrastructure;

namespace TwoNil.Data
{
    public class DbContextFactory
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DbContextFactory(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        /// <summary>
        /// Creates a DbContext instance using the current session's credentials.
        /// </summary>
        public TwoNilDbContext CreateContext()
        {
            var dbConnection = _dbConnectionFactory.CreateConnection("nameOfConnectionString...");
            return new TwoNilDbContext(dbConnection, true);
        }
    }
}
