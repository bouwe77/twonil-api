using System;

namespace TwoNil.Data
{
    /// <summary>
    // Represents a unit of work performed within a database management system.
    /// </summary>
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
