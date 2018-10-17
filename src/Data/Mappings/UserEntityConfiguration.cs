using System.Data.Entity.ModelConfiguration;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Mappings
{
    public class UserEntityConfiguration : EntityTypeConfiguration<User>
    {
        public UserEntityConfiguration()
        {
        }
    }
}
