#region Using Directives

using System;
using System.Reflection;

#endregion

namespace SweetVids.Core.Validation
{
    public sealed class MaximumStringLengthAttribute : ValidationAttribute
    {
        public static Func<MaximumStringLengthAttribute, PropertyInfo, string> GetMessage = (att, prop) => string.Format("{0} cannot be longer than {1} characters", att.PropertyName, att.Length);

        private readonly int _length;

        public MaximumStringLengthAttribute(int length)
        {
            _length = length;
        }

        public int Length
        {
            get { return _length; }
        }

        protected override void validate(object target, object rawValue, INotification notification)
        {
            if (rawValue == null)
            {
                return;
            }

            if (rawValue.ToString().Length > _length)
            {
                string message = GetMessage(this, Property);
                logMessage(notification, message);
            }
        }

        public static int GetLength(PropertyInfo property)
        {
            if (property.PropertyType != typeof (string))
            {
                return 0;
            }

            var attribute =
                GetCustomAttribute(property, typeof (MaximumStringLengthAttribute)) as MaximumStringLengthAttribute;

            return attribute == null ? 100 : attribute._length;
        }
    }
}