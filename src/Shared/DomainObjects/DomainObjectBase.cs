using System;
using Randomization;

namespace TwoNil.Shared.DomainObjects
{
    public abstract class DomainObjectBase
    {
        protected DomainObjectBase()
        {
            Id = IdGenerator.GetId().ToLower();
        }

        public string Id { get; set; }

        public string GameId { get; set; }

        public DateTime LastModified { get; set; }
    }
}
