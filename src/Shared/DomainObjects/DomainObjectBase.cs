using System;
using Randomization;
using SQLite;

namespace TwoNil.Shared.DomainObjects
{
   public abstract class DomainObjectBase
   {
      protected DomainObjectBase()
      {
         Id = IdGenerator.GetId().ToLower();
      }

      [PrimaryKey]
      public string Id { get; set; }

      [Ignore]
      public string GameId { get; set; }

      public string LastModified { get; set; }

      protected bool Equals(DomainObjectBase other)
      {
         return string.Equals(Id, other.Id, StringComparison.OrdinalIgnoreCase);
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         if (obj.GetType() != GetType()) return false;
         return Equals((DomainObjectBase)obj);
      }

      public override int GetHashCode()
      {
         return Id?.GetHashCode() ?? 0;
      }
   }
}
