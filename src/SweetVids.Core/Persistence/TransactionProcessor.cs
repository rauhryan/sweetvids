using System;
using StructureMap;

namespace SweetVids.Core.Persistence
{
    public class TransactionProcessor
    {
        private IContainer _container;

        private readonly object _locker = new object();

        public TransactionProcessor(IContainer container)
        {
            _container = container;
        }

        private void execute(Action<IContainer> action)
        {
            IContainer container = null;

            lock (_locker)
            {
                container = _container;
            }

            // This is using the new "Nested" Container feature of StructureMap
            // to create an entirely new Container object that is "scoped" to
            // this action
            using (IContainer nestedContainer = container.GetNestedContainer())

            using (var boundary = nestedContainer.GetInstance<ITransactionBoundary>())
            {
                try
                {
                    boundary.Start();
                    action(nestedContainer);
                    boundary.Commit();

                }
                catch
                {
                    boundary.Rollback();
                    throw;
                }
                finally
                {
                    boundary.Dispose();
                }
            }
        }

        public void WithinTransaction(Action<IContainer> action)
        {
            execute(action);
        }
    }
}