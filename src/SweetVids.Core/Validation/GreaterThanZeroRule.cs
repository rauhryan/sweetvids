#region Using Directives

using FubuCore.Reflection;

#endregion

namespace SweetVids.Core.Validation
{
    public sealed class GreaterThanZeroRule : IValidationRule
    {
        private readonly PropertyMatch _match;
        private readonly Accessor _property;


        public GreaterThanZeroRule(PropertyMatch match, Accessor property)
        {
            _match = match;
            _property = property;
        }

        #region IValidationRule Members

        public void Validate(object target, Notification notification)
        {
            if (!_match.Matches(target))
            {
                return;
            }

            var attribute = new GreaterThanZeroAttribute();
            attribute.Property = _property.InnerProperty;
            attribute.Validate(target, notification);
        }


        public string Message { get; set; }

        #endregion
    }
}