#region Using Directives

using System;
using System.Collections.Generic;

#endregion

namespace SweetVids.Core.Validation
{
    [Serializable]
    public class MessageBag
    {
        private readonly string _fieldName;
        private readonly List<NotificationMessage> _list = new List<NotificationMessage>();

        public MessageBag()
        {
        }

        public MessageBag(string fieldName)
        {
            _fieldName = fieldName;
        }

        public string FieldName
        {
            get { return _fieldName; }
        }

        public NotificationMessage[] Messages
        {
            get { return _list.ToArray(); }
        }

        public void Add(NotificationMessage message)
        {
            _list.Add(message);
        }

        public bool Contains(NotificationMessage message)
        {
            return _list.Contains(message);
        }
    }
}