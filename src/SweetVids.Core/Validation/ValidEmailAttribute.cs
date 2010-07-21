#region Using Directives

using System;
using System.Net;
using System.Text.RegularExpressions;
using FubuMVC.Core;

#endregion

namespace SweetVids.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidEmailAttribute : ValidationAttribute
    {
        private const string regexPattern = @"^(([^<>()[\]\\.,;:\s@\""]+"
                                            + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                                            + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                                            + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                                            + @"[a-zA-Z]{2,}))$";

        protected override void validate(object target, object rawValue, INotification notification)
        {
            if (rawValue == null)
            {
                logMessage(notification, Notification.INVALID_EMAIL);
                return;
            }
            if (!IsValidEmailAddress(rawValue.ToString()))
            {
                logMessage(notification, Notification.INVALID_EMAIL);
                return;
            }
        }

        private static bool IsValidEmailAddress(string value)
        {
            /*if (!IsValidEmailDomain(value))
            {
                return false;
            }*/
            return (new Regex(regexPattern).IsMatch(value));
        }
    }
}