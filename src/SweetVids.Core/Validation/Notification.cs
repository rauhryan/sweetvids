#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace SweetVids.Core.Validation
{
    public interface INotification
    {
        NotificationMessage[] AllMessages { get; }
        bool IsValid();
        NotificationMessage RegisterMessage(string fieldName, string message, Severity severity);
        bool IsValid(string fieldName);
        void Include(Notification peer);
        NotificationMessage[] GetMessages(string fieldName);
        void AddChild(string propertyName, INotification notification);
        INotification GetChild(string propertyName);
        bool HasMessage(string fieldName, string messageText);
        void AliasFieldInMessages(string fieldName, string alias);
        void AssertValid();
        MessageBag MessagesFor(string fieldName);
        Notification Flatten();
        bool IsTopLevelValid();
        void addMessages(StringBuilder sb);
        void gather(List<NotificationMessage> list);
    }

    [Serializable]
    public class Notification : INotification
    {
        #region statics

        public static Notification Valid()
        {
            return new Notification();
        }

        public static Notification Invalid()
        {
            var returnValue = new Notification();
            returnValue.RegisterMessage("something", "something else", Severity.Error);

            return returnValue;
        }

        #endregion

        public static readonly string INVALID_EMAIL = "Invalid Email Address";
        public static readonly string INVALID_EMAIL_DOMAIN = "Invalid(Nonexistant?) Email Domain";
        public static readonly string INVALID_FORMAT = "Invalid Format";
        public static readonly string LIST_MUST_NOT_BE_EMPTY = "Empty list when list is required to have some items";
        public static readonly string MUST_BE_GREATER_OR_EQUAL_TO_ZERO = "Must be greater or equal to zero";
        public static readonly string MUST_BE_GREATER_THAN_ZERO = "Must be a positive number";
        public static readonly string MUST_BE_TRUE = "This box must be checked";
        public static readonly string REQUIRED_FIELD = "Required Field";
        public static readonly string SYSTEM_FAILURE = "System Failure";
        private readonly Dictionary<string, MessageBag> _bags = new Dictionary<string, MessageBag>();

        private readonly Dictionary<string, INotification> _children = new Dictionary<string, INotification>();
        private readonly List<NotificationMessage> _list = new List<NotificationMessage>();

        #region INotification Members

        public NotificationMessage[] AllMessages
        {
            get
            {
                var list = new List<NotificationMessage>();
                gather(list);

                var returnValue = new Notification();
                returnValue._list.AddRange(list);

                //_list.Sort();
                //returnValue._list.AddRange(_list.ToArray());

                return returnValue._list.ToArray();
            }
        }

        public void Include(Notification peer)
        {
            _list.AddRange(peer.AllMessages);
        }

        public bool IsValid()
        {
            foreach (var pair in _children)
            {
                if (!pair.Value.IsValid())
                {
                    return false;
                }
            }

            return _list.Count == 0;
        }

        public NotificationMessage RegisterMessage(string fieldName, string message, Severity severity)
        {
            var notificationMessage = new NotificationMessage(fieldName, message);
            notificationMessage.Severity = severity;

            if (!_list.Contains(notificationMessage))
            {
                _list.Add(notificationMessage);

                MessagesFor(notificationMessage.FieldName).Add(notificationMessage);
            }

            return notificationMessage;
        }

        public NotificationMessage[] GetMessages(string fieldName)
        {
            List<NotificationMessage> messages =
                _list.FindAll(m => m.FieldName == fieldName);

            return messages.ToArray();
        }

        public void AddChild(string propertyName, INotification notification)
        {
            if (_children.ContainsKey(propertyName))
            {
                _children[propertyName] = notification;
            }
            else
            {
                _children.Add(propertyName, notification);
            }
        }

        public INotification GetChild(string propertyName)
        {
            if (_children.ContainsKey(propertyName))
            {
                return _children[propertyName];
            }

            return Valid();
        }


        public bool HasMessage(string fieldName, string messageText)
        {
            var message = new NotificationMessage(fieldName, messageText);
            return _list.Contains(message);
        }

        public void AliasFieldInMessages(string fieldName, string alias)
        {
            string substitution = string.Format("[{0}]", fieldName);
            foreach (var message in _list)
            {
                message.Substitute(substitution, alias);
            }
        }

        public bool IsValid(string fieldName)
        {
            foreach (var pair in _children)
            {
                if (!pair.Value.IsValid(fieldName))
                {
                    return false;
                }
            }

            return GetMessages(fieldName).Length == 0;
        }


        public void AssertValid()
        {
            if (!IsValid())
            {
                var sb = new StringBuilder();
                sb.AppendLine("Validation Failures");

                addMessages(sb);

                throw new ApplicationException(sb.ToString());
            }
        }

        public void addMessages(StringBuilder sb)
        {
            foreach (var message in _list)
            {
                sb.AppendLine(message.ToString());
            }

            foreach (var pair in _children)
            {
                sb.AppendLine("Properties from " + pair.Key);
                pair.Value.addMessages(sb);
            }
        }

        public MessageBag MessagesFor(string fieldName)
        {
            if (!_bags.ContainsKey(fieldName))
            {
                var bag = new MessageBag(fieldName);
                _bags.Add(fieldName, bag);
            }

            return _bags[fieldName];
        }

        public Notification Flatten()
        {
            var list = new List<NotificationMessage>();
            gather(list);

            var returnValue = new Notification();
            returnValue._list.AddRange(list);

            return returnValue;
        }

        public void gather(List<NotificationMessage> list)
        {
            list.AddRange(_list);
            foreach (var pair in _children)
            {
                pair.Value.gather(list);
            }
        }

        public bool IsTopLevelValid()
        {
            return _list.FindAll(m => m.Severity == Severity.Error).Count == 0;
        }

        #endregion

        public void ForEachField(Action<MessageBag> action)
        {
            foreach (var bag in _bags.Values)
            {
                action(bag);
            }
        }
    }
}