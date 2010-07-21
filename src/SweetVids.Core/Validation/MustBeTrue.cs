#region Using Directives

using System;

#endregion

namespace SweetVids.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MustBeTrue : ValidationAttribute
    {
        private string _message;

        protected override void validate(object target, object value, INotification notification)
        {
            if ((value is bool ? (bool) value : false) != true)
            {
                logMessage(notification, _message ?? Notification.MUST_BE_TRUE);
                return;
            }
        }

        public MustBeTrue()
        {
        }

        public MustBeTrue(string message)
        {
            _message = message;
        }
    }
}