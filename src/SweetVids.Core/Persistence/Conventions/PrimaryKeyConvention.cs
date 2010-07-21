using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace SweetVids.Core.Persistence.Conventions
{
    public class PrimaryKeyConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.Column("Id");
        }
    }
}