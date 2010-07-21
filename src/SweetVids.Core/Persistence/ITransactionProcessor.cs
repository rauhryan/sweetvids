using System;
using StructureMap;

namespace SweetVids.Core.Persistence
{
    public interface ITransactionProcessor
    {
        void WithinTransaction(Action<IContainer> action);
    }
}