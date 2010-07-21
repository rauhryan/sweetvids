using System;
using FluentNHibernate;
using NHibernate;

namespace SweetVids.Core.Persistence
{
    public class NHibernateTransactionBoundary : ITransactionBoundary
    {
        private readonly ISessionSource _sessionSource;
        private bool _isInitialized;
        private ISession _session;
        private ITransaction _transaction;

        public NHibernateTransactionBoundary(ISessionSource sessionSource)
        {
            _sessionSource = sessionSource;
        }

        public ISession Session
        {
            get
            {
                ensure_initialized();
                return _session;
            }
        }

        public bool IsDisposed { get; private set; }

        public void Start()
        {
            _session = _sessionSource.CreateSession();
            _session.FlushMode = FlushMode.Commit;
            _transaction = _session.BeginTransaction();
            _isInitialized = true;
        }

        public void Commit()
        {
            should_not_be_disposed();
            ensure_initialized();
            _transaction.Commit();
        }

        public void Rollback()
        {
            should_not_be_disposed();
            ensure_initialized();
            _transaction.Rollback();

            _transaction = _session.BeginTransaction();
        }

        public void Dispose()
        {
            IsDisposed = true;
            if(_transaction != null)  _transaction.Dispose();
            if(_session != null) _session.Dispose();
        }

        private void should_not_be_disposed()
        {
            if(! IsDisposed) return;
            throw new ObjectDisposedException("NHibernateTransactionBoundary");
        }

        private void ensure_initialized()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("An attempt was made to access the database session outsite of a transaction. Please make sure all access is made within an initialized transaction boundary.");
            }
        }
    }
}