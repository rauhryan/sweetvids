using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;

namespace SweetVids.Core.Persistence.Conventions
{
    public class CustomManyToManyTableNameConvention
        : ManyToManyTableNameConvention
    {
        protected override string GetBiDirectionalTableName(IManyToManyCollectionInspector collection, IManyToManyCollectionInspector otherSide)
        {
            return collection.EntityType.Name + "To" + otherSide.EntityType.Name;
        }

        protected override string GetUniDirectionalTableName(IManyToManyCollectionInspector collection)
        {
            return collection.EntityType.Name + "To" + collection.ChildType.Name;
        }
    }
}