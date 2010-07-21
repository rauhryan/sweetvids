using System;
using System.Text.RegularExpressions;

namespace SweetVids.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ValidYearAttribute : ValidationAttribute
    {
        private const string errorMessage = "You must enter a valid 4-digit year.";

        protected override void validate(object target, object rawValue, INotification notification)
        {
            if (rawValue == null)
            {
                logMessage(notification, errorMessage);
                return;
            }

            if (rawValue.ToString().Length != 4)
            {
                logMessage(notification, errorMessage);
                return;
            }

            if (rawValue is string)
            {
                var value = rawValue as string;
                if (!Regex.IsMatch(value, @"^((19)|(20))\d{2}$"))
                    logMessage(notification, errorMessage);
            }
        }
    }
}