using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace SweetVids.Core.Persistence.Conventions
{
    public class CustomHasManyConvention : IHasManyConvention
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Access.CamelCaseField(CamelCasePrefix.Underscore);
            instance.Cascade.SaveUpdate();
        }
    }
}