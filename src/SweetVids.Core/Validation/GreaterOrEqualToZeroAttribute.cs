#region Using Directives

using System;

#endregion

namespace SweetVids.Core.Validation
{
    public sealed class GreaterOrEqualToZeroAttribute : ValidationAttribute
    {
        protected override void validate(object target, object rawValue, INotification notification)
        {
            if (rawValue == null)
            {
                return;
            }

            double doubleValue = Convert.ToDouble(rawValue);
            if (doubleValue < 0)
            {
                logMessage(notification, Notification.MUST_BE_GREATER_OR_EQUAL_TO_ZERO);
            }
        }
    }
}