using Shally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
   public interface IResourceMapper<in TDomainObject> where TDomainObject : DomainObjectBase, new()
   {
      Resource Map(TDomainObject domainObject, params string[] properties);
   }
}