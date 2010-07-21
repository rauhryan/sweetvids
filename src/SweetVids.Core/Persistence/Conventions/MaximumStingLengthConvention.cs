using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using SweetVids.Core.Validation;

namespace SweetVids.Core.Persistence.Conventions
{
    public class MaximumStingLengthConvention : AttributePropertyConvention<MaximumStringLengthAttribute>
    {
        protected override void Apply(MaximumStringLengthAttribute attribute, IPropertyInstance instance)
        {
            instance.Length(attribute.Length);
        }
    }
}