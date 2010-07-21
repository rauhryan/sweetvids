#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FubuCore.Reflection;

#endregion

namespace SweetVids.Core.Validation
{
    public class ValidationRules<T>
    {
        private readonly List<IValidationRule> _rules = new List<IValidationRule>();

        public void ExecuteRules(Notification notification, object target)
        {
            foreach (var rule in _rules)
            {
                rule.Validate(target, notification);
            }
        }

        public Notification Validate(T target)
        {
            var notification = new Notification();

            foreach (var rule in _rules)
            {
                rule.Validate(target, notification);
            }

            return notification;
        }

        public PropertyRuleExpression IfProperty(Expression<Func<T, object>> expression)
        {
            Accessor accessor = ReflectionHelper.GetAccessor(expression);
            return new PropertyRuleExpression(_rules, accessor);
        }

        #region Nested type: PropertyRuleExpression

        public class PropertyRuleExpression
        {
            private readonly Accessor _matchAccessor;
            private readonly List<IValidationRule> _rules;
            private object _matching;
            private Accessor _target;

            public PropertyRuleExpression()
            {
            }

            public PropertyRuleExpression(List<IValidationRule> rules, Accessor accessor)
            {
                _rules = rules;
                _matchAccessor = accessor;
            }

            public new PropertyRuleExpression Equals(object matching)
            {
                _matching = matching;
                return this;
            }

            public PropertyRuleExpression Property(Expression<Func<T, object>> expression)
            {
                _target = ReflectionHelper.GetAccessor(expression);
                return this;
            }

            public void ShouldBeGreaterThanZero()
            {
                var rule = new GreaterThanZeroRule(new PropertyMatch(_matchAccessor, _matching), _target);
                _rules.Add(rule);
            }

            public PropertyRuleExpression ShouldBeRequired()
            {
                var rule = new RequiredRule(new PropertyMatch(_matchAccessor, _matching), _target);
                _rules.Add(rule);
                return this;
            }

            public void WithMessage(string message)
            {
                _rules[_rules.Count - 1].Message = message;
            }
        }

        #endregion
    }

    public class RequiredRule : IValidationRule
    {
        private readonly PropertyMatch _match;
        private readonly Accessor _target;
        private string _message = Notification.REQUIRED_FIELD;

        public RequiredRule(PropertyMatch match, Accessor target)
        {
            _match = match;
            _target = target;
        }

        #region IValidationRule Members

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public void Validate(object target, Notification notification)
        {
            if (!_match.Matches(target))
            {
                return;
            }

            var attribute = new RequiredAttribute();
            attribute.Property = _target.InnerProperty;
            attribute.Message = _message;
            attribute.Validate(target, notification);
        }

        #endregion
    }
}