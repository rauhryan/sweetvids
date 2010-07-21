using System;
using System.Collections;
using System.Collections.Generic;
using SweetVids.Core.Persistence;
using FubuCore.Binding;
using StructureMap;
using StructureMap.Pipeline;

namespace SweetVids.Core.Util
{
    public static class StructureMapExtensions
    {
        public static void ExecuteInTransaction(this IContainer container, Action<IContainer> action)
        {
            var transactionProcessor = new TransactionProcessor(container);
            transactionProcessor.WithinTransaction(action);

        }

        public static void StartStartables(this IContainer container)
        {
            container.ExecuteInTransaction(c => c.Model.GetAllPossible<IStartable>()
                                                    .Each(x => x.Start()));

        }
    }

    public static class ServiceArgumentsExtensions
    {
        public static ExplicitArguments ToExplicitArgs(this ServiceArguments arguments)
        {
            var explicits = new ExplicitArguments();
            arguments.EachService(explicits.Set);

            return explicits;
        }
    }

    public static class TypeExtensions
    {
        public static bool Is<T>(this object target)
        {
            return typeof(T).IsAssignableFrom(target.GetType());
        }

        public static bool IsType<T>(this Type type)
        {
            return type == typeof(T);
        }

        public static void Add<T>(this List<Type> list)
        {
            list.Add(typeof(T));
        }

        public static void CallOn<T>(this object target, Action<T> action) where T : class
        {
            var subject = target as T;
            if (subject != null)
            {
                try
                {
                    action(subject);
                }
                catch (InvalidOperationException e)
                {
                    if (!e.ToString().Contains("The calling thread"))
                    {
                        throw;
                    }
                }
            }
        }

        public static void CallOnEach<T>(this IEnumerable enumerable, Action<T> action) where T : class
        {
            if (enumerable == null) return;
            foreach (object o in enumerable)
            {
                o.CallOn(action);
            }
        }


    }
}