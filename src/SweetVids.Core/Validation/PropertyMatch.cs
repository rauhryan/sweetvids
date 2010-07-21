using FubuCore.Reflection;

namespace SweetVids.Core.Validation
{
    public class PropertyMatch
    {
        private readonly Accessor _property;
        private readonly object _value;

        public PropertyMatch(Accessor property, object value)
        {
            _property = property;
            _value = value;
        }

        public bool Matches(object target)
        {
            return _property.GetValue(target).Equals(_value);
        }
    }
}