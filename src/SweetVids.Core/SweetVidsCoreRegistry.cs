using System;
using SweetVids.Core.Persistence;
using FluentNHibernate;
using FubuMVC.Core.Configuration;
using NHibernate;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace SweetVids.Core
{
    public class SweetVidsCoreRegistry : Registry
    {
        public SweetVidsCoreRegistry()
        {
            setupNHibernate();
            Scan(x =>
            {
                x.TheCallingAssembly();
                x.WithDefaultConventions();
                x.Convention<SettingsConvention>();
            });


        }

        private void setupNHibernate()
        {

            ForSingletonOf<ISessionSource>().Use<NHibernateSessionSource>();

            For<ISession>().Use(c =>
            {
                var transaction = (NHibernateTransactionBoundary)c.GetInstance<ITransactionBoundary>();
                return transaction.Session;
            });


            For<IConfigurationProperties>().Use(c =>
            {
                var settingsProvider = c.GetInstance<ISettingsProvider>();
                return settingsProvider.SettingsFor<DatabaseSettings>();
            });

            For<ITransactionBoundary>().Use<NHibernateTransactionBoundary>();

            For(typeof(IRepository<>)).Use(typeof(NHibernateRepository<>));

        }


    }
    public class SettingsConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract || type.IsInterface)
                return;
            if (type.Name.EndsWith("Settings"))
                registry.For(type).Use(x =>
                {
                    var provider = x.GetInstance<ISettingsProvider>();
                    return provider.SettingsFor(type);
                });
        }
    }
}
