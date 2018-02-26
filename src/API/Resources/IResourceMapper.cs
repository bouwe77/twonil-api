using Sally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public interface IResourceMapper<TDomainObject> where TDomainObject : DomainObjectBase, new()
   {
      Resource Map(TDomainObject domainObject, params string[] properties);
   }
}