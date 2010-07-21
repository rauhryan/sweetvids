#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Utils;

#endregion

namespace SweetVids.Core.Validation
{
    public class Validator : IValidator
    {
        private static readonly Dictionary<Type, List<ValidationAttribute>> _attributeDictionary
            = new Dictionary<Type, List<ValidationAttribute>>();

        private static readonly object _locker = new object();

        #region IValidator Members

        public INotification Validate(object target)
        {
            return ValidateObject(target);
        }

        NotificationMessage[] IValidator.ValidateByField<T>(T target, Expression<Func<T, object>> expression)
        {
            var name = ReflectionHelper.GetProperty(expression).Name;
            return ValidateField(target, name);
        }

        NotificationMessage[] IValidator.ValidateByField(object target, string propertyName)
        {
            return ValidateField(target, propertyName);
        }

        #endregion

        public static List<ValidationAttribute> FindAttributes(Type type)
        {
            var atts = new List<ValidationAttribute>();

            foreach (var property in type.GetProperties())
            {
                Attribute[] attributes = Attribute.GetCustomAttributes(property, typeof (ValidationAttribute));
                foreach (ValidationAttribute attribute in attributes)
                {
                    attribute.Property = property;
                    atts.Add(attribute);
                }
            }

            return atts;
        }

        public static INotification ValidateObject(object target)
        {
            if (target == null)
            {
                return new Notification();
            }

            List<ValidationAttribute> atts = scanType(target.GetType());
            var notification = new Notification();

            if (target is IValidated)
            {
                ((IValidated) target).Validate(notification);
            }

            foreach (var att in atts)
            {
                att.Validate(target, notification);
            }

            return notification;
        }

        private static List<ValidationAttribute> scanType(Type type)
        {
            if (!_attributeDictionary.ContainsKey(type))
            {
                lock (_locker)
                {
                    if (!_attributeDictionary.ContainsKey(type))
                    {
                        _attributeDictionary.Add(type, FindAttributes(type));
                    }
                }
            }

            return _attributeDictionary[type];
        }

        public static void AssertValid(object target)
        {
            INotification notification = ValidateObject(target);
            if (!notification.IsValid())
            {
                string message = string.Format("{0} was not valid", target);
                throw new ApplicationException(message);
            }
        }

        public static NotificationMessage[] ValidateField(object target, string propertyName)
        {
            List<ValidationAttribute> atts = scanType(target.GetType());
            List<ValidationAttribute> list =
                atts.FindAll(delegate(ValidationAttribute att) { return att.PropertyName == propertyName; });

            var notification = new Notification();
            foreach (var attribute in list)
            {
                attribute.Validate(target, notification);
            }

            if (target is IValidated)
            {
                ((IValidated) target).Validate(notification);
            }

            return notification.GetMessages(propertyName);
        }
    }
}