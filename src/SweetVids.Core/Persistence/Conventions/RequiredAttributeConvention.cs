using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using SweetVids.Core.Validation;

namespace SweetVids.Core.Persistence.Conventions
{
    public class RequiredAttributeConvention: AttributePropertyConvention<RequiredAttribute>
    {
        protected override void Apply(RequiredAttribute attribute, IPropertyInstance instance)
        {
            instance.Not.Nullable();
        }
    }
}