using SweetVids.Core.Persistence;

using FluentNHibernate;
using FubuMVC.Core.Configuration;
using NHibernate;
using StructureMap.Configuration.DSL;

namespace FieldBook.Tests.SchemaCreation
{
    public class SchemaRegistry : Registry
    {
        public SchemaRegistry()
        {
            SetupNHibernate();
        }

        private void SetupNHibernate()
        {
            ForSingletonOf<ISessionSource>().Use<NHibernateSessionSource>();

            For<ITransactionBoundary>().Use<NHibernateTransactionBoundary>();
            For<ISession>().Use(c =>
            {
                var trans = ((NHibernateTransactionBoundary)c.GetInstance<ITransactionBoundary>());
                return trans.Session;
            });

            For<ITransactionBoundary>().Use<NHibernateTransactionBoundary>();
            For<IConfigurationProperties>().Use(c =>
            {
                var settingsProvider = c.GetInstance<ISettingsProvider>();
                return settingsProvider.SettingsFor<DatabaseSettings>();
            });
        }
    }
}