using System;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Database
{
   public class Transaction
   {
      public DomainObjectBase DomainObject { get; set; }
      public Action<DomainObjectBase> Operation { get; set; }
   }
}
