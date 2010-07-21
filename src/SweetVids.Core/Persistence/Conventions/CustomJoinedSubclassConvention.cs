using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace SweetVids.Core.Persistence.Conventions
{
    public class CustomJoinedSubclassConvention : IJoinedSubclassConvention
    {
        public void Apply(IJoinedSubclassInstance instance)
        {
            instance.Key.Column("Id");
            instance.Key.ForeignKey("fk_" + instance.EntityType.Name);
        }
    }
}