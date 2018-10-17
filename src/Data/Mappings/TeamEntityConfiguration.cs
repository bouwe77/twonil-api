using System.Data.Entity.ModelConfiguration;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Mappings
{
    public class TeamEntityConfiguration : EntityTypeConfiguration<Team>
    {
        public TeamEntityConfiguration()
        {
        }
    }
}
