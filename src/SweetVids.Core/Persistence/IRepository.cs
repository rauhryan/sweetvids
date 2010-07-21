#region Using Directives

using System;

using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluentNHibernate.Utils;
using SweetVids.Core.Domain;
using NHibernate;
using NHibernate.Connection;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Linq;


#endregion

namespace SweetVids.Core.Persistence
{

    public interface IRepository<T> where T : Entity
    {
        void Save(T entity);

        T Load(Guid id);

        T Get(Guid id);

        T FindBy<U>(System.Linq.Expressions.Expression<Func<T, U>> expression, U search);

        IQueryable<T> Query();

        IQueryable<T> Query(IDomainQuery<T> whereQuery);

        void Delete(T entity);

        void DeleteAll();

        IEnumerable<T> FindAll(params ICriterion[] criteria);

   

        /// <summary>
        /// Execute the specified stored procedure with the given parameters and then converts
        /// the results using the supplied delegate.
        /// </summary>
        /// <typeparam name="T2">The collection type to return.</typeparam>
        /// <param name="converter">The delegate which converts the raw results.</param>
        /// <param name="sp_name">The name of the stored procedure.</param>
        /// <param name="parameters">Parameters for the stored procedure.</param>
        /// <returns></returns>
        IEnumerable<T2> ExecuteStoredProcedure<T2>(Converter<SafeDataReader, T2> converter, string sp_name,
                                                   params Parameter[] parameters);


        ISQLQuery CreateSQLQuery(string sqlQuery);

        void Evict(T entity);
    }

    public class Parameter
    {
        public Parameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public object Value { get; set; }
    }

    public class NHibernateRepository<T> : IRepository<T> where T : Entity
    {
        private readonly ISession _session;
  

        public NHibernateRepository(ISession session)
        {
            _session = session;
           
        }

        #region IRepository Members

        public void Save(T entity)
        {
            _session.SaveOrUpdate(entity);
   
        }

        public T Load(Guid id)
        {
            return _session.Load<T>(id);
        }

        public T Get(Guid id)
        {
            return _session.Get<T>(id);
        }

        public IQueryable<T> Query()
        {
            return _session.Linq<T>();
        }

        public IQueryable<T> Query(IDomainQuery<T> whereQuery)
        {
            return _session.Linq<T>().Where(whereQuery.Expression);
        }

        public void Delete(T entity)
        {
            _session.Delete(entity);

        }

        public void DeleteAll()
        {
            var query = String.Format("from {0}", typeof(T).Name);
            _session.Delete(query);
        }

        public IEnumerable<T> FindAll(params ICriterion[] criteria)
        {
            var crit = _session.CreateCriteria(typeof(T));
            foreach (var criterion in criteria)
            {
                if (criterion == null) continue;
                crit.Add(criterion);
            }
            return crit.Future<T>();
        }

        public T FindBy<TU>(System.Linq.Expressions.Expression<Func<T, TU>> expression, TU search)
        {
            string propertyName = ReflectionHelper.GetAccessor(expression).FieldName;
            ICriteria criteria =
                _session.CreateCriteria(typeof(T)).Add(
                    Restrictions.Eq(propertyName, search));
            return criteria.UniqueResult() as T;
        }

        /// <summary>
        /// Execute the specified stored procedure with the given parameters and then converts
        /// the results using the supplied delegate.
        /// </summary>
        /// <typeparam name="T2">The collection type to return.</typeparam>
        /// <param name="converter">The delegate which converts the raw results.</param>
        /// <param name="sp_name">The name of the stored procedure.</param>
        /// <param name="parameters">Parameters for the stored procedure.</param>
        /// <returns></returns>
        public IEnumerable<T2> ExecuteStoredProcedure<T2>(Converter<SafeDataReader, T2> converter, string sp_name,
                                                          params Parameter[] parameters)
        {
            IConnectionProvider connectionProvider = ((ISessionFactoryImplementor)_session.SessionFactory).ConnectionProvider;
            IDbConnection connection = connectionProvider.GetConnection();

            try
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sp_name;
                    command.CommandType = CommandType.StoredProcedure;

                    CreateDbDataParameters(command, parameters);
                    var reader = new SafeDataReader(command.ExecuteReader());
                    var results = new List<T2>();

                    while (reader.Read())
                        results.Add(converter(reader));

                    reader.Close();

                    return results;
                }
            }
            finally
            {
                connectionProvider.CloseConnection(connection);
            }
        }

  

        public ISQLQuery CreateSQLQuery(string sqlQuery)
        {
            return _session.CreateSQLQuery(sqlQuery);
        }

        #endregion

        public static void CreateDbDataParameters(IDbCommand command, Parameter[] parameters)
        {
            foreach (Parameter parameter in parameters)
            {
                IDbDataParameter sp_arg = command.CreateParameter();
                sp_arg.ParameterName = parameter.Name;
                sp_arg.Value = parameter.Value;
                command.Parameters.Add(sp_arg);
            }
        }



        public void Evict(T entity)
        {
            _session.Evict(entity);
        }
    }
}