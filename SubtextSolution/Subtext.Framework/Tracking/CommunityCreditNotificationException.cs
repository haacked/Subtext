using System;

namespace Subtext.Framework.Tracking
{
    public class CommunityCreditNotificationException : ApplicationException
    {
        public CommunityCreditNotificationException()
        {
        }

        public CommunityCreditNotificationException(string message) : base(message)
        {
        }

        public CommunityCreditNotificationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}