#region Using Directives

using System;

#endregion

namespace SweetVids.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidatedChildAttribute : ValidationAttribute
    {
        protected override void validate(object target, object rawValue, INotification notification)
        {
            INotification child = Validator.ValidateObject(rawValue);
            notification.AddChild(PropertyName, child);
        }
    }
}