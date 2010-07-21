#region Using Directives

using System;

#endregion

namespace SweetVids.Core.Validation
{
    public enum Severity
    {
        Warning,
        Error,
        Status
    }

    [Serializable]
    public class NotificationMessage : IComparable
    {
        private string _fieldName;
        private string _message;
        private Severity _severity = Severity.Error;

        public NotificationMessage()
        {
        }

        public NotificationMessage(string fieldName, string message)
        {
            _fieldName = fieldName;
            _message = message;
        }

        public Severity Severity
        {
            get { return _severity; }
            set { _severity = value; }
        }

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            var message = (NotificationMessage) obj;
            if (FieldName == message.FieldName)
            {
                return Message.CompareTo(message.Message);
            }
            else
            {
                return FieldName.CompareTo(message.FieldName);
            }
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            var notificationMessage = obj as NotificationMessage;
            if (notificationMessage == null) return false;
            return
                Equals(_fieldName, notificationMessage._fieldName) &&
                Equals(_message, notificationMessage._message) &&
                Equals(_severity, notificationMessage._severity);
        }

        public override int GetHashCode()
        {
            return
                (_fieldName != null ? _fieldName.GetHashCode() : 0) + 29*(_message != null ? _message.GetHashCode() : 0);
        }


        public override string ToString()
        {
            return string.Format("Field {0}:  {1}", _fieldName, _message);
        }

        public void Substitute(string substitution, string alias)
        {
            _message = _message.Replace(substitution, alias);
        }
    }
}