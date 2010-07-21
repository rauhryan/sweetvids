using System.Data;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;

namespace SweetVids.Core.Persistence
{
    public class NHibernateSessionSource : ISessionSource
    {
        private readonly IConfigurationProperties _configurationProperties;
        private readonly object _factorySyncRoot = new object();
        private Configuration _configuration;
        private ISessionFactory _sessionFactory;

        public NHibernateSessionSource(IConfigurationProperties configurationProperties)
        {
            _configurationProperties = configurationProperties;
            if(_sessionFactory != null) return;

            lock(_factorySyncRoot)
            {
                if(_sessionFactory != null) return;

                _configuration = AssembleConfiguration(null);
                _sessionFactory = _configuration.BuildSessionFactory();
            }
        }

        public ISessionSource CreateSessionSource()
        {
            var source = new SessionSource(BuildFluentConfig());
            return source;
        }

        public Configuration AssembleConfiguration(string mappingExportPath)
        {
            return BuildFluentConfig().BuildConfiguration();
        }

        private FluentConfiguration BuildFluentConfig()
        {
            return Fluently.Configure()
                .Database(new PersistenceConfigurer(_configurationProperties))
                .Mappings(x =>
                              {
                                  x.AutoMappings.Add(new AutoPersistenceModelGenerator().Generate());
                                  //x.FluentMappings.AddFromAssemblyOf<RegionMap>();
                                  //x.AutoMappings.ExportTo("d:\\code\\coachesaid3\\MappingFiles");
                              })
                .ExposeConfiguration(config => config.Properties.Add("prepare_sql", "true"));
        }


        
        

        public ISession CreateSession()
        {
            return _sessionFactory.OpenSession();
        }

        public void BuildSchema()
        {
            ISession session = CreateSession();
            IDbConnection connection = session.Connection;

            Dialect dialect = Dialect.GetDialect(AssembleConfiguration(null).Properties);
            string[] drops = _configuration.GenerateDropSchemaScript(dialect);
            executeScripts(drops, connection);

            string[] scripts = _configuration.GenerateSchemaCreationScript(dialect);

            executeScripts(scripts, connection);
        }

        private static void executeScripts(string[] scripts, IDbConnection connection)
        {
            foreach (var script in scripts)
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = script;
                command.ExecuteNonQuery();
            }
        }
    }
    public class PersistenceConfigurer : IPersistenceConfigurer
    {
        private readonly IConfigurationProperties _settings;

        public PersistenceConfigurer(IConfigurationProperties settings)
        {
            _settings = settings;
        }

        #region IPersistenceConfigurer Members

        public Configuration ConfigureProperties(Configuration nhibernateConfig)
        {
            return nhibernateConfig.SetProperties(_settings.GetProperties());
        }

        #endregion
    }
}