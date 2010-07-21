using System;
using StructureMap;

namespace SweetVids.Core.Persistence
{
    public static class With
    {
        public static void UnitOfWork(Action action_To_Be_Performed)
        {
            var trans = ObjectFactory.GetInstance<NHibernateTransactionBoundary>();

            trans.Start();

            
                try
                {
                    action_To_Be_Performed();
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
                finally
                {
                    trans.Dispose();
                }
            
        }
    }
}