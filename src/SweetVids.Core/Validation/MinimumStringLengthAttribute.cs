#region Using Directives

using System;
using System.Reflection;

#endregion

namespace SweetVids.Core.Validation
{
    public sealed class MinimumStringLengthAttribute : ValidationAttribute
    {
        public static Func<MinimumStringLengthAttribute, PropertyInfo, string> GetMessage = (att, prop) => string.Format("{0} cannot be shorter than {1} characters", att.PropertyName, att.Length);

        private readonly int _length;

        public MinimumStringLengthAttribute(int length)
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

            if (rawValue.ToString().Length < _length)
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
                GetCustomAttribute(property, typeof(MinimumStringLengthAttribute)) as MinimumStringLengthAttribute;

            return attribute == null ? 1 : attribute._length;
        }
    }
}