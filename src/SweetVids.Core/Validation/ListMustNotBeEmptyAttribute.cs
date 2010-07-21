#region Using Directives

using System;
using System.Collections;

#endregion

namespace SweetVids.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ListMustNotBeEmptyAttribute : ValidationAttribute
    {
        protected override void validate(object target, object rawValue, INotification notification)
        {
            if (rawValue == null || ((IList) rawValue).Count == 0)
            {
                logMessage(notification, Notification.LIST_MUST_NOT_BE_EMPTY);
                return;
            }
        }
    }
}