#region Using Directives

using System;

#endregion

namespace SweetVids.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class GreaterThanZeroAttribute : ValidationAttribute
    {
        protected override void validate(object target, object rawValue, INotification notification)
        {
            if (rawValue == null)
            {
                return;
            }

            decimal decimalValue = Convert.ToDecimal(rawValue);
            if (decimalValue <= 0)
            {
                logMessage(notification, Notification.MUST_BE_GREATER_THAN_ZERO);
            }
        }
    }
}