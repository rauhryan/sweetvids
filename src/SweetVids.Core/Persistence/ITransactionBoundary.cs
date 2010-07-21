using System;

namespace SweetVids.Core.Persistence
{
    public interface ITransactionBoundary : IDisposable
    {
        void Start();
        void Commit();
        void Rollback();
    }
}