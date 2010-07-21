#region Using Directives

using System;
using System.Reflection;

#endregion

namespace SweetVids.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValidationAttribute : Attribute
    {
        private PropertyInfo _property;
        private Severity _severity = Severity.Error;

        public PropertyInfo Property
        {
            get { return _property; }
            set { _property = value; }
        }

        public string PropertyName
        {
            get { return _property.Name; }
        }

        public Severity Severity
        {
            get { return _severity; }
            set { _severity = value; }
        }

        public void Validate(object target, INotification notification)
        {
            object rawValue = _property.GetValue(target, null);
            validate(target, rawValue, notification);
        }

        protected void logMessage(INotification notification, string message)
        {
            notification.RegisterMessage(Property.Name, message, _severity);
        }

        protected abstract void validate(object target, object rawValue, INotification notification);
    }
}