using System;
using System.Data.Common;
using System.Data.Entity;
using TwoNil.Data.Mappings;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data
{
    public class TwoNilDbContext : DbContext
    {
        public TwoNilDbContext(DbConnection dbConnection, bool contextOwnsConnection) : base(dbConnection, contextOwnsConnection)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.UseDatabaseNullSemantics = true;
#if DEBUG
            Database.Log = Console.WriteLine;
#endif
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserEntityConfiguration());
            modelBuilder.Configurations.Add(new TeamEntityConfiguration());
        }
    }
}
