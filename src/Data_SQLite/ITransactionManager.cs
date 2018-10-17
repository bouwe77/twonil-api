using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data
{
    public interface ITransactionManager
    {
        IEnumerable<Transaction> GetTransactions();
        void RegisterDelete(DomainObjectBase domainObject);
        void RegisterDelete(IEnumerable<DomainObjectBase> domainObjects);
        void RegisterInsert(DomainObjectBase domainObject);
        void RegisterInsert(IEnumerable<DomainObjectBase> domainObjects);
        void RegisterUpdate(DomainObjectBase domainObject);
        void RegisterUpdate(IEnumerable<DomainObjectBase> domainObjects);
        void Save();
    }
}