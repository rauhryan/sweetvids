using System;
using System.Linq.Expressions;

namespace SweetVids.Core.Validation
{
    public interface IValidator
    {
        INotification Validate(object target);
        NotificationMessage[] ValidateByField(object target, string propertyName);
        NotificationMessage[] ValidateByField<T>(T target, Expression<Func<T, object>> expression);
    }
}