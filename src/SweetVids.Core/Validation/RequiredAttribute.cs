#region Using Directives

using System;
using System.Reflection;

#endregion

namespace SweetVids.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAttribute : ValidationAttribute
    {
        public static Func<RequiredAttribute, PropertyInfo, string> GetMessage = (att, prop) =>
                                                                                     {
                                                                                         if (!string.IsNullOrEmpty(att.Message))
                                                                                         {
                                                                                             return att.Message;
                                                                                         }

                                                                                         return Notification.REQUIRED_FIELD;
                                                                                     };

        public string Message { get; set; }

        protected override void validate(object target, object rawValue, INotification notification)
        {

            if (rawValue == null || rawValue.ToString() == string.Empty)
            {
                logMessage(notification, GetMessage(this, Property));
            }
        }

        public static bool IsRequired(PropertyInfo property)
        {
            var attribute = GetCustomAttribute(property, typeof (RequiredAttribute)) as RequiredAttribute;
            return attribute != null && attribute.GetType().Equals(typeof (RequiredAttribute));
        }
    }
}